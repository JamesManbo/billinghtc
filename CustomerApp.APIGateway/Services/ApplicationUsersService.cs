using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IApplicationUsersService
    {
        Task<string> GetApplicationUserByUid(StringValue uid);

    }

    public class ApplicationUsersService : GrpcCaller, IApplicationUsersService
    {
        private readonly IMapper _mapper;
        public ApplicationUsersService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
            _mapper = mapper;
        }

        public async Task<string> GetApplicationUserByUid(StringValue uid)
        {
            return await Call<string>(async channel =>
            {
                var client = new ApplicationUsersGrpc.ApplicationUsersGrpcClient(channel);

                var resultGrpc = await client.GetApplicationUserByUidAsync(uid);

                return resultGrpc.Result;
            });
        }
    }
}
