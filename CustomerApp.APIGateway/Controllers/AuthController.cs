using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.APIGateway.Infrastructure.Helpers;
using CustomerApp.APIGateway.Infrastructure.Validations;
using CustomerApp.APIGateway.Models.AuthModels;
using CustomerApp.APIGateway.Models.FCMModels;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : CustomerBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOtpService _otpService;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthenticationService authenticationService, IOtpService otpService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _otpService = otpService;
            _configuration = configuration;
        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ActionResult<IActionResponse<LoginResultDTO>>> Login(LoginRequest model)
        {
            var validator = new LoginValidator();
            var validateResult = await validator.ValidateAsync(model);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors.ElementAt(0).ErrorMessage);
            }


            var loginRs = await _authenticationService.Login(model);

            if (loginRs!=null && loginRs.Succeeded)
            {
                return Ok(loginRs.Token);
            }
            return BadRequest(loginRs?.Message??"");


        }

        [HttpPut()]
        [Route("ChangePassword")]
        public async Task<ActionResult<IActionResponse<ChangePasswordResultDTO>>> ChangePassword(ChangePasswordRequest model)
        {
            var validator = new ChangePasswordValidator();
            var validateResult = await validator.ValidateAsync(model);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors.ElementAt(0).ErrorMessage);
            }

            model.UserId = UserIdentity.Id;
            var changePass = await _authenticationService.ChangePassword(model);
            if (changePass != null && changePass.Succeeded)
            {
                return Ok(new ActionResponse { Message = "Đổi mật khẩu thành công" });
            }
            return BadRequest(changePass?.Message ?? "");
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

            var changePasswordRequest = new ChangePasswordByUserNameRequest()
            {
                UserName = command.UserName,
                NewPassword = command.NewPassword,
                ConfirmPassword = command.ConfirmPassword,
            };

            var validator = new ChangePasswordByUserNameValidator();
            var validateResult = await validator.ValidateAsync(changePasswordRequest);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors[0].ErrorMessage);
            }
            var strCrypt = EncryptionHelper.Decrypt(command.Code, _configuration.GetValue<string>("MD5TokenCryptoKey"));
            var objs = strCrypt.Split('|');
            if (objs == null || objs.Length != 2 || !objs[0].Equals(command.UserName))
            {
                return BadRequest("Mã không đúng");
            }
            if (DateTime.TryParse(objs[1], out var expriedDate))
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
            }else if (actResponse != null && !actResponse.Succeeded)
            {
                return BadRequest(actResponse.Message);
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
