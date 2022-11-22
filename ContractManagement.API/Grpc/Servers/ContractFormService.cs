using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class ContractFormService : ContractFormServiceGrpc.ContractFormServiceGrpcBase
    {
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IContractFormQueries _contractFormQueries;
        private readonly IMapper _mapper;

        public ContractFormService(IWrappedConfigAndMapper configAndMapper, IContractFormQueries contractFormQueries, IMapper mapper)
        {
            _configAndMapper = configAndMapper;
            _contractFormQueries = contractFormQueries;
            _mapper = mapper;
        }

        public override Task<ContractFormPageListGrpcDTO> GetLists(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstContractFrom = _contractFormQueries.GetList(_mapper.Map<RequestFilterModel>(request));
            var rs = _mapper.Map<ContractFormPageListGrpcDTO>(lstContractFrom);
            return Task.FromResult(rs);
        }

        public override Task<ContractFormGrpcDTO> GetById(Int32Value request, ServerCallContext context)
        {
            var contractFrom = _contractFormQueries.Find(request != null ? request.Value : 0);
            var rs = _mapper.Map<ContractFormGrpcDTO>(contractFrom);
            return Task.FromResult(rs);
        }

        public override Task<LstSelectionItemDTOGrpc> Autocomplete(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstContractFrom = _contractFormQueries.Autocomplete(_mapper.Map<RequestFilterModel>(request));
            var rs = _mapper.Map<LstSelectionItemDTOGrpc>(lstContractFrom);
            return Task.FromResult(rs);
        }
    }
}
