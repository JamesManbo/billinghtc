using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;

namespace CMS.APIGateway.Services
{
    public class OutContractService : GrpcCaller, IOutContractService
    {
        private readonly ILogger<OutContractService> _logger;
        private readonly IMapper _mapper;

        public OutContractService(IMapper mapper, 
            ILogger<OutContractService> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _logger = logger;
            _mapper = mapper;
        }
    }
}