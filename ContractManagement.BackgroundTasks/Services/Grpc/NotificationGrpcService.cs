using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.Domain.Models.Notification;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Notification;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;

namespace ContractManagement.BackgroundTasks.Services.Grpc
{
    public interface INotificationGrpcService
    {
        Task<bool> PushNotificationByDepartment(PushNotificationRequest request, string departmentCode);
        Task<bool> PushNotificationByRole(PushNotificationRequest request, string roleCode);
        Task<bool> PushNotificationByUids(PushNotificationRequest request, string uids = "");
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
            return await Call<long>(async channel =>
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

        public async Task<bool> PushNotificationByDepartment(PushNotificationRequest request, string departmentCode)
        {
            return await Call<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var grpcRequest = _mapper.Map<PushNotificationRequestGrpc>(request);
                grpcRequest.DepartmentCode = departmentCode;
                var resultGrpc = await client.PushNotificationByDepartmentAsync(grpcRequest);

                return resultGrpc.Success;
            });
        }

        public async Task<bool> PushNotificationByRole(PushNotificationRequest request, string roleCode)
        {
            return await Call<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var grpcRequest = _mapper.Map<PushNotificationRequestGrpc>(request);
                grpcRequest.RoleCode = roleCode;
                var resultGrpc = await client.PushNotificationByRoleAsync(grpcRequest);

                return resultGrpc.Success;
            });
        }

        public async Task<bool> PushNotificationByUids(PushNotificationRequest request, string uids = "")
        {
            return await Call<bool>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var grpcRequest = _mapper.Map<PushNotificationRequestGrpc>(request);
                grpcRequest.Uids = string.IsNullOrEmpty(uids) ? request.ReceiverId : uids;

                var resultGrpc = await client.PushNotificationByUidsAsync(grpcRequest);

                return resultGrpc.Success;
            });
        }
    }
}
