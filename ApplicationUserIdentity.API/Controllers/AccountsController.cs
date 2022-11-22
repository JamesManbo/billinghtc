using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Application.Commands.SynchronizeData;
using ApplicationUserIdentity.API.Infrastructure.Helper;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Infrastructure.Validations;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.Authentication;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.Services.GRPC.Clients;
using AutoMapper;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApplicationUserIdentity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _accountRepository;
        private readonly IContractorGrpcService _contractorGrpcService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AccountsController(IUserRepository accountRepository
            , IMapper mapper
            , IOptions<AppSettings> appSettings
            , IContractorGrpcService contractorGrpcService
            , IMediator mediator)
        {
            this._accountRepository = accountRepository;
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._contractorGrpcService = contractorGrpcService;
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _contractorGrpcService.GetFromId(2));
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            var actionResponse = await this._mediator
                .Send(new SyncContractorFromBulkInsertContractCommand(2));
            if (actionResponse.IsSuccess)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost()]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                return BadRequest("Vui lòng nhập tài khoản.");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Vui lòng nhập mật khẩu.");
            }

            var user = await _accountRepository.FindByUserNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Tài khoản không tồn tại.");
            }
            var encryptedPassword = $"{user.PasswordSalt}{model.Password}".EncryptMD5(_appSettings.MD5CryptoKey);
            if (encryptedPassword != user.Password)
            {
                return BadRequest("Mật khẩu không đúng, vui lòng thử lại.");
            }

            //var signinResult =
            //    await _authenticationService.PasswordSignInAsync(model.UserName, model.Password, false,
            //        false);

            //if (signinResult.Succeeded)
            //{
            //    return Ok(await _authenticationService.GenerateJwtToken(model.UserName));
            //}

            return Ok(_accountRepository.GenerateJwtToken(user));
        }

        [HttpPut()]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var validator = new ChangePasswordValidator();
            var validateResult = await validator.ValidateAsync(model);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors.ElementAt(0).ErrorMessage);
            }

            var user = await _accountRepository.GetByIdAsync(UserIdentity.Id);
            if (user == null)
            {
                return BadRequest("Tài khoản không tồn tại.");
            }
            var encryptedOldPassword = $"{user.PasswordSalt}{model.OldPassword}".EncryptMD5(_appSettings.MD5CryptoKey);
            if (encryptedOldPassword != user.Password)
            {
                return BadRequest("Mật khẩu cũ không đúng.");
            }
            var encryptedNewPassword = $"{user.PasswordSalt}{model.NewPassword}".EncryptMD5(_appSettings.MD5CryptoKey);
            user.Password = encryptedNewPassword;
            var actionResponse = await _accountRepository.UpdateAndSave(user);
            return Ok(new ActionResponse { Message = "Đổi mật khẩu thành công" });
        }
    }
}