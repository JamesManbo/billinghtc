using AutoMapper;
using Feedback.API.Protos;
using Global.Models.Filter;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class SupportMapperProfile : Profile
    {
        public SupportMapperProfile()
        {
            CreateMap<SupportCommand, CreateFeedbackAndRequestGrpc>().ReverseMap();
            CreateMap<SupportDTO, FeedbackAndRequestDTOGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, FeedbackAndRequestFilterGrpc>().ReverseMap();
            CreateMap<RequestSupportFilterModel, FeedbackAndRequestFilterGrpc>().ReverseMap();

            CreateMap(typeof(FeedbackAndRequestPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
