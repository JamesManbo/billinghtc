
using AutoMapper;
using ContractManagement.API.Protos;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class TransactionSupporterMapperProfile : Profile
    {
        public TransactionSupporterMapperProfile()
        {
            CreateMap<TransactionSupporterDTO, TransactionSupporterDataDTOGrpc>().ReverseMap();
            CreateMap<TransactionSupporterFilterModel, TransactionSupporterFilterGrpc>().ReverseMap();
        }
    }
}
