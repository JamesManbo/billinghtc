using ApplicationUserIdentity.API.Models.NotificationModels;
using AutoMapper;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class NotificationMappingConfigs : Profile
    {
        public NotificationMappingConfigs()
        {
            CreateMap<PushNotificationRequest, PushNotificationRequestGrpc>().ReverseMap();
            CreateMap<SendMailRequest, SendMailRequestGrpc>().ReverseMap();
            CreateMap<SendSMSRequest, SendSmsRequestGrpc>().ReverseMap();
        }
    }
}
