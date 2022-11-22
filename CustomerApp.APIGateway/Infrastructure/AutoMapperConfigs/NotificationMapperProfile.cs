using AutoMapper;
using CustomerApp.APIGateway.Models.NotificationModels;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Models.PagedList;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {

            CreateMap<NotificationDTO, NotificationDTOGrpc>().ReverseMap();
            CreateMap<NotificationDTO, NotificationRequest>().ReverseMap();
            CreateMap<NotificationFilterModel, NotificationFilterModelGrpc>().ReverseMap();
            CreateMap<NotificationCommand, NotificationRequest>().ReverseMap();

            CreateMap(typeof(NotificationPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
