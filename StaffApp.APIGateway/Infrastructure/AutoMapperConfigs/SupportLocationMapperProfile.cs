using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using Location.API.Protos;
using StaffApp.APIGateway.Models.SupportLocationModel;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class SupportLocationMapperProfile : Profile
    {
        public SupportLocationMapperProfile()
        {

            CreateMap<SupportLocationDTO, SupportLocationGrpcDTO>().ReverseMap();

           // CreateMap(typeof(TaxCategoryPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            CreateMap<SupportLocationPageListGrpcDTO, IPagedList<SupportLocationDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
