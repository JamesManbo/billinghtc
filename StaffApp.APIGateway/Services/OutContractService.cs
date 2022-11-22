using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.CustomerModels;
using StaffApp.APIGateway.Models.EquipmentModels;
using StaffApp.APIGateway.Models.ReportModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IOutContractService
    {
        Task<IPagedList<ContractSimpleDTO>> GetListContact(ContractFilterModel filterModel);
        Task<ContractDTO> GetDetail(int id);
        Task<ContractDTO> GetDetailByCode(string code);
        Task<OutContractIsSuccessDTO> CreatedOutContract(StringValue request);
        Task<string> GetOutContractStatuses();
        Task<string> GenerateContractCode(bool isEnterprise, string customerFullName, int? marketAreaId = null, int? projectId = null, string srvPackageIds = "");
        Task<IEnumerable<OutContractEquipmentDTO>> AutocompleteInstanceAsync(RequestFilterModel filterModel);
        Task<IEnumerable<OutputChannelPointDTO>> AutocompleteOutputChannelPointAsync(OutputChannelFilterModel filterModel);
        Task<IEnumerable<ContractStatusReportModelGrpc>> GetReportContractStatus(ContractStatusReportFilter filterModel);
    }

    public class OutContractService : GrpcCaller, IOutContractService
    {
        private readonly IMapper _mapper;
        public OutContractService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<ContractDTO> GetDetail(int id)
        {
            return await Call<ContractDTO>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                var contractGrpc = await client.GetDetailAsync(new Int32Value() { Value = id});

                return _mapper.Map<ContractDTO>(contractGrpc);
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

        public async Task<OutContractIsSuccessDTO> CreatedOutContract(StringValue request)
        {
            return await Call<OutContractIsSuccessDTO>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                var isSuccess = await client.CreatedOutContractAsync(request);

                return _mapper.Map<OutContractIsSuccessDTO>(isSuccess);
            });
        }

        public async Task<ContractDTO> GetDetailByCode(string code)
        {
            return await Call<ContractDTO>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                var contractGrpc = await client.FindByContractCodeAsync(new StringValue() { Value = code });

                return _mapper.Map<ContractDTO>(contractGrpc);
            });
        }

        public async Task<string> GetOutContractStatuses()
        {
            return await Call<string>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                var contractGrpc = await client.GetOutContractStatusesAsync(new Empty());

                return contractGrpc.Value;
            });
        }

        public async Task<string> GenerateContractCode(bool isEnterprise, string customerFullName, int? marketAreaId = null, int? projectId = null, string srvPackageIds = "")
        {
            return await Call<string>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);

                var req = new GenerateContractCodeRequestGrpc{
                    IsEnterprise = isEnterprise,
                    CustomerFullName = customerFullName,
                    MarketAreaId = marketAreaId,
                    ProjectId = projectId,
                    SrvPackageIds = srvPackageIds
                };
                var contractGrpc = await client.GenerateContractCodeAsync(req);

                return contractGrpc.Value;
            });
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

        public async Task<IEnumerable<OutputChannelPointDTO>> AutocompleteOutputChannelPointAsync(OutputChannelFilterModel filterModel)
        {
            return await Call<IEnumerable<OutputChannelPointDTO>>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var request = _mapper.Map<OutputChannelPointRequestGrpc>(filterModel);

                var lstChannelPointGrpc = await client.AutoCompleteOutputChannelPointAsync(request);

                return lstChannelPointGrpc.Data;
            });
        }

        public async Task<IEnumerable<ContractStatusReportModelGrpc>> GetReportContractStatus(ContractStatusReportFilter filterModel)
        {
            return await Call<IEnumerable<ContractStatusReportModelGrpc>>(async channel =>
            {
                var client = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var request = _mapper.Map<ContractStatusReportFilterGrpc>(filterModel);

                var lstChannelPointGrpc = await client.GetReportContractStatusAsync(request);

                return lstChannelPointGrpc.Data;
            });
        }
    }
}
