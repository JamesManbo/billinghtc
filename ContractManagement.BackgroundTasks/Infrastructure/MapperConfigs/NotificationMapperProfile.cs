using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ContractManagement.Domain.Models.Notification;
using Global.Models.Notification;
using Notification.API.Protos;

namespace ContractManagement.BackgroundTasks.Infrastructure.MapperConfigs
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {
            CreateMap<NotificationCommand, NotificationItem>().ReverseMap();
            CreateMap<PushNotificationRequest, PushNotificationRequestGrpc>().ReverseMap();
        }
    }
}
