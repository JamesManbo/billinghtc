using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class EquipmentTypeMapperProfile : Profile
    {
        public EquipmentTypeMapperProfile()
        {

            CreateMap<EquipmentDTO, EquipmentTypeGrpcDTO>().ReverseMap();

            CreateMap(typeof(EquipmentTypePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
