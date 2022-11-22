using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Configurations;
using OrganizationUnit.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Grpc.Servers
{
    public class ConfigurationSystemParameterService : ConfigurationSystemParameterGrpc.ConfigurationSystemParameterGrpcBase
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSystemUserQueryRepository _configurationSystemUserQueryRepository;

        public ConfigurationSystemParameterService(IConfigurationSystemUserQueryRepository configurationSystemUserQueryRepository,
            IMapper mapper)
        {
            _configurationSystemUserQueryRepository = configurationSystemUserQueryRepository;
            this._mapper = mapper;
        }

        public override Task<Int32Value> GetNumberDaysBadDebt(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new Int32Value() { Value = _configurationSystemUserQueryRepository.Find().NumberDaysBadDebt });
        }

        public override Task<SystemConfigurationParamsGrpcDTO> GetSystemConfiguration(Empty request, ServerCallContext context)
        {
            var systemConfiguration = _configurationSystemUserQueryRepository.Find();
            var transObject = _mapper.Map<SystemConfigurationParamsGrpcDTO>(systemConfiguration);
            return Task.FromResult(transObject);
        }
    }
}
