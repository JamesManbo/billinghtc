using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class EquipmentTypeService : EquipmentTypeGrpc.EquipmentTypeGrpcBase
    {
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        private readonly IMapper _mapper;
        public EquipmentTypeService(IEquipmentTypeQueries equipmentTypeQueries, IMapper mapper)
        {
            _equipmentTypeQueries = equipmentTypeQueries;
            _mapper = mapper;
        }

        public override Task<EquipmentTypePageListGrpcDTO> GetEquipmentTypes(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstProject = _equipmentTypeQueries.GetList(_mapper.Map<RequestFilterModel>(request));
            return Task.FromResult(_mapper.Map<EquipmentTypePageListGrpcDTO>(lstProject));
        }
    }
}
