using ApplicationUserIdentity.API.Models.Otp;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class OtpMapperConfigs : Profile
    {
        public OtpMapperConfigs()
        {
            CreateMap<OtpDto, OtpDtoGrpc>().ReverseMap();
        }
    }
}
