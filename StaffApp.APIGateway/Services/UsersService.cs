using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Users;
using StaffApp.APIGateway.Models.AuthModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IUsersService
    {
        Task<string> GetList(UserFilterModel filterModel);
        Task<string> GetListTypeSelection(bool isPartner);
        Task<string> GetInfo(string uid);
        Task<SettingAccountResponseGrpc> ChangeSetting(SettingAccountCommandGrpc command);
    }

    public class UsersService : GrpcCaller, IUsersService
    {
        private readonly IMapper _mapper;
        public UsersService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
            _mapper = mapper;
        }

        public async Task<SettingAccountResponseGrpc> ChangeSetting(SettingAccountCommandGrpc command)
        {
            return await Call<SettingAccountResponseGrpc>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.ChangeSettingAccountAsync(command);

                return (resultGrpc);
            });
        }

        public async Task<string> GetInfo(string uid)
        {
            return await Call<string>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetByUidAsync(new Google.Protobuf.WellKnownTypes.StringValue() { Value = uid});

                return resultGrpc.Result;
            });
        }

        public async Task<string> GetList(UserFilterModel request)
        {
            return await Call<string>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);
                var filterModel = _mapper.Map<UserRequestFilterModelGrpc>(request);

                var resultGrpc = await client.GetListAsync(filterModel);

                return resultGrpc.Result;
            });
        }

        public async Task<string> GetListTypeSelection(bool isPartner)
        {
            return await Call<string>(async channel =>
            {
                var client = new UsersGrpc.UsersGrpcClient(channel);

                var resultGrpc = await client.GetListTypeSelectionAsync(new IsPartnerGrpc() { IsPartner = isPartner });

                return resultGrpc.Result;
            });
        }
    }
}
