using AutoMapper;
using StaffApp.APIGateway.Models.OtpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class OtpMapperProfile : Profile
    {
        public OtpMapperProfile()
        {
            CreateMap<OtpDto, OtpDtoGrpc>().ReverseMap();
            CreateMap<UpdateOtpUsedResponse, UpdateOtpUsedResponseGrpc>().ReverseMap();

        }
    }
}
