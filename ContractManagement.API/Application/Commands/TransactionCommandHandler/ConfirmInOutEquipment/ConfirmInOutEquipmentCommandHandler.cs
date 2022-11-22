using ContractManagement.API.Grpc.Clients;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Repositories;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Auth;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ConfirmInOutEquipment
{
    public class ConfirmInOutEquipmentCommandHandler : IRequestHandler<CUTransactionCommand, ActionResponse<TransactionDTO>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly INotificationGrpcService _notificationGrpcService;
        public ConfirmInOutEquipmentCommandHandler(ITransactionRepository transactionRepository,
            INotificationGrpcService notificationGrpcService, 
            IWrappedConfigAndMapper configAndMapper)
        {
            _transactionRepository = transactionRepository;
            _notificationGrpcService = notificationGrpcService;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(CUTransactionCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<TransactionDTO>();

            var transaction = await _transactionRepository.GetByIdAsync(request.Id);
            transaction.StatusId = request.StatusId;
            var updateTransaction = await _transactionRepository.UpdateAndSave(transaction);
            actionResponse.SetResult(actionResponse.Result.MapTo<TransactionDTO>(_configAndMapper.MapperConfig));
            actionResponse.CombineResponse(updateTransaction);

            var userUids = transaction.CreatorUserId;
            var notiReq = new PushNotificationRequest()
            {
                Zone = NotificationZone.Contract,
                Type = NotificationType.App,
                Category = NotificationCategory.ContractTransaction,
                Title = $"Phụ lục {transaction.Code} chuyển trạng thái sang Thiết bị đã nhập kho",
                Content = $"Phụ lục {updateTransaction.Result.Code} chuyển trạng thái từ Đã nghiệm thu sang Thiết bị đã nhập kho." ,
                Payload = JsonConvert.SerializeObject(new
                {
                    Type = transaction.Type,
                    TypeName = TransactionType.GetTypeName(transaction.Type),
                    Id = updateTransaction.Result.Id,
                    Code = updateTransaction.Result.Code,
                    Category = NotificationCategory.ContractTransaction
                },
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
            };

            await _notificationGrpcService.PushNotificationByUids(notiReq, userUids);
            return actionResponse;
        }
    }
}
