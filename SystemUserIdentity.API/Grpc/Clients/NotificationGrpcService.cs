using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemUserIdentity.API.Grpc.Clients
{
    public interface INotificationGrpcService
    {
        Task<bool> SendSms(string phones, string message);
    }
    public class NotificationGrpcService : GrpcCaller, INotificationGrpcService
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;
        public NotificationGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger)
   : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcNotification)
        {
            _wrappedConfig = wrappedConfig;
        }

        public async Task<bool> SendSms(string phones, string message)
        {
            return await Call<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var resultGrpc = await client.SendSmsAsync(new SendSmsRequestGrpc { PhoneNumbers = phones, Message = message});

                return resultGrpc.Success;
            });
        }
    }
}
