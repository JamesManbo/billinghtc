using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class TaxCategoryMapperConfigs : Profile
    {
        public TaxCategoryMapperConfigs()
        {
            CreateMap<TaxCategoryDTO, TaxCategoryGrpcDTO>().ReverseMap();
        }
    }
}
