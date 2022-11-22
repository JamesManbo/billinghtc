using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.BackgroundTasks.Services.Grpc;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.OrganizationModels;
using OrganizationUnit.API.Protos.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.BackgroundTasks.Infrastructure.MapperConfigs
{
    public class CommonMapperProfile : Profile
    {
        public CommonMapperProfile()
        {
            CreateMap<TaxCategoryGrpcDTO, TaxCategoryDTO>();
            CreateMap<OrganizationUnitGrpcDTO, OrganizationUnitDTO>();
            CreateMap<ExchangeRateDTOGrpc, ExchangeRateDTO>();
        }
    }
}
