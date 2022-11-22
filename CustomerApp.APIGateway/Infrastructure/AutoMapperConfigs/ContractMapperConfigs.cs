using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.CommonModels;
using CustomerApp.APIGateway.Models.OutContract;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ContractMapperConfigs : Profile
    {
        public ContractMapperConfigs()
        {
            CreateMap<AddressDTO, AddressGrpc>().ReverseMap();
            CreateMap<AttachmentFileDTO, AttachmentFileGrpcDTO>().ReverseMap();
            CreateMap<BillingTimeLineDTO, BillingTimeLineGrpc>().ReverseMap();
            CreateMap<ContractDTO, ContractGrpcDTO>().ReverseMap();
            CreateMap<ContractOfTaxDTO, ContractOfTaxGrpcDTO>().ReverseMap();
            CreateMap<ContractorDTO, ContractorGrpcDTO>().ReverseMap();
            CreateMap<ContractTimeLineDTO, ContractTimeLineGrpc>().ReverseMap();
            CreateMap<InstallationAddressDTO, InstallationAddressGrpc>().ReverseMap();
            CreateMap<MoneyDTO, Money>().ReverseMap();
            CreateMap<OutContractEquipmentDTO, OutContractEquipmentGrpcDTO>().ReverseMap();
            CreateMap<OutContractServicePackageDTO, OutContractServicePackageGrpcDTO>().ReverseMap();
            CreateMap<PaymentMethodDTO, PaymentMethodGrpc>().ReverseMap();
            CreateMap<Timestamp, DateTime>().ConvertUsing(s => s.ToDateTime());
            CreateMap<DateTime, Timestamp>().ConvertUsing(s => s.ToUniversalTime().ToTimestamp());

            CreateMap<ContractSimpleDTO, ContractGrpcDTO>().ReverseMap();
            CreateMap<OutContractServicePackageSimpleDTO, OutContractServicePackageGrpcDTO>().ReverseMap();
            CreateMap<OutputChannelPointDTO, OutputChannelPointGrpcDTO>().ReverseMap();
            CreateMap<ContractContentDTO, ContractContentDTOGrpc>().ReverseMap();
            CreateMap(typeof(ContractPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
