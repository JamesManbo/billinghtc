using AutoMapper;
using Feedback.API.Models;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedback.API.Infrastructure.MapperConfigs
{
    public class NotificationMapperConfigs : Profile
    {
        public NotificationMapperConfigs()
        {
            CreateMap<PushNotificationRequest, PushNotificationRequestGrpc>();
        }
    }
}
