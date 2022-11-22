using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;
using Google.Protobuf.WellKnownTypes;
using StaffApp.APIGateway.Models.AuthModels;
using StaffApp.APIGateway.Models.FCMModels;

namespace StaffApp.APIGateway.Services
{
    public interface IAuthenticationService
    {
        Task<ChangePasswordResult> ChangePassword(DraftChangePasswordRequest command);
        Task<ChangePasswordResult> ChangePasswordWithoutOldPassword(DraftChangePasswordRequest command);
        Task<ForgotPaswordResult> ForgotPassword(string userName);
        Task<SignInResultDTO> Login(LoginRequest loginRequest);
        Task<UserDTO> FindUserById(string userId);
        Task<bool> RegisterFCM(RegisterFcmTokenCommand command);
        Task<bool> UnRegisterFCM(string token);
        Task<bool> LoginByAD(string userName, string password);
        Task<string> GenerateJwtToken(string userName);
    }

    public class AuthenticationService : GrpcCaller, IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMapper _mapper;
        public AuthenticationService(IMapper mapper,
           ILogger<AuthenticationService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcSystemUserIdentity)
        {
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ChangePasswordResult> ChangePassword(DraftChangePasswordRequest command)
        {
            return await Call<ChangePasswordResult>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var clientRequest = _mapper.Map<ChangePasswordGrpcCommand>(command);
                return await client.ChangePasswordAsync(clientRequest);
            });

        }

        public async Task<ChangePasswordResult> ChangePasswordWithoutOldPassword(DraftChangePasswordRequest command)
        {
            return await Call<ChangePasswordResult>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var clientRequest = _mapper.Map<ChangePasswordGrpcCommand>(command);
                return await client.ChangePasswordWithoutPassAsync(clientRequest);
            });
        }

        public async Task<SignInResultDTO> Login(LoginRequest loginRequest)
        {
            return await Call<SignInResultDTO>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var clientRequest = _mapper.Map<LoginGrpcCommand>(loginRequest);
                return await client.LoginAsync(clientRequest);
            });
        }

        public async Task<UserDTO> FindUserById(string userId)
        {
            return await Call<UserDTO>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                return await client.FindUserByIdAsync(new StringValue() { Value = userId });
            });
        }

        public async Task<bool> RegisterFCM(RegisterFcmTokenCommand command)
        {
            return await Call<bool>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var response = await client.RegisterFCMTokenAsync(_mapper.Map<RegisterFCMTokenCommandGrpc>(command));
                return response?.Successed ?? false;
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

        public async Task<ForgotPaswordResult> ForgotPassword(string userName)
        {
            return await Call<ForgotPaswordResult>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var rs = await client.ForgotPasswordAsync(new StringValue() { Value = userName });
                return rs;
            });

        }

        public async Task<bool> LoginByAD(string userName, string password)
        {
            return await Call<bool>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var rs = await client.LoginByADAsync(new LoginByADRequestGrpc() { UserName = userName, Password = password });
                return rs.IsSuccess;
            });
        }

        public async Task<string> GenerateJwtToken(string userName)
        {
            return await Call<string>(async channel =>
            {
                var client = new AuthenticationGrpc.AuthenticationGrpcClient(channel);
                var rs = await client.GenerateJwtTokenAsync(new StringValue() {Value = userName });
                return rs.Token;
            });
        }
    }
}
