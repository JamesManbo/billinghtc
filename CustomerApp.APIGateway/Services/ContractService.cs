using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.OutContract;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IContractService
    {
        Task<ContractDTO> FindByContractorId(string id);
        Task<ContractDTO> GetDetail(int id);
        Task<IPagedList<ContractSimpleDTO>> GetListContact(ContractFilterModel filterModel);
        Task<IEnumerable<OutContractEquipmentDTO>> AutocompleteInstanceAsync(RequestFilterModel filterModel);
    }
    public class ContractService : GrpcCaller, IContractService
    {
        private readonly ILogger<ContractService> _logger;
        private readonly IMapper _mapper;
        public ContractService(IMapper mapper,
           ILogger<ContractService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OutContractEquipmentDTO>> AutocompleteInstanceAsync(RequestFilterModel filterModel)
        {
            return await Call<IEnumerable<OutContractEquipmentDTO>>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstEquipmentGrpc = await client.GetOutContractEquipmentsAsync(request);

                return lstEquipmentGrpc.Data;
            });
        }

        public async Task<ContractDTO> FindByContractorId(string id)
        {
            return await Call<ContractDTO>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                return await client.FindByContractorIdAsync(new StringValue() { Value = id });
            });
        }

        public async Task<ContractDTO> GetDetail(int id)
        {
            return await Call<ContractDTO>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                return await client.GetDetailAsync(new Int32Value() { Value = id });
            });
        }

        public async Task<IPagedList<ContractSimpleDTO>> GetListContact(ContractFilterModel filterModel)
        {
            return await Call<IPagedList<ContractSimpleDTO>>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var request = _mapper.Map<RequestGetContractsGrpc>(filterModel);

                var lstContractGrpc = await client.GetContractsAsync(request);

                return _mapper.Map<IPagedList<ContractSimpleDTO>>(lstContractGrpc);
            });
        }
    }
}
