using AutoMapper;
using CustomerApp.APIGateway.Models;
using Global.Configs.MicroserviceRouterConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ContractManagement.API.Protos;
using Google.Protobuf.WellKnownTypes;
using Global.Models.Filter;
using Global.Models.PagedList;

namespace CustomerApp.APIGateway.Services
{
    public interface ITelService
    {
        Task<IPagedList<ServiceDTO>> GetList(RequestFilterModel filterModel);
    }
    public class TelService : GrpcCaller, ITelService
    {
        private readonly ILogger<TelService> _logger;
        private readonly IMapper _mapper;
        public TelService(IMapper mapper,
           ILogger<TelService> logger)
           : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IPagedList<ServiceDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<ServiceDTO>>(async channel =>
            {
                var client = new ServiceGrpc.ServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstServiceGrpc = await client.GetServicesAsync(request);    

                return _mapper.Map<IPagedList<ServiceDTO>>(lstServiceGrpc);
            });
        }
    }
}
