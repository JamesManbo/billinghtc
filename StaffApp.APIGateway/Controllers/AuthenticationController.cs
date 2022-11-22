using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.Authentication;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StaffApp.APIGateway.Infrastructure.Helpers;
using StaffApp.APIGateway.Infrastructure.ValidationConfigs;
using StaffApp.APIGateway.Models;
using StaffApp.APIGateway.Models.AuthModels;
using StaffApp.APIGateway.Models.FCMModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    /// <summary>
    /// API xác thực
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOtpService _otpService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IAuthenticationService authenticationService, IOtpService otpService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _otpService = otpService;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IActionResponse<UserDTO>>> Get()
        {
            if (string.IsNullOrEmpty(UserIdentity.UniversalId))
            {
                return BadRequest();
            }
            var rs = await _authenticationService.FindUserById(UserIdentity.UniversalId);
            if (rs == null) return NotFound();
            return Ok(rs);
        }


        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Access token</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("Login")]
        public async Task<ActionResult<IActionResponse<SignInResultDTO>>> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                //lấy ra danh sách người dùng từ AD
                if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
                {
                    var signinUser = await _authenticationService.LoginByAD(model.UserName, model.Password);
                    if (signinUser)
                    {
                        return Ok(new SignInResultDTO { Token= await _authenticationService.GenerateJwtToken(model.UserName) });
                    }
                }
                var signinResult =
                        await _authenticationService.Login(new LoginRequest { Password = model.Password, UserName = model.UserName, RememberMe = model.RememberMe});

                if (signinResult.Succeeded)
                {
                    return Ok(new SignInResultDTO { Token = await _authenticationService.GenerateJwtToken(model.UserName) });
                }

                if (!signinResult.Succeeded)
                {
                    var signinUser = await _authenticationService.LoginByAD(model.UserName, model.Password);
                    if (signinUser)
                    {
                        return Ok(new SignInResultDTO { Token = await _authenticationService.GenerateJwtToken(model.UserName) });
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

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Thông báo</returns>
        /// <response code="201">Thông báo thay đổi mk thành công</response>
        /// <response code="400">Thông báo kèm danh sách lỗi</response> 
        [HttpPut]
        [Route("ChangePass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IActionResponse<ChangePasswordResult>>> PutPass(DraftChangePasswordRequest command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validator = new ChangePasswordValidator();
            var validateResult = await validator.ValidateAsync(command);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors[0].ErrorMessage);
            }

            command.UserName = UserIdentity.UserName;
            var actResponse = await _authenticationService.ChangePassword(command);
            if (actResponse != null && actResponse.Succeeded)
            {
                return Ok(new ActionResponse { Message = "Đổi mật khẩu thành công" });
            }
            if (actResponse.Errors != null && actResponse.Errors.Count > 0)
            {
                return BadRequest(actResponse.Errors[0].Description);
            }

            return BadRequest(actResponse);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ForgotPass")]
        public async Task<ActionResult<IActionResponse>> ForgotPass(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return BadRequest("");
            var actResponse = await _authenticationService.ForgotPassword(userName);
            if (actResponse != null)
            {
                if (actResponse.Succeeded)
                    return Ok(new { Message = actResponse.Message, DateExpired = actResponse.DateExpired });
                else return BadRequest(actResponse.Message);
            }

            return BadRequest("");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassWithOtp")]
        public async Task<ActionResult<IActionResponse>> ForgotPasswordWithOtp(ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Otp))
            {
                return BadRequest("");
            }
            var rs = await _otpService.FindOtpByUserName(request.UserName);
            if (rs != null)
            {
                if (!rs.Otp.Equals(request.Otp))
                {
                    return BadRequest("Mã otp không đúng");
                }
                if (rs.DateExpired < DateTime.Now)
                {
                    return BadRequest("Mã otp đã quá hạn");
                }
                if (rs.IsUse.HasValue && rs.IsUse.Value)
                {
                    return BadRequest("Mã otp đã dùng");
                }

                await _otpService.UpdateOtpUsed(rs.Id);

                var code = request.UserName + "|" + DateTime.Now.AddMinutes(2).ToString();
                return Ok(EncryptionHelper.Encrypt(code, _configuration.GetValue<string>("MD5TokenCryptoKey")));

            }
            else
            {
                return BadRequest("Mã otp không tồn tại");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ChangePassWithCode")]
        public async Task<ActionResult<IActionResponse>> ChangePassWithCode(ChangePasswordWithCodeRequest command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var changePasswordRequest = new DraftChangePasswordRequest()
            {
                UserName = command.UserName,
                NewPassword = command.NewPassword,
                ConfirmPassword = command.ConfirmPassword,
                OldPassword = "Default" //qua validator
            };

            var validator = new ChangePasswordValidator();
            var validateResult = await validator.ValidateAsync(changePasswordRequest);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors[0].ErrorMessage);
            }
            var strCrypt = EncryptionHelper.Decrypt(command.Code , _configuration.GetValue<string>("MD5TokenCryptoKey"));
            var objs = strCrypt.Split('|');
            if (objs==null || objs.Length!=2 || !objs[0].Equals(command.UserName))
            {
                return BadRequest("Mã không đúng");
            }
            if (DateTime.TryParse( objs[1], out var expriedDate))
            {
                if (DateTime.Now > expriedDate)
                {
                    return BadRequest("Mã đã quá hạn");
                }
            }

            changePasswordRequest.UserName = command.UserName;
            var actResponse = await _authenticationService.ChangePasswordWithoutOldPassword(changePasswordRequest);
            if (actResponse != null && actResponse.Succeeded)
            {
                return Ok(new ActionResponse { Message = "Đổi mật khẩu thành công" });
            }
            if (actResponse.Errors != null && actResponse.Errors.Count > 0)
            {
                return BadRequest(actResponse.Errors[0].Description);
            }

            return BadRequest(actResponse);

        }

        [HttpPost]
        [Route("RegisterFCM")]
        public async Task<ActionResult<IActionResponse<bool>>> RegisterFCM(RegisterFcmTokenCommand request)
        {
            if (string.IsNullOrEmpty(UserIdentity.UniversalId))
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest();
            }

            request.ReceiverId = UserIdentity.UniversalId;
            var rs = await _authenticationService.RegisterFCM(request);
            return Ok(rs);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UnRegisterFCM")]
        public async Task<ActionResult<IActionResponse<bool>>> UnRegisterFCM(RegisterFcmTokenCommand request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest();
            }
            var rs = await _authenticationService.UnRegisterFCM(request.Token);
            return Ok(rs);
        }
    }
}