using ApplicationUserIdentity.API.Proto;
using AutoMapper;
using CustomerApp.APIGateway.Models.AuthModels;
using CustomerApp.APIGateway.Models.FCMModels;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IAuthenticationService
    {
        //Task<ChangePasswordDTO> ChangePassword(DraftChangePasswordRequest command);
        Task<LoginResultDTO> Login(LoginRequest loginRequest);
        Task<ChangePasswordResultDTO> ChangePassword(ChangePasswordRequest changePasswordRequest);
        Task<ChangePasswordResultDTO> ChangePasswordWithoutOldPassword(ChangePasswordByUserNameRequest command);
        Task<ForgotPaswordResult> ForgotPassword(string userName);
        Task<bool> RegisterFCM(RegisterFcmTokenCommand command);
        Task<bool> UnRegisterFCM(string token);
    }
    public class AuthenticationService : GrpcCaller, IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        public AuthenticationService(IMapper mapper,
           ILogger<AuthenticationService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ChangePasswordResultDTO> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            return await Call<ChangePasswordResultDTO>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var clientRequest = _mapper.Map<ChangePasswordRequestGrpc>(changePasswordRequest);
                return await client.ChangePasswordAsync(clientRequest);
            });
        }

        public async Task<ChangePasswordResultDTO> ChangePasswordWithoutOldPassword(ChangePasswordByUserNameRequest command)
        {
            return await Call<ChangePasswordResultDTO>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var clientRequest = _mapper.Map<ChangePasswordGrpcCommand>(command);
                return await client.ChangePasswordWithoutPassAsync(clientRequest);
            });
        }

        public async Task<ForgotPaswordResult> ForgotPassword(string userName)
        {
            return await Call<ForgotPaswordResult>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                return await client.ForgotPasswordAsync(new StringValue() { Value = userName });
            });
        }

        public async Task<LoginResultDTO> Login(LoginRequest loginRequest)
        {
            return await Call<LoginResultDTO>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var clientRequest = _mapper.Map<LoginRequestGrpc>(loginRequest);
                return await client.LoginAsync(clientRequest);
            });
        }

        public async Task<bool> RegisterFCM(RegisterFcmTokenCommand command)
        {
            return await Call<bool>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                await client.RegisterFCMTokenAsync(_mapper.Map<RegisterFCMTokenCommandGrpc>(command));
                return true;
            });
        }

        public async Task<bool> UnRegisterFCM(string token)
        {
            return await Call<bool>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                await client.UnRegisterFCMTokenAsync(new StringValue { Value = token });
                return true;
            });
        }
    }
}
