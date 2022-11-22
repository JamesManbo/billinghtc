using ContractManagement.API.Grpc.Clients;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpgradeEquipments
{
    public class CUUpgradeEquipmentsCommandHandler : IRequestHandler<CUUpgradeEquipmentsCommand, ActionResponse<TransactionDTO>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly IOutContractQueries _outContractQueries;

        public CUUpgradeEquipmentsCommandHandler(ITransactionRepository transactionRepository,
            INotificationGrpcService notificationGrpcService,
            ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IOutContractQueries outContractQueries)
        {
            _transactionRepository = transactionRepository;
            _notificationGrpcService = notificationGrpcService;
            _transactionQueries = transactionQueries;
            _projectQueries = projectQueries;
            _outContractQueries = outContractQueries;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(CUUpgradeEquipmentsCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionDTO>();
            request.StatusId = TransactionStatus.WaitAcceptanced.Id;
            request.Type = TransactionType.UpgradeEquipments.Id;

            if (request.OutContractId > 0)
            {
                if (request.IsAppendix == true)
                {
                    request.Code = "PL" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractId, true) + "_" + request.ContractCode;
                }
                else
                {
                    request.Code = "TS" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractId, false) + "_" + request.ContractCode;
                }
                var savedRsp = await _transactionRepository.CreateAndSave(request);
                actionResp.CombineResponse(savedRsp);

                var userIds = _projectQueries.GetAvaliableSupporterByOutContractId(request.OutContractId);
                if (userIds != null && userIds.Any())
                {
                    var notiReq = new PushNotificationRequest()
                    {
                        Zone = NotificationZone.Contract,
                        Type = NotificationType.App,
                        Category = NotificationCategory.Acceptance,
                        Title = request.Code != null ? request.Code : "",
                        Content = TransactionType.UpgradeEquipments.Name,
                        Payload = JsonConvert.SerializeObject(new { Type = (int)TransactionTypeEnums.UpgradeEquipments, Id = savedRsp.Result.Id, Category = NotificationCategory.Acceptance }, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        })
                    };
                    await _notificationGrpcService.PushNotificationByUids(notiReq, string.Join(',', userIds));
                }
                else
                {
                    var projectDTO = _projectQueries.FindByOutContractId(request.OutContractId);
                    actionResp.AddError($"Dự án {projectDTO.ProjectName} không có Supporter");
                    return actionResp;
                }
            }
            else if (request.OutContractIds != null)
            {
                for (int i = 0; i < request.OutContractIds.Count(); i++)
                {
                    string outContractCode = _outContractQueries.OutContractCodeById(request.OutContractIds.ElementAt(i));
                    if (request.IsAppendix == true)
                    {
                        request.Code = "PL" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractIds.ElementAt(i), true) + "_" + outContractCode;
                    }
                    else
                    {
                        request.Code = "TS" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractIds.ElementAt(i), false) + "_" + outContractCode;
                    }
                    var savedRsp = await _transactionRepository.CreateAndSave(request);
                    actionResp.CombineResponse(savedRsp);

                    var userIds = _projectQueries.GetAvaliableSupporterByOutContractId(request.OutContractIds.ElementAt(i));
                    if (userIds != null && userIds.Any())
                    {
                        var notiReq = new PushNotificationRequest()
                        {
                            Zone = NotificationZone.Contract,
                            Type = NotificationType.App,
                            Category = NotificationCategory.Acceptance,
                            Title = request.Code != null ? request.Code : "",
                            Content = TransactionType.UpgradeEquipments.Name,
                            Payload = JsonConvert.SerializeObject(new { Type = (int)TransactionTypeEnums.UpgradeEquipments, Id = savedRsp.Result.Id, Category = NotificationCategory.Acceptance }, new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            })
                        };
                        await _notificationGrpcService.PushNotificationByUids(notiReq, string.Join(',', userIds));
                    }
                    else
                    {
                        var projectDTO = _projectQueries.FindByOutContractId(request.OutContractIds.ElementAt(i));
                        actionResp.AddError($"Dự án {projectDTO.ProjectName} không có Supporter");
                        return actionResp;
                    }
                }
            }

            return actionResp;
        }
    }
}
