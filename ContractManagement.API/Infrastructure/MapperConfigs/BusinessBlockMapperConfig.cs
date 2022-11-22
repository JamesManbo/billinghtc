using AutoMapper;
using ContractManagement.API.Application.Commands.BusinessBlockCommandHandler;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class BusinessBlockMapperConfig : Profile
    {
        public BusinessBlockMapperConfig()
        {
            CreateMap<ManagementBusinessBlock, BusinessBlockDTO>().ReverseMap();
            CreateMap<ManagementBusinessBlock, BusinessBlockCommand>().ReverseMap();
            CreateMap<BusinessBlockDTO, BusinessBlockCommand>().ReverseMap();
        }
    }
}
