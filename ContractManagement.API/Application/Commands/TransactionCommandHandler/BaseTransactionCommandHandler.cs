using ContractManagement.API.Application.Commands.TransactionCommandHandler.TransactionNotification;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Configs.SystemArgument;
using Global.Models.Auth;
using Global.Models.StateChangedResponse;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;
using TransactionStatus = ContractManagement.Domain.AggregatesModel.BaseContract.TransactionStatus;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler
{
    public abstract class BaseTransactionCommandHandler
    {
        protected readonly UserIdentity _userIdentity;
        protected readonly IProjectQueries _projectQueries;
        protected readonly ITransactionQueries _transactionQueries;
        protected readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        protected readonly IFileRepository _fileRepository;
        protected readonly INotificationGrpcService _notificationGrpcService;
        protected readonly IUserGrpcService _userGrpcService;
        protected readonly IContractorQueries _contractorQueries;
        protected readonly IConfiguration _config;

        public List<string> SupporterIds { get; set; }

        public BaseTransactionCommandHandler(ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IFileRepository fileRepository,
            UserIdentity userIdentity,
            INotificationGrpcService notificationGrpcService,
            IUserGrpcService userGrpcService,
            IContractorQueries contractorQueries,
            IConfiguration config)
        {
            this._transactionQueries = transactionQueries;
            this._projectQueries = projectQueries;
            this._attachmentFileService = attachmentFileService;
            this._fileRepository = fileRepository;
            this._userIdentity = userIdentity;
            this._notificationGrpcService = notificationGrpcService;
            this._userGrpcService = userGrpcService;
            this._contractorQueries = contractorQueries;
            this._config = config;
        }

        protected ActionResponse<TransactionDTO> PreHandler(CUTransactionBaseCommand request)
        {
            var actionResp = new ActionResponse<TransactionDTO>();

            if (request.Id == 0)
            {
                if (request.Type == TransactionType.RenewContract.Id)
                {
                    request.StatusId = TransactionStatus.Acceptanced.Id;
                }
                else
                {
                    request.StatusId = TransactionStatus.WaitAcceptanced.Id;
                }

                request.CreatedBy = _userIdentity.UserName;
                request.CreatorUserId = _userIdentity.UniversalId;

                if (request.TransactionServicePackages != null
                    && request.TransactionServicePackages.Count > 0)
                {
                    request.TransactionServicePackages.ForEach(t =>
                    {
                        t.IsTechnicalConfirmation = request.IsTechnicalConfirmation;
                        t.IsSupplierConfirmation = request.IsSupplierConfirmation;
                    });
                }

                if (_transactionQueries.IsTransactionCodeExistsed(request.Code))
                {
                    actionResp.AddError($"Mã {(request.IsAppendix == true ? "phụ lục" : "giao dịch")} đã tồn tại.", nameof(request.Code));
                    return actionResp;

                }
            }

            if (request.IsTechnicalConfirmation == true
                && request.ProjectId.HasValue
                && request.Type != (int)TransactionTypeEnums.RenewContract)
            {
                var project = _projectQueries.Find(request.ProjectId.Value);
                SupporterIds = _projectQueries.GetAvaliableSupporterByOutContractId(projectId: project.Id);
                if (SupporterIds == null || SupporterIds.Count == 0)
                {
                    actionResp.AddError($"Không có nhân viên kỹ thuật hỗ trợ cho dự án {project.ProjectName}, vui lòng thêm kỹ thuật cho dự án này.");
                }
            }

            if (request.TransactionServicePackages != null && request.TransactionServicePackages.Any(x => (x.StartPoint != null && x.StartPoint.Equipments.Count() > 0) ||
                   (x.EndPoint != null && x.EndPoint.Equipments.Count() > 0)))
            {
                request.HasEquipment = true;
            }
            else
            {
                request.HasEquipment = false;
            }

            return actionResp;
        }

        protected async Task AttachmentHandler(CUTransactionBaseCommand request, int transactionId)
        {
            var needToPersistentAttachments = request.AttachmentFiles
                        ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl))
                        ?.Select(c => c.TemporaryUrl)
                        ?.ToArray();
            if (needToPersistentAttachments != null && needToPersistentAttachments.Any())
            {
                var attachmentFiles =
                    await _attachmentFileService.PersistentFiles(needToPersistentAttachments);

                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new ContractDomainException("An error has occured when save the attachment files");

                foreach (var fileCommand in attachmentFiles)
                {
                    fileCommand.Name = request.AttachmentFiles
                           .Find(c => c.TemporaryUrl.Equals(fileCommand.TemporaryUrl, StringComparison.OrdinalIgnoreCase))
                           ?.Name;
                    fileCommand.Name = string.IsNullOrEmpty(fileCommand.Name) ? fileCommand.FileName : fileCommand.Name;
                    fileCommand.CreatedBy = request.CreatedBy;
                    fileCommand.TransactionId = transactionId;
                    fileCommand.OutContractId = request.OutContractId;
                    var savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                    if (!savedFileRsp.IsSuccess) throw new ContractDomainException(savedFileRsp.Message);
                }
            }
        }

        protected async Task PushNotifyOrEmailToImplementers(TransactionDTO transaction, bool isUpdated)
        {
            var departmentCodes = _config.GetSection("DepartmentCode").Get<DepartmentCode>();
            if (transaction.Contractor == null)
            {
                transaction.Contractor = _contractorQueries.FindById(transaction.ContractorId);
                transaction.ContractorFullName = transaction.Contractor?.ContractorFullName;
            }

            var notifyBuilder = new TransactionNotifyBuilder(transaction, isUpdated);
            if (transaction.IsTechnicalConfirmation == true)
            {
                var appNotification = notifyBuilder.Build<PushNotificationRequest>();
                _ = _notificationGrpcService.PushNotificationByUids(appNotification, string.Join(',', SupporterIds));
                var supportManagers = await _userGrpcService.GetManagerByOrganization(departmentCodes.SupporterDepartmentCode);
                if (supportManagers != null && supportManagers.Count() > 0)
                {
                    var emailContent = notifyBuilder.Build<SendMailRequest>();
                    emailContent.Emails = string.Join(",", supportManagers.Where(s => !string.IsNullOrEmpty(s.Email)).Select(s => s.Email));
                    _ = _notificationGrpcService.SendEmail(emailContent);
                }
            }

            if (transaction.IsSupplierConfirmation == true)
            {
                var emails = await this._userGrpcService.GetEmailsOfServiceProvider();
                var emailContent = notifyBuilder.Build<SendMailRequest>();
                emailContent.Emails = emails;
                _ = _notificationGrpcService.SendEmail(emailContent);
            }
        }
    }
}
