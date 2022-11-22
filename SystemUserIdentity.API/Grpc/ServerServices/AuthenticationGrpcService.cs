using AutoMapper;
using ContractManagement.API.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using OrganizationUnit.Domain.AggregateModels.OTPAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories.FCMRepository;
using OrganizationUnit.Infrastructure.Repositories.OtpRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Grpc.Clients;
using SystemUserIdentity.API.Grpc.ClientServices;
using SystemUserIdentity.API.Infrastructure.Helpers;
using SystemUserIdentity.API.Infrastructure.Services;
using SystemUserIdentity.API.Proto;

namespace SystemUserIdentity.API.Grpc.ServerServices
{

    public class AuthenticationGrpcService : AuthenticationGrpc.AuthenticationGrpcBase
    {
        private readonly IAuthenticationService<User> _authenticationService;
        private readonly IFCMTokenQueries _fcmTokenQueries;
        private readonly IFCMTokenRepository _fcmTokenRepository;
        private readonly IProjectGrpcService _projectGrpcService;
        private readonly IMapper _mapper;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly IOtpRepository _otpRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserQueries _userQueries;

        public AuthenticationGrpcService(IAuthenticationService<User> authenticationService,
            IFCMTokenQueries fcmTokenQueries,
            IFCMTokenRepository fcmTokenRepository,
            IProjectGrpcService projectGrpcService,
            INotificationGrpcService notificationGrpcService,
            IOtpRepository otpRepository,
            IConfiguration configuration,
            IUserQueries userQueries,
                IMapper mapper)
        {
            _authenticationService = authenticationService;
            _fcmTokenQueries = fcmTokenQueries;
            _fcmTokenRepository = fcmTokenRepository;
            _projectGrpcService = projectGrpcService;
            _notificationGrpcService = notificationGrpcService;
            _otpRepository = otpRepository;
            _configuration = configuration;
            _userQueries = userQueries;
            _mapper = mapper;
        }

        public override async Task<ChangePasswordResultGrpcDTO> ChangePassword(ChangePasswordGrpcCommand changePasswordGrpcCommand, ServerCallContext context)
        {
            var actResponse = await _authenticationService.ChangePassword(changePasswordGrpcCommand.UserName,
                changePasswordGrpcCommand.OldPassword, changePasswordGrpcCommand.NewPassword);
            var rs = new ChangePasswordResultGrpcDTO()
            {
                Succeeded = actResponse.Succeeded,

            };
            rs.Errors.AddRange(_mapper.Map<IEnumerable<UserError>>(actResponse.Errors));
            return rs;
        }

        public override async Task<ChangePasswordResultGrpcDTO> ChangePasswordWithoutPass(ChangePasswordGrpcCommand request, ServerCallContext context)
        {
            var actResponse = await _authenticationService.ChangePasswordUser(request.UserName, request.NewPassword);

            var rs = new ChangePasswordResultGrpcDTO()
            {
                Succeeded = actResponse.Succeeded,

            };
            rs.Errors.AddRange(_mapper.Map<IEnumerable<UserError>>(actResponse.Errors));
            return rs;

        }

        public override async Task<ForgotPasswordResultGrpc> ForgotPassword(StringValue request, ServerCallContext context)
        {
            var account = await _authenticationService.FindByUserNameAsync(request.Value);

            if (account == null)
                return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Tài khoản không tồn tại" };

            if (string.IsNullOrEmpty(account.MobilePhoneNo))
                return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Tài khoản chưa đăng ký số điện thoại" };
            if (account.IsLock)
                return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Tài khoản đã bị khóa" };

            var otp = OtpHelper.GenOTP(6);
            var currentDate = DateTime.Now;
            var otpExpiredTime = _configuration.GetValue<int>("OtpExpiredSeconds");
            var obj = new OtpEntity()
            {
                CreatedDate = currentDate,
                DateExpired = currentDate.AddSeconds(otpExpiredTime),
                Otp = otp,
                Phone = account.MobilePhoneNo
            };
            var createOtpRs = await _otpRepository.CreateAndSave(obj);
            if (createOtpRs.IsSuccess)
            {
                //var sendOtpRs = await _notificationGrpcService.SendSms(account.MobilePhoneNo, otp);
                return new ForgotPasswordResultGrpc { Succeeded = true, Message = String.Format("Otp đã được gửi tới số điện thoại {0}", account.MobilePhoneNo.Substring(account.MobilePhoneNo.Length - 3).PadLeft(account.MobilePhoneNo.Length, '*')), DateExpired = _mapper.Map<Timestamp>(obj.DateExpired) };
            }

            return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Có lỗi" };

        }

