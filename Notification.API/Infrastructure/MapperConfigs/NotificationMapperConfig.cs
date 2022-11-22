using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Models.Filter;
using Global.Models.Notification;
using Global.Models.PagedList;
using Notification.API.Models;
using Notification.API.Protos;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Users;

namespace Notification.API.Infrastructure.MapperConfigs
{
    public class NotificationMapperConfig : Profile
    {
        public NotificationMapperConfig()
        {
            CreateMap<NotificationFilterModelGrpc, NotificationFilterModel>().ReverseMap();
            CreateMap<NotificationCommand, Models.Notification>().ReverseMap();
            CreateMap<NotificationItem, Models.Notification>().ReverseMap();

            CreateMap<Models.Notification, NotificationDTOGrpc>();
            CreateMap<IPagedList<Models.Notification>, NotificationPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

            CreateMap<PushNotificationRequestGrpc, Models.Notification>().ReverseMap();
            CreateMap<SendSMSRequest, SendSmsRequestGrpc>().ReverseMap();
            CreateMap<SendMailRequest, SendMailRequestGrpc>().ReverseMap();

            CreateMap<UserTokenModel, OrganizationUnit.API.Protos.Users.FcmTokenGrpc>().ReverseMap();
            CreateMap<UserTokenModel, ApplicationUserIdentity.API.Protos.FcmTokenGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, UsersInGroupRequestFilterModelGrpc>().ReverseMap();
        }
    }
}