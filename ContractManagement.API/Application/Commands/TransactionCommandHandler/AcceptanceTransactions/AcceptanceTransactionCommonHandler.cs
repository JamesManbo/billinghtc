using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using ContractManagement.Utility;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.AcceptanceTransactions
{
    public class AcceptanceTransactionCommonHandler : IRequestHandler<AcceptanceTransactionCommandApp, ActionResponse<TransactionDTO>>
    {
        private readonly ILogger<AcceptanceTransactionCommonHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IFileRepository _fileRepository;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly IUserGrpcService _userGrpcService;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public AcceptanceTransactionCommonHandler(ITransactionRepository transactionRepository,
            IAttachmentFileResourceGrpcService attachmentFileService,
            INotificationGrpcService notificationGrpcService,
            IFileRepository fileRepository,
            ILogger<AcceptanceTransactionCommonHandler> logger,
            IUserGrpcService userGrpcService)
        {
            _transactionRepository = transactionRepository;
            _attachmentFileService = attachmentFileService;
            _fileRepository = fileRepository;
            _notificationGrpcService = notificationGrpcService;
            this._logger = logger;
            this.jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"
            };
            this._userGrpcService = userGrpcService;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(AcceptanceTransactionCommandApp request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionDTO>();
            var transactionEntity = await _transactionRepository.GetByIdAsync(request.TransactionId);
            var oldStatus = transactionEntity.StatusId;
            if (transactionEntity == null ||
                (transactionEntity.StatusId != TransactionStatus.WaitAcceptanced.Id && transactionEntity.StatusId != TransactionStatus.Cancelled.Id))
            {
                actionResp.AddError("Invalid parameter");
                return actionResp;
            }

            //if (!string.IsNullOrEmpty(transactionEntity.HandleUserId) && transactionEntity.HandleUserId!=request.AcceptanceStaffUid)
            //{
            //    actionResp.AddError("No permission");

            //    return actionResp;
            //}

            #region Add/update attachment files

            var needToPersistents = request.AttachmentFiles
                    ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl));
            if (needToPersistents != null && needToPersistents.Any())
            {
                var attachmentFiles =
                    await _attachmentFileService.PersistentFiles(needToPersistents
                        .Select(c => c.TemporaryUrl)
                        .ToArray());

                if (attachmentFiles == null || !attachmentFiles.Any())
                {
                    actionResp.AddError("Can not persistent attachment files.");
                    var attachmentFileRequest = JsonConvert.SerializeObject(request.AttachmentFiles, this.jsonSerializerSettings);
                    return actionResp;
                }

                foreach (var fileCommand in attachmentFiles)
                {
                    fileCommand.Name = request.AttachmentFiles
                           .Find(c => c.TemporaryUrl.Equals(fileCommand.TemporaryUrl, StringComparison.OrdinalIgnoreCase))
                           ?.Name;
                    fileCommand.Name = string.IsNullOrEmpty(fileCommand.Name) ? fileCommand.FileName : fileCommand.Name;
                    fileCommand.CreatedBy = request.AcceptanceStaff;
                    fileCommand.TransactionId = transactionEntity.Id;
                    fileCommand.OutContractId = transactionEntity.OutContractId;

                    var savedTranFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                    if (!savedTranFileRsp.IsSuccess)
                    {
                        actionResp.AddError("Can not save contract attachment files.");
                        break;
                    };
                }
            }

            if (!actionResp.IsSuccess)
            {
                return actionResp;
            }

            #endregion

            transactionEntity.EffectiveDate = DateTime.UtcNow.ToExactLocalDate();
            transactionEntity.StatusId = TransactionStatus.Acceptanced.Id;
            var acceptanceStaff = await _userGrpcService.GetUserByUid(request.AcceptanceStaff);
            transactionEntity.TechnicalStaffId = acceptanceStaff.IdentityGuid;
            transactionEntity.TechnicalStaff = $"{acceptanceStaff.FullName} ({acceptanceStaff.UserName})";
            transactionEntity.AcceptanceNotes = request.Note;

            foreach (var channel in transactionEntity.TransactionServicePackages)
            {
                if (transactionEntity.Type == TransactionType.DeployNewOutContract.Id ||
                    transactionEntity.Type == TransactionType.AddNewServicePackage.Id)
                {
                    channel.TimeLine.Effective = transactionEntity.EffectiveDate;
                }
                else if (transactionEntity.Type == TransactionType.ChangeServicePackage.Id)
                {
                    if (!channel.IsOld.HasValue || !channel.IsOld.Value)
                    {
                        channel.TimeLine.Effective = transactionEntity.EffectiveDate;
                    }
                    else
                    {
                        channel.TimeLine.TerminateDate = transactionEntity.EffectiveDate;
                    }
                }
                else if (transactionEntity.Type == TransactionType.TerminateServicePackage.Id ||
                  transactionEntity.Type == TransactionType.TerminateContract.Id)
                {
                    channel.TimeLine.TerminateDate = transactionEntity.EffectiveDate;
                }
                else if (transactionEntity.Type == TransactionType.RestoreServicePackage.Id)
                {
                    channel.TimeLine.SuspensionEndDate = transactionEntity.EffectiveDate;
                }
                else if (transactionEntity.Type == TransactionType.SuspendServicePackage.Id)
                {
                    channel.TimeLine.SuspensionStartDate = transactionEntity.EffectiveDate;
                }

                if (request.Equipments != null && request.Equipments.Count > 0)
                {
                    foreach (var requestEquipment in request.Equipments)
                    {
                        if (channel.StartPoint != null &&
                            channel.StartPoint.Equipments != null &&
                            channel.StartPoint.Equipments.Any(e => e.Id == requestEquipment.Id))
                        {
                            channel.StartPoint.Equipments.First(e => e.Id == requestEquipment.Id).SerialCode = requestEquipment.SerialCode;
                        }

                        if (channel.EndPoint != null &&
                            channel.EndPoint.Equipments != null &&
                            channel.EndPoint.Equipments.Any(e => e.Id == requestEquipment.Id))
                        {
                            channel.EndPoint.Equipments.First(e => e.Id == requestEquipment.Id).SerialCode = requestEquipment.SerialCode;
                        }
                    }
                }
            }

            var savedRsp = await _transactionRepository.UpdateAndSave(transactionEntity);
            actionResp.CombineResponse(savedRsp);

            if (!actionResp.IsSuccess)
            {
                return actionResp;
            }

            await _notificationGrpcService.PushNotificationByUids(new PushNotificationRequest
            {
                Zone = NotificationZone.Contract,
                Type = NotificationType.Private,
                Category = NotificationCategory.ContractTransaction,
                Title = $"Phụ lục {transactionEntity.Code} chuyển trạng thái sang {TransactionStatus.Acceptanced.Name}",
                Content = $"Phụ lục {transactionEntity.Code} chuyển trạng thái từ {TransactionStatus.From(oldStatus).Name}" +
                    $" sang {TransactionStatus.Acceptanced.Name}.",
                Payload = JsonConvert.SerializeObject(new
                {
                    Id = transactionEntity.Id,
                    Category = NotificationCategory.ContractTransaction,
                    Type = transactionEntity.Type,
                    TypeName = TransactionType.GetTypeName(transactionEntity.Type),
                    Code = transactionEntity.Code,
                    RedirectPage = "transaction-management"
                }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
            }, transactionEntity.CreatorUserId);

            return actionResp;
        }
    }
}
