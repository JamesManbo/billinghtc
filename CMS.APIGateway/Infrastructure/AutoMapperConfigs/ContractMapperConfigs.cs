using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.APIGateway.Models;
using ContractManagement.API.Protos;

namespace CMS.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ContractMapperConfigs : Profile
    {
        public ContractMapperConfigs()
        {
            CreateMap<ContractDTO, ContractGrpcDTO>().ReverseMap();

            //CreateMap<CreateDraftContractCommand, CreateContractGrpcCommand>().ReverseMap();
        }
    }
}
