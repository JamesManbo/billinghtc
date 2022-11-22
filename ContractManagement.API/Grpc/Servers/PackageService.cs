using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class PackageService : TelcoServicePackageGrpcService.TelcoServicePackageGrpcServiceBase
    {
        private readonly IServicePackageQueries _packagesQueries;
        private readonly IMapper _mapper;
        public PackageService(IServicePackageQueries packagesQueries, IMapper mapper)
        {
            _packagesQueries = packagesQueries;
            _mapper = mapper;
        }

        public override Task<PackagePageListGrpcDTO> GetPackages(PackageRequestGrpc request, ServerCallContext context)
        {
            var lstPackage = _packagesQueries.GetPackagesByServiceId(_mapper.Map<PackageFilterModel>(request));
            return Task.FromResult(_mapper.Map<PackagePageListGrpcDTO>(lstPackage));
        }

        public override async Task<ListServicePackageGrpcResponse> GetAll(Empty request, ServerCallContext context)
        {
            var result = new ListServicePackageGrpcResponse();
            var allPackages = _packagesQueries.GetAllSimple();
            foreach (var item in allPackages)
            {
                result.PackageDtos.Add(_mapper.Map<PackageGrpcDTO>(item));
            }
            return result;
        }
    }
}
