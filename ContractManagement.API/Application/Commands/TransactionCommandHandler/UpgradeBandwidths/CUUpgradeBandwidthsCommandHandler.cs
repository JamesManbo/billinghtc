using AutoMapper;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure;
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

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpgradeBandwidths
{
    public class CUUpgradeBandwidthsCommandHandler : IRequestHandler<CUUpgradeBandwidthsCommand, ActionResponse<TransactionDTO>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOutContractQueries _outContractQueries;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IProjectQueries _projectQueries;

        public CUUpgradeBandwidthsCommandHandler(ITransactionRepository transactionRepository,
             IOutContractQueries outContractQueries,
             INotificationGrpcService notificationGrpcService,
             ITransactionQueries transactionQueries,
             IProjectQueries projectQueries)
        {
            _transactionRepository = transactionRepository;
            _outContractQueries = outContractQueries;
            _notificationGrpcService = notificationGrpcService;
            _transactionQueries = transactionQueries;
            _projectQueries = projectQueries;
        }
        public async Task<ActionResponse<TransactionDTO>> Handle(CUUpgradeBandwidthsCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionDTO>();
            var oldServicePackage = request.TransactionServicePackages.First(e => e.IsOld == true);

            if (request.OutContractId > 0)
            {
                var transaction = new Transaction(request);
                foreach (var transSrvPackageCommand in request.TransactionServicePackages)
                {
                    transaction.AddTransServicePackage(transSrvPackageCommand);
                }

                transaction.StatusId = TransactionStatus.WaitAcceptanced.Id;
                transaction.Type = TransactionType.UpgradeBandwidth.Id;

                if (request.IsAppendix == true)
                {
                    transaction.Code = "PL" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractId, true) + "_" + request.ContractCode;
                }
                else
                {
                    transaction.Code = "TS" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractId, false) + "_" + request.ContractCode;
                }

                transaction.OutContractId = request.OutContractId;


                actionResp.CombineResponse(await _transactionRepository.CreateAndSave(transaction));
                if (!actionResp.IsSuccess)
                {
                    throw new ContractDomainException(actionResp.Message);
                }

                if (transaction.IsTechnicalConfirmation == true)
                {
                    var userIds = _projectQueries.GetAvaliableSupporterByOutContractId(outContractId: request.OutContractId);
                    if (userIds != null && userIds.Any())
                    {
                        var notiReq = new PushNotificationRequest()
                        {
                            Zone = NotificationZone.Contract,
                            Type = NotificationType.App,
                            Category = NotificationCategory.Acceptance,
                            Title = transaction.Code != null ? transaction.Code : "",
                            Content = TransactionType.UpgradeBandwidth.Name,
                            Payload = JsonConvert.SerializeObject(new
                            {
                                Type = (int)TransactionTypeEnums.UpgradeBandwidth,
                                Id = transaction.Id,
                                Category = NotificationCategory.Acceptance
                            }, new JsonSerializerSettings
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
            }
            else if (request.OutContractIds != null)
            {
                request.OutContractIds = _outContractQueries
                .OutContractIdByIds(request.OutContractIds, oldServicePackage.ServiceId, oldServicePackage.ServicePackageId)
                .ToList();

                for (int i = 0; i < request.OutContractIds.Count(); i++)
                {
                    var transaction = new Transaction(request);
                    foreach (var transSrvPackageCommand in request.TransactionServicePackages)
                    {
                        transaction.AddTransServicePackage(transSrvPackageCommand);
                    }

                    transaction.StatusId = TransactionStatus.WaitAcceptanced.Id;
                    transaction.Type = TransactionType.UpgradeBandwidth.Id;

                    string outContractCode = _outContractQueries.OutContractCodeById(request.OutContractIds.ElementAt(i));
                    if (request.IsAppendix == true)
                    {
                        transaction.Code = "PL" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractIds.ElementAt(i), true) + "_" + outContractCode;
                    }
                    else
                    {
                        transaction.Code = "TS" + _transactionQueries.GetOrderNumberByOutContractIdAndCode(request.OutContractIds.ElementAt(i), false) + "_" + outContractCode;
                    }

                    transaction.OutContractId = request.OutContractIds.ElementAt(i);


                    actionResp.CombineResponse(await _transactionRepository.CreateAndSave(transaction));
                    if (!actionResp.IsSuccess)
                    {
                        throw new ContractDomainException(actionResp.Message);
                    }

                    if (transaction.IsTechnicalConfirmation == true)
                    {
                        var userIds = _projectQueries.GetAvaliableSupporterByOutContractId(outContractId: request.OutContractIds.ElementAt(i));
                        if (userIds != null && userIds.Any())
                        {
                            var notiReq = new PushNotificationRequest()
                            {
                                Zone = NotificationZone.Contract,
                                Type = NotificationType.App,
                                Category = NotificationCategory.Acceptance,
                                Title = transaction.Code != null ? transaction.Code : "",
                                Content = TransactionType.UpgradeBandwidth.Name,
                                Payload = JsonConvert.SerializeObject(new
                                {
                                    Type = (int)TransactionTypeEnums.UpgradeBandwidth,
                                    Id = transaction.Id,
                                    Category = NotificationCategory.Acceptance
                                }, new JsonSerializerSettings
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
            }

            return actionResp;
        }
    }
}
