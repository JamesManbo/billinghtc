using AutoMapper;
using ContractManagement.Domain.Models.Organizations;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Clients.Organizations
{
    public interface IUserGrpcService
    {
        Task<List<SelectionItem>> GetUserByIds(string id);
        Task<UserDTO> GetUserByUid(string id);
        Task<IEnumerable<UserDTO>> GetManagerByOrganization(string organizationUnitCode);
        Task<string> GetEmailsOfServiceProvider();
    }
    public class UserGrpcService : GrpcCaller, IUserGrpcService
    {
        private readonly IMapper _mapper;
        public UserGrpcService(IWrappedConfigAndMapper wrappedConfig,
            ILogger<GrpcCaller> logger,
            IMapper mapper)
           : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
            this._mapper = mapper;
        }

        public async Task<List<SelectionItem>> GetUserByIds(string id)
        {
            return await CallAsync<List<SelectionItem>>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetUserByIdsAsync(new StringValue { Value = id });

                var rs = JArray.Parse(resultGrpc.Result);
                return rs.ToObject<List<SelectionItem>>();
            });
        }

        public async Task<UserDTO> GetUserByUid(string id)
        {
            return await CallAsync<UserDTO>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);
                var resultGrpc = await client.GetByUidAsync(new StringValue { Value = id });
                return JsonConvert.DeserializeObject<UserDTO>(resultGrpc.Result);
            });
        }

        public async Task<string> GetEmailsOfServiceProvider()
        {
            return await CallAsync<string>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);
                var resultGrpc = await client.GetEmailsOfServiceProviderAsync(new StringValue { Value = string.Empty });
                return JsonConvert.DeserializeObject<string>(resultGrpc.Result);
            });
        }

        public async Task<IEnumerable<UserDTO>> GetManagerByOrganization(string organizationUnitCode)
        {
            return await CallAsync(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);
                var resultGrpc = await client.GetManagementUserAsync(new StringValue { Value = organizationUnitCode });

                if (resultGrpc == null || (resultGrpc.Users?.Count ?? 0) == 0) return Enumerable.Empty<UserDTO>();

                return resultGrpc.Users.Select(_mapper.Map<UserDTO>);
            });
        }
    }
}
