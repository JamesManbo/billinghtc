using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.PackageModels;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IPackageService
    {
        Task<IPagedList<PackageDTO>> GetList(PackageFilterModel filterModel);
    }
    public class PackageService : GrpcCaller, IPackageService
    {
        private readonly IMapper _mapper;
        public PackageService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<PackageDTO>> GetList(PackageFilterModel filterModel)
        {
            return await Call<IPagedList<PackageDTO>>(async channel =>
            {
                var client = new TelcoServicePackageGrpcService.TelcoServicePackageGrpcServiceClient(channel);
                
                var request = _mapper.Map<PackageRequestGrpc>(filterModel);

                var lstServiceGrpc = await client.GetPackagesAsync(request);

                return _mapper.Map<IPagedList<PackageDTO>>(lstServiceGrpc);
            });
        }
    }
}
