using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class TransactionSupporterMapperConfigs : Profile
    {
        public TransactionSupporterMapperConfigs()
        {
            CreateMap<IEnumerable<TransactionSupporterDTO>, TransactionSupporterPageListGrpcDTO>().ReverseMap();
            CreateMap<TransactionSupporterDTO, TransactionSupporterDataDTOGrpc>().ReverseMap();
        }
    }
}
