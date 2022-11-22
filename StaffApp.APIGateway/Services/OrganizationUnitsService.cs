using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IOrganizationUnitsService
    {
        Task<string> GetList(RequestFilterModel filterModel);
        Task<OrganizationUnitGrpcDTO> GetByCode(string code);

    }

    public class OrganizationUnitsService : GrpcCaller, IOrganizationUnitsService
    {
        private readonly IMapper _mapper;
        public OrganizationUnitsService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
            _mapper = mapper;
        }

        public async Task<OrganizationUnitGrpcDTO> GetByCode(string code)
        {
            return await Call<OrganizationUnitGrpcDTO>(async channel =>
            {
                var client = new OrganizationUnitsGrpc.OrganizationUnitsGrpcClient(channel);

                var resultGrpc = await client.GetByCodeAsync(new StringValue() {Value = code });

                return resultGrpc;
            });
        }

        public async Task<string> GetList(RequestFilterModel request)
        {
            return await Call<string>(async channel =>
            {
                var client = new OrganizationUnitsGrpc.OrganizationUnitsGrpcClient(channel);
                var filterModel = _mapper.Map<RequestFilterGrpc>(request);

                var resultGrpc = await client.GetListAsync(filterModel);

                return resultGrpc.Result;
            });
        }
    }
}
