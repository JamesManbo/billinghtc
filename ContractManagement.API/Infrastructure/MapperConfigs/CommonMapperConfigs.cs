using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class CommonMapperConfigs : Profile
    {
        public CommonMapperConfigs()
        {
            CreateMap<RequestFilterGrpc, Global.Models.Filter.RequestFilterModel>().ReverseMap();
            CreateMap<SelectionItemDTOGrpc, SelectionItem>().ReverseMap();
            CreateMap<IEnumerable<SelectionItem>, LstSelectionItemDTOGrpc>().ForMember(s => s.LstSelectionItemDTOGrpc_, m => m.MapFrom(d => d));

        }
    }
}
