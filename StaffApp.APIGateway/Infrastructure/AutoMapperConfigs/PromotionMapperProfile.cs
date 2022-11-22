using AutoMapper;
using ContractManagement.API.Protos;
using StaffApp.APIGateway.Models.PromotionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class PromotionMapperProfile : Profile
    {
        public PromotionMapperProfile()
        {
            CreateMap<AvailablePromotionDTO, PromotionDTOGrpc>().ReverseMap();
        }
    }
}
