using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Models.SupportModels;
using Feedback.API.Protos;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class SupportMapperConfigs: Profile
    {
        public SupportMapperConfigs()
        {
            CreateMap<ContractFilterModel, RequestGetContractsGrpc>().ReverseMap();
            CreateMap<SupportRequest, CreateFeedbackAndRequestGrpc>().ReverseMap();
            CreateMap<SupportDTO, FeedbackAndRequestDTOGrpc>().ReverseMap();
            CreateMap<SupportDTO, SupportRequest>().ReverseMap();
            CreateMap<SupportRequestFilterModel, FeedbackAndRequestFilterGrpc>().ReverseMap();
            CreateMap(typeof(FeedbackAndRequestPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
