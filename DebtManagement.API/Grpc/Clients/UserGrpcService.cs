using AutoMapper;
using DebtManagement.Domain.Models.UserGrpc;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IUserGrpcService
    {
        Task<string> GetByUid(string id);
    }

    public class UserGrpcService : GrpcCaller, IUserGrpcService
    {
        public UserGrpcService(ILogger<GrpcCaller> logger)
            : base(logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
        }

        public async Task<string> GetByUid(string id)
        {
            return await CallAsync(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);
                var request = new StringValue()
                {
                    Value = id
                };
                var response = await client.GetByUidAsync(request);

                if (response == null || response.Result == "null")
                {
                    return "";
                }

                var userGrpcDto = JObject.Parse(response.Result).ToObject<UserGrpcDto>();

                return userGrpcDto != null ? userGrpcDto.FirstName + " " + userGrpcDto.LastName : "";
            });
        }
    }
}
