using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Infrastructure.Queries;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class UnitOfMeasurementService : UnitOfMeasurementGrpc.UnitOfMeasurementGrpcBase
    {
        private readonly IUnitOfMeasurementQueries _unitOfMeasurementQueries;
        private readonly IMapper _mapper;
        public UnitOfMeasurementService(IUnitOfMeasurementQueries unitOfMeasurementQueries, IMapper mapper)
        {
            _unitOfMeasurementQueries = unitOfMeasurementQueries;
            _mapper = mapper;
        }

        public override Task<UnitOfMeasurementListDTOGrpc> GetUnitOfMeasurements(UnitOfMeasurementFilterGrpc request, ServerCallContext context)
        {
            var lstUnit = _unitOfMeasurementQueries.GetSelectionList(_mapper.Map<UnitOfMeasurementFilterModel>(request)).ToList();
            var result = new UnitOfMeasurementListDTOGrpc();
            for (int i = 0; i < lstUnit.Count; i++)
            {
                result.UnitOfMeasurementDTOGrpcs.Add(_mapper.Map<SelectionItemDTOGrpc>(lstUnit.ElementAt(i)));
            }
            return Task.FromResult(result);
        }
    }
}
