using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IUnitOfMeasurementService
    {
        Task<IEnumerable<SelectionItemDTO>> GetSelectList(UnitOfMeasurementFilterModel filterModel);
    }
    public class UnitOfMeasurementService : GrpcCaller, IUnitOfMeasurementService
    {
        private readonly IMapper _mapper;
        public UnitOfMeasurementService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<SelectionItemDTO>> GetSelectList(UnitOfMeasurementFilterModel filterModel)
        {
            return await Call<IEnumerable<SelectionItemDTO>>(async channel =>
            {
                var client = new UnitOfMeasurementGrpc.UnitOfMeasurementGrpcClient(channel);
                var request = _mapper.Map<UnitOfMeasurementFilterGrpc>(filterModel);

                var lstServiceGrpc = await client.GetUnitOfMeasurementsAsync(request);
                return lstServiceGrpc.UnitOfMeasurementDTOGrpcs.ToList();
            });
        }
    }
}
