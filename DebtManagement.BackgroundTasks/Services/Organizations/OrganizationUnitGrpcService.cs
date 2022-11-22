using AutoMapper;
using DebtManagement.BackgroundTasks.Services.Grpc;
using DebtManagement.Domain.Models.OrganizationModels;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrganizationUnit.API.Protos;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using OrganizationUnit.API.Protos.Organizations;

namespace DebtManagement.BackgroundTasks.Services.Organizations
{
    public interface IOrganizationUnitGrpcService
    {
        Task<IEnumerable<OrganizationUnitDTO>> GetAll();
    }
    public class OrganizationUnitGrpcService : GrpcCaller, IOrganizationUnitGrpcService
    {
        private readonly IMapper _mapper;
        public OrganizationUnitGrpcService(IMapper mapper, ILogger<OrganizationUnitGrpcService> logger)
            : base(mapper, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
            this._mapper = mapper;
        }

        public async Task<IEnumerable<OrganizationUnitDTO>> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var grpcClient = new OrganizationUnitsGrpc.OrganizationUnitsGrpcClient(channel);
                var grpcResponse = await grpcClient.GetAllAsync(new Empty());
                if (grpcResponse != null && grpcResponse.OrganizationUnits.Count > 0)
                {
                    return grpcResponse
                        .OrganizationUnits
                        .Select(this._mapper.Map<OrganizationUnitDTO>);
                }

                return default;
            });
        }
    }
}
