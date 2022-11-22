using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.MarketAreaModels;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;

namespace StaffApp.APIGateway.Services
{
    public interface IMarketAreaService
    {
        Task<IPagedList<MarketAreaModelDTO>> GetList(RequestFilterModel filterModel);
        Task<IEnumerable<MarketAreaModelDTO>> GetAll();
    }
    public class MarketAreaService : GrpcCaller, IMarketAreaService
    {
        private readonly IMapper _mapper;
        public MarketAreaService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<MarketAreaModelDTO>> GetAll()
        {
            return await Call<IEnumerable<MarketAreaModelDTO>>(async channel =>
            {
                var client = new MarketAreaServiceGrpc.MarketAreaServiceGrpcClient(channel);
                var response = await client.GetAllAsync(new Empty());
                return _mapper.Map<IEnumerable<MarketAreaModelDTO>>(response.MarketAreaDtos);
            });
        }

        public async Task<IPagedList<MarketAreaModelDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<MarketAreaModelDTO>>(async channel =>
            {
                var client = new MarketAreaServiceGrpc.MarketAreaServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstSupportGrpc = await client.GetMarketAreaAsync(request);

                return _mapper.Map<IPagedList<MarketAreaModelDTO>>(lstSupportGrpc);
            });
        }

    }
}
