using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class TransactionMapperConfig : Profile
    {
        public TransactionMapperConfig()
        {
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<CUTransactionCommand, Transaction>();
            CreateMap<CUTransactionCommand, TransactionDTO>().ReverseMap();

            CreateMap<CUAddNewServicePackageTransaction, Transaction>()
                .ForMember(s => s.TransactionServicePackages, m => m.Ignore())
                .ForMember(s => s.AttachmentFiles, m => m.Ignore());

            CreateMap<Transaction, CUTransactionSuspendServicePackagesCommand>().ReverseMap();
            CreateMap<Transaction, CUTransactionRestoreServicePackagesCommand>().ReverseMap();
            CreateMap<Transaction, CUTransactionTerminateServicePackagesCommand>().ReverseMap();
            CreateMap<CUChangeServicePackageTransaction, Transaction>().ReverseMap()
                .ForMember(s => s.TransactionServicePackages, m => m.Ignore())
                .ForMember(s => s.AttachmentFiles, m => m.Ignore());
            CreateMap<Transaction, CUTerminateContractCommand>().ReverseMap();
            CreateMap<Transaction, CUChangeLocationServicePackagesCommand>().ReverseMap();
            CreateMap<Transaction, CUChangeEquipmentsCommand>().ReverseMap();
            CreateMap<Transaction, CUUpgradeEquipmentsCommand>().ReverseMap();
            CreateMap<Transaction, CUReclaimEquipmentsCommand>().ReverseMap();
            CreateMap<Transaction, CUUpgradeBandwidthsCommand>().ReverseMap();

            CreateMap<TransactionRequestFilterModel, RequestGetAcceptancesGrpc>().ReverseMap();
            CreateMap<AcceptanceTransactionCommandApp, AcceptanceTransactionCommandAppGrpc>().ReverseMap();
            CreateMap<CreateUpdateFileCommand, FileCommandGrpc>().ReverseMap();
            CreateMap<TransactionEquipmentDTO, TransactionEquipmentCommandGrpc>().ReverseMap();

            CreateMap<AttachmentFileDTO, TransactionAttachmentFileDTOGrpc>();
            CreateMap<TransactionDTO, AcceptanceDTOGrpc>();
            CreateMap<AcceptanceDTO, AcceptanceDTOGrpc>();
            CreateMap<IPagedList<TransactionDTO>, AcceptancesPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

        }
        
    }
}
