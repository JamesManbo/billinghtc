using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notification.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Grpc.Client
{
    public interface IApplicationUserGrpcService
    {
        Task<IPagedList<ApplicationUserModel>> GetListApplicationUser(RequestFilterModel filter);
        Task<List<UserTokenModel>> GetFCMTokensByUids(string uids);
    }

    public class ApplicationUserGrpcService : GrpcCaller, IApplicationUserGrpcService
    {
        private readonly IMapper _mapper;
        public ApplicationUserGrpcService(IMapper mapper, ILogger<GrpcCaller> logger)
   : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
            _mapper = mapper;
        }

        public async Task<List<UserTokenModel>> GetFCMTokensByUids(string uids)
        {
            return await Call<List<UserTokenModel>>(async channel =>
            {
                var client = new ApplicationUsersGrpc.ApplicationUsersGrpcClient(channel);

                var resultGrpc = await client.GetFCMTokensByUidsAsync(new StringValue { Value = uids });

                return resultGrpc.Tokens;
            });
        }

        public async Task<IPagedList<ApplicationUserModel>> GetListApplicationUser(RequestFilterModel filter)
        {
            return await Call<IPagedList<ApplicationUserModel>>(async channel =>
            {
                var client = new ApplicationUsersGrpc.ApplicationUsersGrpcClient(channel);

                var resultGrpc = await client.GetListAsync(_mapper.Map<UsersInGroupRequestFilterModelGrpc>(filter));
                var rs = JsonConvert.DeserializeObject<PagedList<ApplicationUserModel>>(resultGrpc.Result);

                return rs;
            });
        }
    }

}
