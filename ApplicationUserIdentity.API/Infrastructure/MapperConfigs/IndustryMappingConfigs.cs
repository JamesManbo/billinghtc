using ApplicationUserIdentity.API.Application.Commands.Industry;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class IndustryMappingConfigs : Profile
    {
        public IndustryMappingConfigs()
        {
            CreateMap<Industry, CUIndustryCommand>().ReverseMap();
            CreateMap<IndustryDTO, IndustryModelGrpc>().ReverseMap();
            CreateMap<IPagedList<IndustryDTO>, IndustryPageListGrpc>()
               .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));
        }
    }
}
