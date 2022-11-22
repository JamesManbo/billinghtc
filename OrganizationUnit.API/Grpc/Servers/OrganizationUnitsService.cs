using AutoMapper;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Organizations;
using OrganizationUnit.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Grpc.Servers
{
    public class OrganizationUnitsService : OrganizationUnitsGrpc.OrganizationUnitsGrpcBase
    {
        private readonly IOrganizationUnitQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public OrganizationUnitsService(IOrganizationUnitQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public override Task<ResultGrpc> GetList(RequestFilterGrpc filterModel, ServerCallContext context)
        {
            if (filterModel.Type == Protos.Organizations.RequestType.Selection)
            {
                return Task.FromResult(new ResultGrpc()
                {
                    Result = JsonConvert.SerializeObject(_queryRepository.GetSelectionList())
                });
            }

            if (filterModel.Type == Protos.Organizations.RequestType.Hierarchical)
            {
                return Task.FromResult(new ResultGrpc()
                {
                    Result = JsonConvert.SerializeObject(_queryRepository.GetHierarchicalList())
                });
            }

            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_queryRepository.GetList(_mapper.Map<RequestFilterModel>(filterModel)))
            });
        }

        public override Task<ListOrganizationUnitGrpc> GetAll(Empty request, ServerCallContext context)
        {
            var result = new ListOrganizationUnitGrpc();
            var allOrganizationUnits = _queryRepository.GetAll();
            foreach (var item in allOrganizationUnits)
            {
                result.OrganizationUnits.Add(_mapper.Map<OrganizationUnitGrpcDTO>(item));
            }

            return Task.FromResult(result);
        }

        public override Task<OrganizationUnitGrpcDTO> GetByCode(StringValue request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Value)) return null;
            var ou = _queryRepository.GetByCode(request.Value);
            return Task.FromResult(_mapper.Map<OrganizationUnitGrpcDTO>(ou));
        }

        public override Task<ListOrganizationUnitGrpc> GetChildrenByCode(StringValue request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Value)) return null;
            var result = new ListOrganizationUnitGrpc();
            var ous = _queryRepository.GetAllChildByCode(request.Value);
            result.OrganizationUnits.AddRange(ous.Select(_mapper.Map<OrganizationUnitGrpcDTO>));
            return Task.FromResult(result);
        }

        public override Task<OrganizationUnitGrpcDTO> GetById(StringValue request, ServerCallContext context)
        {
            var ou = _queryRepository.GetByUniversalIdentity(request.Value);
            return Task.FromResult(_mapper.Map<OrganizationUnitGrpcDTO>(ou));
        }
    }
}
