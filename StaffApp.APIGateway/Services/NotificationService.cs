using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;
using StaffApp.APIGateway.Models.NotificationModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface INotificationService
    {
        Task<IPagedList<NotificationDTO>> GetListByReceiver(NotificationFilterModel filterModel);
        Task<NotificationDTO> GetById(string id);
        Task<NotificationDTO> UpdateViewed(string id);
    }
    public class NotificationService : GrpcCaller, INotificationService
    {
        private readonly IMapper _mapper;
        public NotificationService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcNotification)
        {
            _mapper = mapper;
        }

        public async Task<NotificationDTO> GetById(string id)
        {
            return await Call<NotificationDTO>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var lstServiceGrpc = await client.GetByIdAsync(new StringValue() { Value = id });

                return _mapper.Map<NotificationDTO>(lstServiceGrpc);
            });
        }

        public async Task<IPagedList<NotificationDTO>> GetListByReceiver(NotificationFilterModel filterModel)
        {
            return await Call<IPagedList<NotificationDTO>>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);
                var request = _mapper.Map<NotificationFilterModelGrpc>(filterModel);

                var lstServiceGrpc = await client.GetListByReceiverAsync(request);

                return _mapper.Map<IPagedList<NotificationDTO>>(lstServiceGrpc);
            });
        }

        public async Task<NotificationDTO> UpdateViewed(string id)
        {
            return await Call<NotificationDTO>(async channel =>
            {
                var client = new NotificationGrpc.NotificationGrpcClient(channel);

                var lstServiceGrpc = await client.UpdateViewedNotificationAsync(new StringValue {Value = id });

                return _mapper.Map<NotificationDTO>(lstServiceGrpc);
            });
        }
    }
}
