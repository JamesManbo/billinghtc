using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IEquipmentTypeService
    {
        Task<IPagedList<EquipmentDTO>> GetList(RequestFilterModel filterModel);
    }

    public class EquipmentTypeService: GrpcCaller, IEquipmentTypeService
    {
        private readonly IMapper _mapper;
        public EquipmentTypeService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<EquipmentDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<EquipmentDTO>>(async channel =>
            {
                var client = new EquipmentTypeGrpc.EquipmentTypeGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstServiceGrpc = await client.GetEquipmentTypesAsync(request);           

                return _mapper.Map<IPagedList<EquipmentDTO>>(lstServiceGrpc);
            });
        }
    }
}
