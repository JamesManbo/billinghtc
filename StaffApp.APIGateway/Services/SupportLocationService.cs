using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.SupportLocationModel;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.SupportLocationModel;
using Location.API.Protos;


namespace StaffApp.APIGateway.Services
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
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstSupportGrpc = await client.GetSupportLocationAsync(request);

                return _mapper.Map<IPagedList<SupportLocationDTO>>(lstSupportGrpc);
            });
        }

    }
}
