using AutoMapper;
using ContractManagement.API.Protos;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class UnitOfMeasurementMapperProfile : Profile
    {
        public UnitOfMeasurementMapperProfile()
        {

            CreateMap<UnitOfMeasurementFilterModel, UnitOfMeasurementFilterGrpc>().ReverseMap();
        }
    }
}
