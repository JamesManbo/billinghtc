using AutoMapper;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContractOfTaxMapperConfigs : Profile
    {
        public ContractOfTaxMapperConfigs()
        {
            //CreateMap<OutContractTax, ContractOfTaxDTO>()
            //     .ForMember(dest => dest.Id, opts => opts.MapFrom(x => x.TaxCategoryId));
        }
    }
}
