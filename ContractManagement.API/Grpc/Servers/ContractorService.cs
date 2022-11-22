using AutoMapper;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.FilterModels;

namespace ContractManagement.API.Grpc.Servers
{
    public class ContractorService : ContractorGrpc.ContractorGrpcBase
    {
        private readonly IContractorQueries _contractorQueries;
        private readonly IMapper _mapper;
        public ContractorService(IContractorQueries servicesQueries, IMapper mapper)
        {
            _contractorQueries = servicesQueries;
            _mapper = mapper;
        }

        public override Task<ContractorGrpcDTO> FindById(StringValue request, ServerCallContext context)
        {
            var result = _contractorQueries.FindById(request.Value);
            return Task.FromResult(_mapper.Map<ContractorGrpcDTO>(result));
        }

        public override Task<ContractorPageListGrpcDTO> GetContractors(RequestGetContractorsByProjectIdsGrpc request, ServerCallContext context)
        {
            try
            {
                var contractors = _contractorQueries.GetListByProjectIds(_mapper.Map<ContractorByProjectIdsFilterModel>(request));

                return Task.FromResult(_mapper.Map<ContractorPageListGrpcDTO>(contractors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override Task<ContractorPageListGrpcDTO> GetListByProjectIdsForApp(RequestGetContractorsByProjectIdsGrpc request, ServerCallContext context)
        {
            var contractors = _contractorQueries.GetListByProjectIdsForApp(_mapper.Map<ContractorByProjectIdsFilterModel>(request));

            return Task.FromResult(_mapper.Map<ContractorPageListGrpcDTO>(contractors));
        }

        public override Task<ListContractorGrpcDTO> GetAllByIds(StringValue request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Value)) return default;

            var result = new ListContractorGrpcDTO();
            var ids = request.Value.Split(',').Select(int.Parse).ToArray();
            var contractors = _contractorQueries.GetAllByIds(ids);

            return Task.FromResult(_mapper.Map<ListContractorGrpcDTO>(contractors));
        }

        public override Task<ListContractorGrpcDTO> GetFromId(Int32Value request, ServerCallContext context)
        {
            var contractors = _contractorQueries.GetFromId(request.Value);
            return Task.FromResult(_mapper.Map<ListContractorGrpcDTO>(contractors));
        }

        public override Task<ContractorPageListGrpcDTO> GetByMarketAreaIdsProjectIds(RequestGetByMarketAreaIdsProjectIdsGrpc request, ServerCallContext context)
        {
            var contractors = _contractorQueries.GetListByMarketIdsProjectIds(_mapper.Map<ContractorByMarketAreaIdsProjectIdsFilterModel>(request));

            return Task.FromResult(_mapper.Map<ContractorPageListGrpcDTO>(contractors));
        }
    }
}
