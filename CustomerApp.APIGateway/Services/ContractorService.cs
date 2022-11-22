using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.OutContract;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IContractorService
    {
        Task<ContractorDTO> GetContactorByIdentity(string identity);
    }
    public class ContractorService : GrpcCaller, IContractorService
    {
        private readonly ILogger<ContractService> _logger;
        private readonly IMapper _mapper;
        public ContractorService(IMapper mapper,
           ILogger<ContractService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ContractorDTO> GetContactorByIdentity(string identity)
        {
            return await Call<ContractorDTO>(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);

                var lstServiceGrpc = await client.FindByIdAsync(new StringValue() { Value = identity });

                return _mapper.Map<ContractorDTO>(lstServiceGrpc);
            });
        }
    }
}
