
using ApplicationUserIdentity.API.Proto;
using AutoMapper;
using CustomerApp.APIGateway.Models.UserModels;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IUserService
    {
        Task<UserDTO> FindUserById(int id);
        Task<UserDTO> FindUserByUId(string uid);
        Task<ChangeInfoResponse> ChangeInfoAccount(AccountCommand accountCommand);
    }

    public class UserService : GrpcCaller, IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(IMapper mapper,
           ILogger<UserService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDTO> FindUserById(int id)
        {
            return await Call<UserDTO>(async channel =>
            {
                var client = new UserGrpc.UserGrpcClient(channel);

                var userGrpc = await client.FindUserByIdAsync(new Int32Value() { Value = id });

                return _mapper.Map<UserDTO>(userGrpc);
            });
        }

        public async Task<UserDTO> FindUserByUId(string uid)
        {
            return await Call<UserDTO>(async channel =>
            {
                var client = new UserGrpc.UserGrpcClient(channel);

                var userGrpc = await client.FindUserByUIdAsync(new StringValue() { Value = uid });

                return _mapper.Map<UserDTO>(userGrpc);
            });
        }

        public async Task<ChangeInfoResponse> ChangeInfoAccount(AccountCommand accountCommand)
        {
            return await Call<ChangeInfoResponse>(async channel =>
            {
                var client = new UserGrpc.UserGrpcClient(channel);

                var userGrpc = await client.ChangeInfoApplicationUserAsync(_mapper.Map<ApplicationUserCommandGrpc>(accountCommand));

                return _mapper.Map<ChangeInfoResponse>(userGrpc);
            });
        }
    }
}
