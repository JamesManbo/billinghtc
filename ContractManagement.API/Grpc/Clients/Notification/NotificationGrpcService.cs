using AutoMapper;
using ContractManagement.Domain.Models.Notification;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Clients
{
    public interface INotificationGrpcService
    {
        Task<bool> PushNotificationByRole(PushNotificationRequest request, string roleCode);
        Task<bool> PushNotificationByUids(PushNotificationRequest request, string uids = "");
        Task<bool> SendEmail(SendMailRequest request);
    }
    public class NotificationGrpcService : GrpcCaller, INotificationGrpcService
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;
        private readonly IMapper _mapper;
        public NotificationGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger, IMapper mapper)
            : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcNotification)
        {
            _wrappedConfig = wrappedConfig;
            _mapper = mapper;
        }

        public async Task<bool> PushNotificationByRole(PushNotificationRequest request, string roleCode)
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = request.MapTo<PushNotificationRequestGrpc>(_wrappedConfig.MapperConfig);
                rq.RoleCode = roleCode;

                var resultGrpc = await client.PushNotificationByRoleAsync(rq);

                return resultGrpc.Success;
            });
        }

        public async Task<bool> PushNotificationByUids(PushNotificationRequest request, string uids = "")
        {
            return await CallAsync<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var rq = request.MapTo<PushNotificationRequestGrpc>(_wrappedConfig.MapperConfig);
                rq.Uids = string.IsNullOrEmpty(uids) ? request.ReceiverId : uids;

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
    }
}
