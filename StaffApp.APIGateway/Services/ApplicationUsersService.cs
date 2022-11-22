using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.RequestModels;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Protos;

namespace StaffApp.APIGateway.Services
{
    public interface IApplicationUsersService
    {
        Task<string> GetList(UsersInGroupRequestFilterModel filterModel);
        Task<string> GetApplicationUserByUid(StringValue uid);
        Task<string> GenerateUserCode(string groupCodes, string categoryCode, string typeCode);

    }

    public class ApplicationUsersService : GrpcCaller, IApplicationUsersService
    {
        private readonly IMapper _mapper;
        public ApplicationUsersService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
            _mapper = mapper;
        }

        public async Task<string> GetList(UsersInGroupRequestFilterModel request)
        {
            return await Call<string>(async channel =>
            {
                var client = new ApplicationUsersGrpc.ApplicationUsersGrpcClient(channel);
                var filterModel = _mapper.Map<UsersInGroupRequestFilterModelGrpc>(request);

                var resultGrpc = await client.GetListAsync(filterModel);

                return resultGrpc.Result;
            });
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

        public async Task<string> GenerateUserCode(string groupCodes, string categoryCode, string typeCode)
        {
            return await Call<string>(async channel =>
            {
                var client = new ApplicationUsersGrpc.ApplicationUsersGrpcClient(channel);

                var req = new GenerateUserCodeRequestGrpc (){
                GroupCodes = groupCodes,
                CategoryCode = categoryCode,
                    TypeCode = typeCode
                };
                var resultGrpc = await client.GenerateUserCodeAsync(req);

                return resultGrpc.Value;
            });
        }
    }
}
