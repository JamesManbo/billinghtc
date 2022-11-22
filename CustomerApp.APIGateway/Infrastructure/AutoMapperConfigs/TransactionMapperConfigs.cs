using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.TransactionModels;
using CustomerApp.APIGateway.Models.TransactionModels.RequestApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class TransactionMapperConfigs : Profile
    {
        public TransactionMapperConfigs()
        {
            CreateMap<RequestTransactionGRPC, RequestAddNewTransaction>().ReverseMap(); 
            CreateMap<IsSuccessGRPC, IsSuccessDTO>().ReverseMap();
            CreateMap<ErrorModel, ErrorModelGRPC>().ReverseMap();

            CreateMap<CUAddNewServicePackageTransactionApp, CUAddNewServicePackageTransaction>().ReverseMap();
            CreateMap<CUTransactionServicePackageCommandApp, CUTransactionServicePackageCommand>().ReverseMap();
            CreateMap<CUTransactionChannelPointCommandApp, CUTransactionChannelPointCommand>().ReverseMap();

        }
    }
}
