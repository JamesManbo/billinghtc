using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Services.GRPC.Servers
{
    public class ApplicationUsersGrpcService: ApplicationUsersGrpc.ApplicationUsersGrpcBase
    {
        private readonly IUserQueries _userQueries;
        private readonly IFCMTokenQueries _fcmTokenQueries;
       
        private readonly IMapper _mapper;
        public ApplicationUsersGrpcService(IUserQueries userQueries, IFCMTokenQueries fcmTokenQueries, IMapper mapper)
        {
            _userQueries = userQueries;
            _fcmTokenQueries = fcmTokenQueries;
            _mapper = mapper;
        }

        public override Task<ResultGrpc> GetList(UsersInGroupRequestFilterModelGrpc filterModelGrpc, ServerCallContext context)
        {
            if (filterModelGrpc.Type == Protos.RequestType.Selection)
            {
                return Task.FromResult(new ResultGrpc()
                {
                    Result = JsonConvert.SerializeObject(_userQueries.GetSelectionList(_mapper.Map<UserRequestFilterModel>(filterModelGrpc)))
                });
            }

            if (filterModelGrpc.Type == Protos.RequestType.Autocomplete)
            {
                return Task.FromResult(new ResultGrpc()
                {
                    Result = JsonConvert.SerializeObject(_userQueries.Autocomplete(_mapper.Map<UserRequestFilterModel>(filterModelGrpc)))
                });
            }

            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetList(_mapper.Map<UserRequestFilterModel>(filterModelGrpc)))
            });
        }

        public override Task<ResultGrpc> GetApplicationUserByUid(StringValue uid, ServerCallContext context)
        {
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.Find(uid.Value))
            });
        }

        public override Task<StringValue> GenerateUserCode(GenerateUserCodeRequestGrpc request, ServerCallContext context)
        {
            var rs = _userQueries.GenerateUserCode(request.GroupCodes?.Split(","), request.CategoryCode, request.TypeCode);
            return Task.FromResult(new StringValue { Value = rs});
        }

        public override Task<ListTokenResponseGrpc> GetFCMTokensByUids(StringValue request, ServerCallContext context)
        {
            var tokens = _fcmTokenQueries.GetListTokenByUids(request.Value);
            var rs = new ListTokenResponseGrpc();
            tokens.ForEach(tk =>
            {
                rs.Tokens.Add(_mapper.Map<FcmTokenGrpc>(tk));
            });
            return Task.FromResult(rs);
        }

    }
}
