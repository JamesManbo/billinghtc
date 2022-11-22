using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ChannelGroupMapperConfigs : Profile
    {
        public ChannelGroupMapperConfigs()
        {
            CreateMap<ChannelGroups, ChannelGroupDTO>().ReverseMap();
            CreateMap<ChannelGroups, SelectionItem>().ReverseMap();
        }
    }
}
