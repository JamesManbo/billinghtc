using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class CommonMapperProfile : Profile
    {
        public CommonMapperProfile()
        {
            CreateMap<RequestFilterModel, OrganizationUnit.API.Protos.Organizations.RequestFilterGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, OrganizationUnit.API.Protos.Users.RequestFilterGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, RequestFilterGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, Location.API.Protos.RequestFilterGrpc>().ReverseMap();
            CreateMap<UsersInGroupRequestFilterModel, UsersInGroupRequestFilterModelGrpc>().ReverseMap();
           
            CreateMap<Timestamp, DateTime>().ConvertUsing(s => s.ToDateTime());
            CreateMap<DateTime, Timestamp>().ConvertUsing(s => s.ToUniversalTime().ToTimestamp());

            CreateMap<PaymentMethod, PaymentMethodGrpc>().ReverseMap();
            CreateMap<ContractTimeLine, ContractTimeLineGrpc>().ReverseMap();
            CreateMap<BillingTimeLineDTO, BillingTimeLineGrpc>().ReverseMap();
            CreateMap<BillingTimeLineDTO, BillingTimeLine>().ReverseMap();
            CreateMap<BillingTimeLineDTO, BillingTimeLine>().ReverseMap();
            CreateMap<InstallationAddress, InstallationAddressGrpc>().ReverseMap();
            CreateMap<SelectionItemDTO, SelectionItemDTOGrpc>().ReverseMap();
        }
    }
}
