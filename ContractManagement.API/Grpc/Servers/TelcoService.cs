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

namespace ContractManagement.API.Grpc.Servers
{
    public class TelcoService : ServiceGrpc.ServiceGrpcBase
    {
        private readonly IServicesQueries _servicesQueries;
        private readonly IMapper _mapper;
        public TelcoService(IServicesQueries servicesQueries, IMapper mapper)
        {
            _servicesQueries = servicesQueries;
            _mapper = mapper;
        }

        public override Task<ServicePageListGrpcDTO> GetServices(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstService = _servicesQueries.GetList(_mapper.Map<RequestFilterModel>(request));
            var rs = _mapper.Map<ServicePageListGrpcDTO>(lstService);
            return Task.FromResult(rs);
        }

        public override Task<ServiceListGrpcDTO> GetAllService(Empty request, ServerCallContext context)
        {
                var lstService = _servicesQueries.GetAll().ToList();
                var result = new ServiceListGrpcDTO();
                for (int i = 0; i < lstService.Count; i++)
                {
                    result.ServiceDtos.Add(_mapper.Map<ServiceGrpcDTO>(lstService.ElementAt(i)));
                }
                return Task.FromResult(result);
        }

        public override Task<ServiceGrpcDTO> GetDetail(Int32Value request, ServerCallContext context)
        {
            var service = _servicesQueries.Find(request.Value); ;
            var rs = _mapper.Map<ServiceGrpcDTO>(service);
            return Task.FromResult(rs);
        }
    }
}
