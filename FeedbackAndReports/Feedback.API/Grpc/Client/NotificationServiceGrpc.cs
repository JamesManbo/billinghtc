using AutoMapper;
using Feedback.API.Models;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedback.API.Grpc.Client
{
    public interface INotificationGrpcService
    {
        Task<bool> PushNotificationByRole(PushNotificationRequest request, string roleCode);
        Task<bool> PushNotificationByCustomerUids(PushNotificationRequest request, string uids);
    }
    public class NotificationServiceGrpc : GrpcCaller, INotificationGrpcService
    {
        private readonly IMapper _mapper;
        public NotificationServiceGrpc(ILogger<GrpcCaller> logger, IMapper mapper)
            : base(logger, MicroserviceRouterConfig.GrpcNotification)
        {
            this._mapper = mapper;
        }

        public async Task<bool> PushNotificationByRole(PushNotificationRequest request, string roleCode)
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = _mapper.Map<PushNotificationRequestGrpc>(request);
                rq.RoleCode = roleCode;
                var resultGrpc = await client.PushNotificationByRoleAsync(rq);

                return resultGrpc.Success;
            });
        }

        public async Task<bool> PushNotificationByCustomerUids(PushNotificationRequest request, string uids)
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = _mapper.Map<PushNotificationRequestGrpc>(request);
                rq.Uids = uids;

                var resultGrpc = await client.PushNotificationByCustomerUidsAsync(rq);

                return resultGrpc.Success;
            });
        }
    }
}
