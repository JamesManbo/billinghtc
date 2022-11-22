using AutoMapper;
using OrganizationUnit.Domain.Models.Otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;

namespace SystemUserIdentity.API.Infrastructure.AutoMapperConfigs
{
    public class OtpMapperConfigs : Profile
    {
        public OtpMapperConfigs()
        {
            CreateMap<OtpDto, OtpDtoGrpc>().ReverseMap();
        }
    }
}
