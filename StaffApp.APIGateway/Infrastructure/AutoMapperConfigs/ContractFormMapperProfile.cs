using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.ContractFormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ContractFormMapperProfile : Profile
    {
        public ContractFormMapperProfile()
        {

            CreateMap<ContractFormDTO, ContractFormGrpcDTO>().ReverseMap();

            CreateMap(typeof(ContractFormPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            //CreateMap<ServicePageListGrpcDTO, IPagedList<ServiceDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
