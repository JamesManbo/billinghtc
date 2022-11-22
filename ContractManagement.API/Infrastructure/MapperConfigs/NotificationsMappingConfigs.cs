using AutoMapper;
using ContractManagement.Domain.Models.Notification;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class NotificationsMappingConfigs : Profile
    {
        public NotificationsMappingConfigs()
        {
            CreateMap<PushNotificationRequest, PushNotificationRequestGrpc>().ReverseMap();
            CreateMap<SendMailRequest, SendMailRequestGrpc>().ReverseMap();
        }
    }
}
