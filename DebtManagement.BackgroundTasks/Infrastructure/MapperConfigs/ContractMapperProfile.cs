using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.BackgroundTasks.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.BackgroundTasks.Infrastructure.MapperConfigs
{
    public class ContractMapperProfile : Profile
    {
        public ContractMapperProfile()
        {
            CreateMap<PromotionDetailGrpcModel, PromotionDetailDTO>();
            CreateMap<PromotionGrpcModel, PromotionDTO>();
        }
    }
}
