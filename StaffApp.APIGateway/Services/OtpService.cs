using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.OtpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;

namespace StaffApp.APIGateway.Services
{
    public interface IOtpService
    {
        Task<OtpDto> FindOtpByUserName(string userName);
        Task<UpdateOtpUsedResponse> UpdateOtpUsed(int id);
    }
    public class OtpService : GrpcCaller, IOtpService
    {
        private readonly ILogger<OtpService> _logger;
        private readonly IMapper _mapper;
        public OtpService(IMapper mapper,
           ILogger<OtpService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcSystemUserIdentity)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OtpDto> FindOtpByUserName(string userName)
        {
            return await Call<OtpDto>(async channel =>
            {
                var client = new OtpGrpc.OtpGrpcClient(channel);
                return await client.FindByUserNameAsync(new StringValue() { Value = userName });
            });
        }

        public async Task<UpdateOtpUsedResponse> UpdateOtpUsed(int id)
        {
            return await Call<UpdateOtpUsedResponse>(async channel =>
            {
                var client = new OtpGrpc.OtpGrpcClient(channel);
                return await client.UpdateOtpUsedAsync(new Int32Value() { Value = id });
            });
        }
    }
}
