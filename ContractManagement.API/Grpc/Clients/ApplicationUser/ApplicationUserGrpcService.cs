using ApplicationUserIdentity.API.Protos;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models.ApplicationUsers;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Clients.ApplicationUser
{
    public interface IApplicationUserGrpcService
    {
        Task<ApplicationUserDTO> GetApplicationUserByUid(string id);
    }
    public class ApplicationUserGrpcService : GrpcCaller, IApplicationUserGrpcService
    {
        public ApplicationUserGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger)
           : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
        }

        public async Task<ApplicationUserDTO> GetApplicationUserByUid(string id)
        {
            return await CallAsync<ApplicationUserDTO>(async channel =>
            {
                var client = new ApplicationUsersGrpc.ApplicationUsersGrpcClient(channel);
                var resultGrpc = await client.GetApplicationUserByUidAsync(new StringValue { Value = id});
                return JsonConvert.DeserializeObject<ApplicationUserDTO>(resultGrpc.Result);
            });
        }
    }
}
