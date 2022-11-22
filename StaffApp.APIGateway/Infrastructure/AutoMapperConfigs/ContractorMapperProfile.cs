using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.CustomerModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ContractorMapperProfile : Profile
    {
        public ContractorMapperProfile()
        {

            //CreateMap<CustomerDTO, ContractorGrpcDTO>()
            //    .ForMember(d => d.ContractorFullName, m => m.MapFrom(e => e.FullName))
            //    .ForMember(d => d.ContractorCode, m => m.MapFrom(e => e.CustomerCode))
            //    .ForMember(d => d.ContractorAddress, m => m.MapFrom(e => e.Address))
            //    .ForMember(d => d.ContractorPhone, m => m.MapFrom(e => e.MobilePhoneNo))
            //    .ReverseMap();
            CreateMap<ContractorFilterModel, RequestGetContractorsByProjectIdsGrpc>().ReverseMap();
            CreateMap<ContractorDTO, ContractorGrpcDTO>().ReverseMap();

            CreateMap(typeof(ContractorPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            
        }
    }
}
