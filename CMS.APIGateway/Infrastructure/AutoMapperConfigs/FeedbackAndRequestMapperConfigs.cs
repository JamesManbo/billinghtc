using AutoMapper;
using CMS.APIGateway.Models.FeedbackAndRequest;
using Feedback.API.Protos;
using Global.Models.Filter;
using Global.Models.PagedList;

namespace CMS.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class FeedbackAndRequestMapperConfigs : Profile
    {
        public FeedbackAndRequestMapperConfigs()
        {
            CreateMap<CUFeedbackAndRequest, CreateFeedbackAndRequestGrpc>().ReverseMap();
            CreateMap<FeedbackAndRequestDtoGrpc, FeedbackAndRequestDTOGrpc>().ReverseMap();
            CreateMap<FeedbackAndRequestFilterGrpc, FeedbackAndRequestFilterModel>().ReverseMap();

            CreateMap<FeedbackAndRequestPageListGrpcDTO, IPagedList<FeedbackAndRequestDtoGrpc>>()
                .ForMember(e => e.Subset, s => s.MapFrom(e => e.Subset));
           
            //CreateMap<IPagedList<FeedbackAndRequestDtoGrpc>, FeedbackAndRequestPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

        }
    }
}
