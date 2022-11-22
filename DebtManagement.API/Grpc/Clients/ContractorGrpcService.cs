using System.Collections.Generic;
using System.Threading.Tasks;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models.ContractModels;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IContractorGrpcService
    {
        Task<ContractorGrpcDTO> FindById(string id);
    }

    public class ContractorGrpcService : GrpcCaller, IContractorGrpcService
    {
        public ContractorGrpcService(ILogger<GrpcCaller> logger)
            : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<ContractorGrpcDTO> FindById(string id)
        {
            return await CallAsync(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);
                var result = await client.FindByIdAsync(new StringValue {Value = id});
                return result;
            });
        }
    }
}