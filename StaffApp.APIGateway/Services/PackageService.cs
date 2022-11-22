using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.ServicePackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IPackageService
    {
        Task<IPagedList<ServicePackageDTO>> GetList(PackageFilterModel filterModel);
    }

    public class PackageService : GrpcCaller, IPackageService
    {
        private readonly IMapper _mapper;
        public PackageService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<ServicePackageDTO>> GetList(PackageFilterModel filterModel)
        {
            return await Call<IPagedList<ServicePackageDTO>>(async channel =>
            {
                var client = new TelcoServicePackageGrpcService.TelcoServicePackageGrpcServiceClient(channel);
                

                var request = _mapper.Map<PackageRequestGrpc>(filterModel);

                var lstServiceGrpc = await client.GetPackagesAsync(request);
                
                return _mapper.Map<IPagedList<ServicePackageDTO>>(lstServiceGrpc);
            });
        }
    }
}
