using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.ContractFormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IContractFormService
    {
        Task<IPagedList<ContractFormDTO>> GetList(RequestFilterModel filterModel);
        Task<ContractFormDTO> GetById(int id);
        Task<IEnumerable<SelectionItem>> Autocomplete(RequestFilterModel filterModel);
    }
    public class ContractFormService : GrpcCaller, IContractFormService
    {
        private readonly IMapper _mapper;
        public ContractFormService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<ContractFormDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<ContractFormDTO>>(async channel =>
            {
                var client = new ContractFormServiceGrpc.ContractFormServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstContractFormGrpc = await client.GetListsAsync(request);

                return _mapper.Map<IPagedList<ContractFormDTO>>(lstContractFormGrpc);
            });
        }

        public async Task<ContractFormDTO> GetById(int id)
        {
            return await Call<ContractFormDTO>(async channel =>
            {
                var client = new ContractFormServiceGrpc.ContractFormServiceGrpcClient(channel);
                var request = new Int32Value() { Value = id };

                var contractFormGrpc = await client.GetByIdAsync(request);

                return _mapper.Map<ContractFormDTO>(contractFormGrpc);
            });
        }

        public async Task<IEnumerable<SelectionItem>> Autocomplete(RequestFilterModel filterModel)
        {
            return await Call<IEnumerable<SelectionItem>>(async channel =>
            {
                var client = new ContractFormServiceGrpc.ContractFormServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstContractFormGrpc = await client.AutocompleteAsync(request);

                return _mapper.Map<IEnumerable<SelectionItem>>(lstContractFormGrpc);
            });
        }
    }
}
