using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models.ContractModels;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IContractGrpcService
    {
        Task<IEnumerable<ChannelAddressModel>> GetChannelAddresses(string[] channelCids);
        Task<RepeatedField<OutContractSimpleGrpcDTO>> GetByIds(int[] ids);
        Task<RepeatedField<OutContractServicePackageGrpcDTO>> GetOutContractServicePackageByIds(int[] ids);
    }

    public class ContractGrpcService : GrpcCaller, IContractGrpcService
    {
        private readonly IMapper _mapper;

        public ContractGrpcService(
            ILogger<GrpcCaller> logger,
            IMapper mapper)
            : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<RepeatedField<OutContractSimpleGrpcDTO>> GetByIds(int[] ids)
        {
            return await CallAsync(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var rpcResponse = await client.GetByIdsAsync(new StringValue { Value = string.Join(',', ids) });

                return rpcResponse.OutContracts;
            });
        }

        public async Task<IEnumerable<ChannelAddressModel>> GetChannelAddresses(string[] channelCids)
        {
            return await CallAsync(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var rpcResponse = await client.GetChannelAddressesByCidAsync(new StringValue { Value = string.Join(',', channelCids) });

                return rpcResponse.Result?.Select(_mapper.Map<ChannelAddressModel>);
            });
        }

        public async Task<RepeatedField<OutContractServicePackageGrpcDTO>> GetOutContractServicePackageByIds(int[] ids)
        {
            return await CallAsync(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var rpcResponse = await client.GetOutContractServicePackageByIdsAsync(new StringValue { Value = string.Join(',', ids) });

                return rpcResponse.OutContractServicePackages;
            });
        }

    }
}