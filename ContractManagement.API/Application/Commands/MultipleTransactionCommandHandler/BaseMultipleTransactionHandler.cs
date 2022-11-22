using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.MultipleTransactionCommand;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Configs.SystemArgument;
using Global.Models.Auth;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;

namespace ContractManagement.API.Application.Commands.MultipleTransactionCommandHandler
{
    public class BaseMultipleTransactionHandler
    {
        protected readonly UserIdentity UserIdentity;
        protected readonly IMediator _mediator;
        protected readonly IProjectQueries _projectQueries;
        protected readonly ITransactionQueries _transactionQueries;
        protected readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        protected readonly IFileRepository _fileRepository;
        protected readonly INotificationGrpcService _notificationGrpcService;
        protected readonly IUserGrpcService _userGrpcService;
        protected readonly IConfiguration _config;
        public List<string> SupporterIds { get; set; }
        private Dictionary<string, int> ContractNumberOfTranDic;
        protected List<Transaction> SavedTransactions;

        public BaseMultipleTransactionHandler(
            IMediator mediator,
            ITransactionQueries transactionQueries,
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
            this.UserIdentity = userIdentity;
            this._notificationGrpcService = notificationGrpcService;
            this._userGrpcService = userGrpcService;
            _config = config;
            ContractNumberOfTranDic = new Dictionary<string, int>();
            SavedTransactions = new List<Transaction>();
            _mediator = mediator;
        }
        protected ActionResponse PreHandler(BaseMultipleTransactionCommand request)
        {
            var actionResp = new ActionResponse();

            request.CreatedBy = UserIdentity.UserName;
            request.CreatorUserId = UserIdentity.UniversalId;

            if (request.IsTechnicalConfirmation == true)
            {
                request.AutoConfirmation = false;
                var project = _projectQueries.Find(request.ProjectId);
                SupporterIds = _projectQueries.GetAvaliableSupporterByOutContractId(projectId: project.Id);
                if (SupporterIds == null || SupporterIds.Count == 0)
                {
                    actionResp.AddError($"Không có nhân viên kỹ thuật hỗ trợ cho dự án {project.ProjectName}, vui lòng thêm kỹ thuật cho dự án này.");
                }
            }


            return actionResp;
        }

        private string NotifyTitle(int transactionType, string projectName)
        {
            switch (transactionType)
            {
                case (int)TransactionTypeEnums.ChangeServicePackage:
                    return $"Điều chỉnh gói cước hàng loạt dự án {projectName}";

                case (int)TransactionTypeEnums.SuspendServicePackage:
                    return $"Tạm ngưng kênh truyền hàng loạt {projectName}";

                default:
                    return string.Empty;
            }
        }

        protected async Task<ActionResponse> ApproveTransactions()
        {
            var approveTransactionCommand = new CUApprovedTransactionSimplesCommand()
            {
                IsOutContract = true,
                AcceptanceStaff = UserIdentity.UserName,
                EffectiveDate = DateTime.UtcNow.AddHours(7),
                TransactionSimpleCommands = new List<CUTransactionSimpleCommand>()
            };

            foreach (var transaction in SavedTransactions)
            {
                var simpleApproveCmd = new CUTransactionSimpleCommand()
                {
                    Id = transaction.Id,
                    OutContractId = transaction.OutContractId.Value,
                    Type = transaction.Type
                };
                approveTransactionCommand.TransactionSimpleCommands.Add(simpleApproveCmd);
            }

            return await this._mediator.Send(approveTransactionCommand);
        }

        protected async Task PushNotifyOrEmailToImplementers(int transactionType, BaseMultipleTransactionCommand transaction)
        {
            var departmentCodes = _config.GetSection("DepartmentCode").Get<DepartmentCode>();
            var notifyContent = $"Vào ngày {transaction.CreatedDate:dd/MM/yyyy},\n" +
                           $"Có {transaction.ChannelIds.Length:D2} kênh thuộc dự án {transaction.ProjectName} yêu cầu được điều chỉnh gói cước.";

            if (transaction.IsTechnicalConfirmation == true)
            {
                var appNotification = new PushNotificationRequest()
                {
                    Title = NotifyTitle(transactionType, transaction.ProjectName),
                    Type = NotificationType.App,
                    Zone = NotificationZone.Contract,
                    Category = NotificationCategory.ContractTransaction,
                    Content = notifyContent,
                    Payload = JsonConvert.SerializeObject(new
                    {
                        Type = transactionType,
                        TypeName = TransactionType.GetTypeName(transactionType),
                        Category = NotificationCategory.ContractTransaction
                    }, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                };
                _ = _notificationGrpcService.PushNotificationByUids(appNotification, string.Join(',', SupporterIds));
                var supportManagers = await _userGrpcService.GetManagerByOrganization(departmentCodes.SupporterDepartmentCode);
                if (supportManagers != null && supportManagers.Count() > 0)
                {
                    var body = $"<p>Có {transaction.ChannelIds.Length:D2} kênh thuộc dự án {transaction.ProjectName}";
                    if (transactionType == (int)TransactionTypeEnums.ChangeServicePackage)
                    {
                        body += " yêu cầu điều chỉnh gói cước.</p>";
                    }
                    else if (transactionType == (int)TransactionTypeEnums.SuspendServicePackage)
                    {
                        body += " yêu cầu tạm ngưng sử dụng.</p>";
                    }
                    var emailContent = new SendMailRequest()
                    {
                        Subject = NotifyTitle(transactionType, transaction.ProjectName),
                        Body = $"<body>{body}<body/>"
                    };
                    emailContent.Emails = string.Join(",", supportManagers.Where(s => !string.IsNullOrEmpty(s.Email)).Select(s => s.Email));
                    _ = _notificationGrpcService.SendEmail(emailContent);
                }
            }
        }
        protected string GenerateTransactionCode(string contractCode)
        {
            int transactionIndex;
            if (this.ContractNumberOfTranDic.ContainsKey(contractCode))
            {
                transactionIndex = this.ContractNumberOfTranDic.GetValueOrDefault(contractCode);
                transactionIndex++;
                this.ContractNumberOfTranDic[contractCode] = transactionIndex;

            }
            else
            {
                transactionIndex = this._transactionQueries.GetOrderNumberByContractCode(contractCode, false);
                this.ContractNumberOfTranDic.Add(contractCode, transactionIndex);
            }
            return $"TS{transactionIndex:D2}_{contractCode}";
        }
    }
}
