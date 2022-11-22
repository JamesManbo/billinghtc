using AutoMapper;
using CustomerApp.APIGateway.Models.SupportLocationModel;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Location.API.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface ISupportLocationService
    {
        Task<IPagedList<SupportLocationDTO>> GetList(RequestFilterModel filterModel);
    }
    public class SupportLocationService : GrpcCaller, ISupportLocationService
    {
        private readonly IMapper _mapper;
        public SupportLocationService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcLocation)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<SupportLocationDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<SupportLocationDTO>>(async channel =>
            {
                var client = new SupportLocationServiceGrpc.SupportLocationServiceGrpcClient(channel);
                var request = _mapper.Map<Location.API.Protos.RequestFilterGrpc>(filterModel);

                var lstSupportGrpc = await client.GetSupportLocationAsync(request);

                return _mapper.Map<IPagedList<SupportLocationDTO>>(lstSupportGrpc);
            });
        }

    }
}
