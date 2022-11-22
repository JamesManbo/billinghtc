using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.CustomerModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IContractorService
    {
        Task<IPagedList<ContractorDTO>> GetListContactor(ContractorFilterModel filterModel);
        Task<IPagedList<ContractorDTO>> GetListOnlyContactor(RequestFilterModel filterModel);
    }
    public class ContractorService : GrpcCaller, IContractorService
    {
        private readonly IMapper _mapper;
        public ContractorService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<ContractorDTO>> GetListContactor(ContractorFilterModel filterModel)
        {
            return await Call<IPagedList<ContractorDTO>>(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);
                var request = _mapper.Map<RequestGetContractorsByProjectIdsGrpc>(filterModel);

                var lstContractorGrpc = await client.GetListByProjectIdsForAppAsync(request);

                return _mapper.Map<IPagedList<ContractorDTO>>(lstContractorGrpc);
            });
        }

        public async Task<IPagedList<ContractorDTO>> GetListOnlyContactor(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<ContractorDTO>>(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstContractorGrpc = await client.GetOnlyContractorsAsync(request);

                return _mapper.Map<IPagedList<ContractorDTO>>(lstContractorGrpc);
            });
        }
    }
}
