using System;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Infrastructure.Services;
using SystemUserIdentity.API.Models.AccountViewModels;
using SystemUserIdentity.API.Models.ManageViewModels;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Infrastructure.Queries;
using SystemUserIdentity.API.Grpc.ClientServices;
using Global.Models.StateChangedResponse;
using Microsoft.Extensions.Hosting;

namespace SystemUserIdentity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = AuthenticationSchemes.CmsApiIdentityKey)]
    public class AuthenticationController : CustomBaseController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService<User> _authenticationService;
        private readonly IUserQueries _userQueries;
        private readonly IProjectGrpcService _projectGrpcService;

        public AuthenticationController(ILogger<AuthenticationController> logger,
            IAuthenticationService<User> authenticationService,
            IUserQueries userQueries,
            IProjectGrpcService projectGrpcService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userQueries = userQueries;
            _projectGrpcService = projectGrpcService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = _userQueries.FindById(UserIdentity.Id);
            if (currentUser != null)
            {
                var responsibleProjects = await _projectGrpcService.GetProjectsBySupporter(currentUser.IdentityGuid.ToString());
                currentUser.ProjectIds = responsibleProjects?.ProjectDtos.Select(p => p.Id).ToArray();
            }
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ValidPassword")]
        public IActionResult ValidPassword(string rawPassword)
        {
            return Ok(_authenticationService.EncryptPassword(rawPassword));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actResponse =
                await _authenticationService.CreateAsync(registerViewModel.User, registerViewModel.Password);
            if (actResponse.Errors.Any())
            {
                return BadRequest(actResponse);
            }

            return Ok(actResponse);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                //lấy ra danh sách người dùng từ AD
                if (environment == Environments.Production)
                {
                    if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
                    {
                        var signinUser = await _authenticationService.LoginByAD(model.UserName, model.Password);
                        if (signinUser)
                        {
                            return Ok(_authenticationService.GenerateJwtToken(model.UserName));
                        }
                    }
                }

                var signinResult =
                        await _authenticationService.PasswordSignInAsync(
                            model.UserName,
                            model.Password,
                            model.RememberMe,
                            false);

                if (signinResult.Succeeded)
                {
                    return Ok(_authenticationService.GenerateJwtToken(model.UserName));
                }

                if (!signinResult.Succeeded && environment == Environments.Production)
                {
                    var signinUser = await _authenticationService.LoginByAD(model.UserName, model.Password);
                    if (signinUser)
                    {
                        return Ok(_authenticationService.GenerateJwtToken(model.UserName));
                    }
                }

                if (signinResult.IsLockedOut)
                {
                    return BadRequest(
                        "Tài khoản hiện đã bị khóa, vui lòng liên hệ với quản trị viên để biết thêm thông tin.");
                }

                if (signinResult.IsNotExisted)
                {
                    return BadRequest("Tài khoản không tồn tại hoặc đã bị xóa khỏi hệ thống.");
                }

                return BadRequest("Tài khoản hoặc mật khẩu không đúng, vui lòng thử lại");
            }

            return BadRequest();
        }

        [HttpPut, Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel changePassModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actResponse = await _authenticationService.ChangePassword(UserIdentity.UserName,
                changePassModel.OldPassword, changePassModel.NewPassword);
            if (actResponse.Succeeded)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpPut, Route("change-password-user")]
        public async Task<IActionResult> ChangePasswordUser([FromBody] ChangePasswordUserViewModel changePassUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actionResponse = new ActionResponse<ChangePasswordUserViewModel>();
            if (changePassUserModel.NewPassword == null || changePassUserModel.NewPassword == ""
                || changePassUserModel.ConfirmPassword == null || changePassUserModel.ConfirmPassword == "")
            {
                if (changePassUserModel.NewPassword == null || changePassUserModel.NewPassword == "")
                {
                    actionResponse.AddError("Mật khẩu mới không được bỏ trống", nameof(changePassUserModel.NewPassword));
                }
                if (changePassUserModel.ConfirmPassword == null || changePassUserModel.ConfirmPassword == "")
                {
                    actionResponse.AddError("Xác nhận mật khẩu mới không được bỏ trống", nameof(changePassUserModel.ConfirmPassword));
                }
                return BadRequest(actionResponse);
            }
            else
            {
                var check = string.Compare(changePassUserModel.NewPassword, changePassUserModel.ConfirmPassword);
                if (check == 0)
                {
                    changePassUserModel.UserName = (changePassUserModel.Permission == "CHANGE_PASSWORD_PARTNER" || changePassUserModel.Permission == "CHANGE_PASSWORD_USER") ? changePassUserModel.UserName : UserIdentity.UserName;
                    var actResponse = await _authenticationService.ChangePasswordUser(changePassUserModel.UserName, changePassUserModel.NewPassword);
                    if (actResponse.Succeeded)
                    {
                        return Ok(actResponse);
                    }
                    return BadRequest(actResponse);
                }
                else
                {
                    actionResponse.AddError("Xác nhận mật khẩu mới không đúng", nameof(changePassUserModel.ConfirmPassword));
                    return BadRequest(actionResponse);
                }
            }
        }
    }
}