using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.ContractModels.AppModel;
using StaffApp.APIGateway.Models.EquipmentModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Globalization;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class OutContractMapperProfile : Profile
    {
        public OutContractMapperProfile()
        {
            CreateMap<ContractGrpcDTO, ContractSimpleDTO>()
                .ForMember(d => d.ContractorFullName, m => m.MapFrom(e => e.Contractor.ContractorFullName))
                .ForMember(d => d.ContractorCode, m => m.MapFrom(e => e.Contractor.ContractorCode))
                .ForMember(d => d.ContractorPhone, m => m.MapFrom(e => e.Contractor.ContractorPhone))
                .ForMember(d => d.ContractorAddress, m => m.MapFrom(e => e.Contractor.ContractorAddress))
                .ReverseMap();
            CreateMap<RequestGetContractsGrpc, ContractFilterModel>().ReverseMap();
            CreateMap<MoneyDTO, Money>().ReverseMap();
            CreateMap<Money, decimal>().ConvertUsing(s => s == null ? 0m : Convert.ToDecimal(s.Value));
            CreateMap<decimal, Money>().ConvertUsing(s => new Money()
            {
                Value = s.ToString(CultureInfo.InvariantCulture),
                FormatValue = s == 0 ? "0" : string.Format("{0:#,0}", s),
                CurrencyCode = "VND"
            });

            CreateMap<ContractDTO, ContractGrpcDTO>().ReverseMap();
            CreateMap<ContractContentDTO, ContractContentDTOGrpc>().ReverseMap();
            CreateMap<OutContractServicePackageDTO, OutContractServicePackageGrpcDTO>().ReverseMap();
            CreateMap<OutContractServicePackageSimpleDTO, OutContractServicePackageGrpcDTO>().ReverseMap();
            CreateMap<OutContractIsSuccessDTO, OutContractIsSuccessGRPC>().ReverseMap();
            CreateMap<ContractTimeLineDTO, ContractTimeLineGrpc>().ReverseMap();
            CreateMap<AttachmentFileDTO, AttachmentFileGrpcDTO>().ReverseMap();
            CreateMap<OutputChannelPointDTO, OutputChannelPointGrpcDTO>().ReverseMap();
            CreateMap<OutContractEquipmentDTO, OutContractEquipmentGrpcDTO>().ReverseMap();
            CreateMap<OutputChannelFilterModel, OutputChannelPointRequestGrpc>().ReverseMap();

            
            CreateMap<CreateOutContract, CreateOutContractApp>().ReverseMap();
            CreateMap<CUContractServicePackage, CUContractServicePackageApp>().ReverseMap();
            CreateMap<CUOutputChannelPointCommand, CUOutputChannelPointCommandApp>().ReverseMap();
            CreateMap<CUContractEquipment, CUContractEquipmentApp>().ReverseMap();
            CreateMap<OutContractServicePackageTaxDTO, OutContractServicePackageTaxGrpcDTO>().ReverseMap();
            CreateMap<ContractStatusReportFilter, ContractStatusReportFilterGrpc>().ReverseMap();

            CreateMap(typeof(ContractPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

        }
    }
}