        public async override Task<SignInResultGrpc> Login(LoginGrpcCommand request, ServerCallContext context)
        {
            var signinResult =
                await _authenticationService.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe.HasValue ? request.RememberMe.Value : false,
                        false);
            var rs = _mapper.Map<SignInResultGrpc>(signinResult);

            //if (signinResult.Succeeded)
            //{
            //    rs.Token = _authenticationService.GenerateJwtToken(request.UserName);
            //}

            return rs;
        }

        public override async Task<UserGrpcDTO> FindUserById(StringValue request, ServerCallContext context)
        {
            var actResponse = _authenticationService.FindById(request.Value);

            if (actResponse != null)
            {
                var responsibleProjects = await _projectGrpcService.GetProjectsBySupporter(actResponse.IdentityGuid.ToString());
                if (responsibleProjects != null && responsibleProjects.ProjectDtos.Count > 0)
                {
                    actResponse.ProjectIds
                        = responsibleProjects.ProjectDtos.Select(p => p.Id).ToArray();
                    var lstPr = new List<OrganizationUnit.Domain.Models.User.ProjectDTO>();
                    foreach (ProjectGrpcDTO proj in responsibleProjects.ProjectDtos)
                    {
                        lstPr.Add(_mapper.Map<OrganizationUnit.Domain.Models.User.ProjectDTO>(proj));
                    }
                    actResponse.Projects = lstPr;
                }
                var userPermissions = _userQueries.GetPermissionsOfUser(actResponse.Id);
                actResponse.Permissions = userPermissions.Select(c => c.PermissionCode).ToArray();
                actResponse.RoleCodes = userPermissions.Select(c => c.RoleCode).Distinct().ToArray();
            }

            return await Task.FromResult(_mapper.Map<UserGrpcDTO>(actResponse));
        }

        public override async Task<RegisterFCMTokenResponseGrpc> RegisterFCMToken(RegisterFCMTokenCommandGrpc request, ServerCallContext context)
        {
            var existRecord = _fcmTokenQueries.FindByReceiverIdAndFcmToken(request.ReceiverId, request.Token);
            if (existRecord == null)
            {
                var saveResponse = await _fcmTokenRepository.CreateAndSave(_mapper.Map<FCMToken>(request));
                return new RegisterFCMTokenResponseGrpc()
                {
                    Successed = saveResponse.IsSuccess
                };
            }

            return new RegisterFCMTokenResponseGrpc()
            {
                Successed = true
            };
        }

        public override async Task<UnRegisterFCMTokenResponseGrpc> UnRegisterFCMToken(StringValue request, ServerCallContext context)
        {
            var existRecord = await _fcmTokenRepository.GetByToken(request.Value);

            if (existRecord != null)
            {
                await _fcmTokenRepository.RemoveAndSave(existRecord);
            }
            return new UnRegisterFCMTokenResponseGrpc();
        }

        public override async Task<GenerateJwtTokenResponseGrpc> GenerateJwtToken(StringValue userName, ServerCallContext context)
        {
            var r = new GenerateJwtTokenResponseGrpc
            {
                Token = _authenticationService.GenerateJwtToken(userName.Value)
            };
            return await Task.FromResult(r);

        }

        public override async Task<LoginByADResponseGrpc> LoginByAD(LoginByADRequestGrpc request, ServerCallContext context)
        {
            var isSuccess = await _authenticationService.LoginByAD(request.UserName, request.Password);
            return new LoginByADResponseGrpc()
            {
                IsSuccess = isSuccess
            };
        }
    }
}
