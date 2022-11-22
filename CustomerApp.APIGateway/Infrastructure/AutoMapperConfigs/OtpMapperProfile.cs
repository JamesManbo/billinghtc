using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using CustomerApp.APIGateway.Models.OtpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
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
