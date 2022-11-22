using AutoMapper;
using ContractManagement.API.Grpc.Servers;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class TransactionServicePackageMapperConfig : Profile
    {
        public TransactionServicePackageMapperConfig()
        {
            CreateMap<TransactionChannelPoint, TransactionChannelPointDTO>();
            CreateMap<TransactionServicePackage, TransactionServicePackageDTO>();
            CreateMap<TransactionChannelPointDTO, TransactionChannelPointDTOGrpc>();

            CreateMap<CUTransactionChannelPointCommand, TransactionChannelPoint>();
            CreateMap<CUOutContractChannelCommand, TransactionServicePackage>()
                .ForMember(d => d.Id, m => m.Ignore());

            CreateMap<CUServicePackageOfChangeCommand, TransactionServicePackage>();

            CreateMap<CUContractEquipmentCommand, CUTransactionEquipmentCommand>();
            CreateMap<CUOutputChannelPointCommand, CUTransactionChannelPointCommand>();
            CreateMap<CUTransactionSLACommand, CUServiceLevelAgreementCommand>();

            CreateMap<CUOutContractChannelCommand, CUTransactionServicePackageCommand>()
                .ForMember(s => s.ServiceLevelAgreements, m => m.MapFrom(d => d.ServiceLevelAgreements))
                .ForMember(s => s.StartPoint, m => m.MapFrom(d => d.StartPoint))
                .ForMember(s => s.EndPoint, m => m.MapFrom(d => d.EndPoint))
                .ForMember(s => s.TransactionPromotionForContracts, m => m.MapFrom(d => d.PromotionForContractNews))
                .ReverseMap();

            CreateMap<TransactionPromotionForContract, PromotionContract>().ReverseMap();

            CreateMap<CUTransactionServicePackageCommand, OutContractServicePackage>()
                .ForMember(s => s.Id, m => m.MapFrom(d => d.OutContractServicePackageId))
                .ForMember(s => s.StartPoint, m => m.MapFrom(d => d.StartPoint))
                .ForMember(s => s.EndPoint, m => m.MapFrom(d => d.EndPoint))
                .ForMember(s => s.PaymentTarget, m => m.MapFrom(d => d.PaymentTarget))
                .ForMember(s => s.ServiceLevelAgreements, m => m.MapFrom(d => d.ServiceLevelAgreements))
                .ReverseMap();


            CreateMap<TransactionServicePackage, TransactionServicePackageDTO>().ReverseMap();
            CreateMap<CUTransactionServicePackageCommand, TransactionServicePackageDTO>().ReverseMap();
            CreateMap<PromotionContract, TransactionPromotionForContactDTO>().ReverseMap();
            CreateMap<CUServicePackageOfChangeCommand, TransactionServicePackageDTO>().ReverseMap();
            CreateMap<TransactionServicePackageDTO, TransactionServicePackageDTOGrpc>().ReverseMap();
            CreateMap<TransactionPromotionForContract, TransactionPromotionForContactDTO>().ReverseMap();

            CreateMap<TransactionPromotionForContract, CreateAppliedPromotionCommand>()
                .ForMember(s => s.Id, d => d.Ignore());
            CreateMap<TransactionServiceLevelAgreement, CUServiceLevelAgreementCommand>()
                .ForMember(s => s.Id, d => d.MapFrom(e => e.ContractSlaId ?? 0));

            CreateMap<TransactionChannelTax, CUOutContractServicePackageTaxCommand>()
                .ForMember(s => s.Id, m => m.MapFrom(e => e.ContractChannelTaxId ?? 0))
                .ForMember(s => s.TaxCategoryCode, m => m.MapFrom(d => d.TaxCategoryCode))
                .ForMember(s => s.TaxCategoryId, m => m.MapFrom(d => d.TaxCategoryId))
                .ForMember(s => s.TaxCategoryName, m => m.MapFrom(d => d.TaxCategoryName))
                .ForMember(s => s.TaxValue, m => m.MapFrom(d => d.TaxValue));

            CreateMap<TransactionEquipment, CUContractEquipmentCommand>()
                .ForMember(s => s.TransactionEquipmentId, d => d.MapFrom(e => e.Id))
                .ForMember(s => s.Id, d => d.MapFrom(e => e.ContractEquipmentId));

            CreateMap<TransactionChannelPoint, CUOutputChannelPointCommand>()
                .ForMember(s => s.Id, d => d.MapFrom(e => e.ContractPointId));

            CreateMap<TransactionPriceBusTable, CUChannelPriceBusTableCommand>()
                .ForMember(c => c.Id, d => d.MapFrom(e => e.ContractPbtId));

            CreateMap<TransactionServicePackage, CUOutContractChannelCommand>()
                .ForMember(s => s.OutContractServicePackageTaxes, d => d.MapFrom(e => e.TaxValues))
                .ForMember(s => s.PaymentTarget, d => d.Ignore())
                .ForMember(s => s.Id, d => d.Ignore());

            CreateMap<CUTransactionServicePackageCommand, TransactionServicePackage>();

            CreateMap<TransactionPriceBusTable, TransactionPriceBusTableDTO>();
        }
    }
}
