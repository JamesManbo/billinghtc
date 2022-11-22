using ApplicationUserIdentity.API.Grpc.Clients;
using ApplicationUserIdentity.API.Models.NotificationModels;
using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Services.GRPC.Clients
{
    public interface INotificationGrpcService
    {
        Task<bool> PushNotificationByUids(PushNotificationRequest request, string uids);
        Task<bool> SendSms(SendSMSRequest request);
        Task<bool> SendEmail(SendMailRequest request);
    }
    public class NotificationGrpcService : GrpcCaller, INotificationGrpcService
    {
        private readonly IMapper _mapper;
        public NotificationGrpcService(IMapper mapper, ILogger<NotificationGrpcService> logger)
            : base(logger, MicroserviceRouterConfig.GrpcNotification)
        {
            _mapper = mapper;
        }

        public async Task<bool> PushNotificationByUids(PushNotificationRequest request, string uids)
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = _mapper.Map<PushNotificationRequestGrpc>(request);
                rq.Uids = uids;

                var resultGrpc = await client.PushNotificationByUidsAsync(rq);

                return resultGrpc.Success;
            });
        }

        public async Task<bool> SendEmail(SendMailRequest request)
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = _mapper.Map<SendMailRequestGrpc>(request);

                var resultGrpc = await client.SendMailAsync(rq);

                return resultGrpc.Success;
            });
        }

        public async Task<bool> SendSms(SendSMSRequest request)
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = _mapper.Map<SendSmsRequestGrpc>(request);

                var resultGrpc = await client.SendSmsAsync(rq);

                return resultGrpc.Success;
            });
        }
    }
}
