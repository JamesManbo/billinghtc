using AutoMapper;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Notification.API.Models;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Grpc.Client
{
    public interface IUserGrpcService
    {
        Task<List<UserTokenModel>> GetListTokenByRoleUser(string roleCode);
        Task<List<UserTokenModel>> GetListTokenByDepartment(string departmentCode);
        Task<List<UserModel>> GetAllUserByRoleCode(string roleCode);
        Task<List<UserModel>> GetAllUserByDepartmentCode(string departmentCode);
        Task<List<UserTokenModel>> GetFCMTokensByUids(string uids);
    }
    public class UserGrpcService : GrpcCaller, IUserGrpcService
    {
        public UserGrpcService(IMapper mapper, ILogger<GrpcCaller> logger)
   : base(mapper, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
        }

        public async Task<List<UserTokenModel>> GetListTokenByRoleUser(string roleCode)
        {
            return await Call<List<UserTokenModel>>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetFCMTokensByRoleUserAsync(new StringValue { Value = roleCode });

                return resultGrpc.Tokens;
            });
        }

        public async Task<List<UserTokenModel>> GetFCMTokensByUids(string uids)
        {
            return await Call<List<UserTokenModel>>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetFCMTokensByUidsAsync(new StringValue { Value = uids });

                return resultGrpc.Tokens;
            });
        }

        public async Task<List<UserTokenModel>> GetListTokenByDepartment(string departmentCode)
        {
            return await Call<List<UserTokenModel>>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetFCMTokensByDepartentAsync(new StringValue { Value = departmentCode });

                return resultGrpc.Tokens;
            });
        }

        public async Task<List<UserModel>> GetAllUserByRoleCode(string roleCode)
        {
            return await Call<List<UserModel>>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetListByRoleCodeAsync(new StringValue { Value = roleCode });
                var result = JsonConvert.DeserializeObject<List<UserModel>>(resultGrpc.Result);
                return result;
            });
        }

        public async Task<List<UserModel>> GetAllUserByDepartmentCode(string departmentCode)
        {
            return await Call<List<UserModel>>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetListByDepartmentCodeAsync(new StringValue { Value = departmentCode });
                var result = JsonConvert.DeserializeObject<List<UserModel>>(resultGrpc.Result);
                return result;
            });
        }
    }
}
