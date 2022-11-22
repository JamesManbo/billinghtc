using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class OutContractServicePackageMapperConfigs : Profile
    {
        public OutContractServicePackageMapperConfigs()
        {
            CreateMap<ChannelAddressModel, ChannelAddressModelGrpc>();
            CreateMap<OutContractServicePackageDTO, OutContractServicePackageGrpcDTO>().ReverseMap();
            CreateMap<ContractPackageGridDTO, OutContractServicePackageGrpcDTO>().ReverseMap();

            CreateMap<CUOutContractChannelCommand, OutContractServicePackage>();
            CreateMap<OutContractServicePackage, OutContractServicePackageDTO>()
                .ForMember(d => d.OutContractServicePackageTaxes, s => s.MapFrom(e => e.TaxValues));
            CreateMap<OutContractServicePackage, OutContractServicePackageGrpcDTO>();

            CreateMap<OutputChannelPoint, OutputChannelPointDTO>().ReverseMap();
            CreateMap<OutputChannelPointDTO, OutputChannelPointGrpcDTO>().ReverseMap();
            CreateMap<CUOutputChannelPointCommand, OutputChannelPoint>();

            CreateMap<OutContractServicePackageTax, OutContractServicePackageTaxDTO>();
            CreateMap<OutContractServicePackageTax, ImportOutContractPackageTax>();
            CreateMap<OutContractServicePackageTaxDTO, OutContractServicePackageTaxGrpcDTO>().ReverseMap();

            CreateMap<ChannelPriceBusTable, ChannelPriceBusTableDTO>();

            CreateMap<OutContractServicePackage, ImportOutContractServicePackage>()
                .ForMember(d => d.TimeLine_PrepayPeriod, m => m.MapFrom(s => s.TimeLine.PrepayPeriod))
                .ForMember(d => d.TimeLine_PaymentPeriod, m => m.MapFrom(s => s.TimeLine.PaymentPeriod))
                .ForMember(d => d.TimeLine_Effective, m => m.MapFrom(s => s.TimeLine.Effective))
                .ForMember(d => d.TimeLine_Signed, m => m.MapFrom(s => s.TimeLine.Signed))
                .ForMember(d => d.TimeLine_StartBilling, m => m.MapFrom(s => s.TimeLine.StartBilling))
                .ForMember(d => d.TimeLine_LatestBilling, m => m.MapFrom(s => s.TimeLine.LatestBilling))
                .ForMember(d => d.TimeLine_NextBilling, m => m.MapFrom(s => s.TimeLine.NextBilling))
                .ForMember(d => d.TimeLine_SuspensionStartDate, m => m.MapFrom(s => s.TimeLine.SuspensionStartDate))
                .ForMember(d => d.TimeLine_SuspensionEndDate, m => m.MapFrom(s => s.TimeLine.SuspensionEndDate))
                .ForMember(d => d.TimeLine_DaysSuspended, m => m.MapFrom(s => s.TimeLine.DaysSuspended))
                .ForMember(d => d.TimeLine_DaysPromotion, m => m.MapFrom(s => s.TimeLine.DaysPromotion))
                .ForMember(d => d.TimeLine_PaymentForm, m => m.MapFrom(s => s.TimeLine.PaymentForm))
                .ForMember(d => d.TaxValues, m => m.MapFrom(s => s.TaxValues));

            CreateMap<PromotionDetailDTO, PromotionDetailGrpcModel>();
            CreateMap<AvailablePromotionDto, PromotionGrpcModel>();
        }
    }
}
