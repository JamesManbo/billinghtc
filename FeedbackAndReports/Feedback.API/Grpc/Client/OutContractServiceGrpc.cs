using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedback.API.Grpc.Client
{
    public interface IOutContractServiceGrpc
    {
        Task<ContractGrpcDTO> GetOutContractByChannelId(int channelId);
        Task<ContractGrpcDTO> GetOutContractByChannelCId(string cId);
    }
    public class OutContractServiceGrpc : GrpcCaller, IOutContractServiceGrpc
    {
        public OutContractServiceGrpc(ILogger<GrpcCaller> logger) : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<ContractGrpcDTO> GetOutContractByChannelCId(string cId)
        {
            return await CallAsync(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var request = new StringValue() { Value = cId };
                return await client.GetContractByChannelCIdAsync(request);
            });
        }

        public async Task<ContractGrpcDTO> GetOutContractByChannelId(int channelId)
        {
            return await CallAsync(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var request = new Int32Value() { Value = channelId };
                return await client.GetContractByChannelIdAsync(request);
            });
        }
    }
}
