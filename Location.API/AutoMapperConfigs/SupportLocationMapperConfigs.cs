using AutoMapper;
using Global.Models.Filter;
using Global.Models.PagedList;
using Location.API.Model;
using Location.API.Protos;

namespace Location.API.AutoMapperConfigs
{
    public class SupportLocationMapperConfigs : Profile
    {
        public SupportLocationMapperConfigs()
        {
            CreateMap<SupportLocation, SupportLocationGrpcDTO>().ReverseMap();
            CreateMap<RequestFilterGrpc, RequestFilterModel>().ReverseMap();

            CreateMap<IPagedList<SupportLocation>, SupportLocationPageListGrpcDTO>()
                .ForMember(s => s.Subset, 
                    m => m.MapFrom(e => e.Subset));

        }
    }
}
