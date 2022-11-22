using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Notification;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface INotificationGrpcService
    {
        Task<long> BulkInsert(List<NotificationItem> notifications);
        Task<bool> SendSMS(SendSmsRequestGrpc smsContent);
    }

    public class NotificationGrpcService : GrpcCaller, INotificationGrpcService
    {
        private readonly IMapper _mapper;
        public NotificationGrpcService(IMapper mapper, ILogger<GrpcCaller> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcNotification)
        {
            _mapper = mapper;
        }

        public async Task<long> BulkInsert(List<NotificationItem> notifications)
        {
            return await CallAsync(async channel =>
            {
                var request = new NotificationGrpcRequest();
                for (int i = 0; i < notifications.Count; i++)
                {
                    request.NotificationCommands.Add(this._mapper.Map<NotificationCommand>(notifications.ElementAt(i)));
                }

                var client = new NotificationGrpc.NotificationGrpcClient(channel);
                var insertedRecords = await client.BulkInsertAsync(request);
                return insertedRecords.Value;
            });
        }

        public async Task<bool> SendSMS(SendSmsRequestGrpc smsContent)
        {
            return await CallAsync(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);
                var grpcResp = await client.SendSmsAsync(smsContent);
                return grpcResp?.Success ?? false;
            });
        }
    }
}
