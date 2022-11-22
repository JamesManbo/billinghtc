using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.AcceptanceDTO;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using StaffApp.APIGateway.Models.TransactionsModel.App;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class TransactionMapperProfile : Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<RequestAddNewTransaction, RequestTransactionGRPC>().ReverseMap();
            CreateMap<IsSuccessGRPC, IsSuccessDTO>().ReverseMap();

            CreateMap<TransactionChannelPointDTO, TransactionChannelPointDTOGrpc>().ReverseMap();
            CreateMap<TransactionServicePackageDTO, TransactionServicePackageDTOGrpc>().ReverseMap();
            CreateMap<TransactionAttachmentFileDTO, TransactionAttachmentFileDTOGrpc>().ReverseMap();
            CreateMap<TransactionEquipmentDTO, TransactionEquipmentDTOGrpc>().ReverseMap();
            CreateMap<TransactionDTO, AcceptanceDTOGrpc>().ReverseMap();
            CreateMap<TransactionDTO, AcceptanceDTO>().ReverseMap();
            CreateMap<AcceptanceFilterModel, RequestGetAcceptancesGrpc>().ReverseMap();
            CreateMap<AcceptanceDTO, AcceptanceDTOGrpc>().ReverseMap();
            CreateMap<ErrorDTO, ErrorModelGRPC>().ReverseMap();
            CreateMap<CUTransactionEquipmentApp, CUTransactionEquipment>().ReverseMap();
            CreateMap<AddNewServicePackageTransactionApp, AddNewServicePackageTransactionCommand>().ReverseMap();
            CreateMap<CUTransactionEquipmentCommandApp, CUTransactionEquipmentCommand>().ReverseMap();
            CreateMap<CUTransactionChannelPointCommandApp, CUTransactionChannelPointCommand>().ReverseMap();
            CreateMap<AcceptanceTransactionCommand, AcceptanceTransactionCommandAppGrpc>().ReverseMap();
            CreateMap<TransactionEquipmentDTO, TransactionEquipmentCommandGrpc>().ReverseMap();
            CreateMap<FileCommandGrpc, CreateUpdateFile>().ReverseMap();
            CreateMap(typeof(AcceptancesPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));


            CreateMap<CUTransactionServicePackage, CUTransactionServicePackageApp>().ReverseMap();
        }
    }
}
