using AutoMapper;
using Global.Models.PagedList;
using Notification.API.Protos;
using StaffApp.APIGateway.Models.NotificationModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {

            CreateMap<NotificationDTO, NotificationDTOGrpc>().ReverseMap();
            CreateMap<NotificationFilterModel, NotificationFilterModelGrpc>().ReverseMap();
            CreateMap<NotificationDTO, NotificationRequest>().ReverseMap();
            CreateMap<NotificationCommand, NotificationRequest>().ReverseMap();

            CreateMap(typeof(NotificationPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
