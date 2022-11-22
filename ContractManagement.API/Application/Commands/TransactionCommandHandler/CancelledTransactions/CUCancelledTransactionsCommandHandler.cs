using AutoMapper;
using ContractManagement.API.Application.Commands.TransactionCommandHandler.TransactionNotification;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using Global.Configs.SystemArgument;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.CancelledTransactions
{
    public class CUCancelledTransactionsCommandHandler : IRequestHandler<CUCancelledTransactionSimplesCommand, ActionResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly IMapper _mapper;
        protected readonly IProjectQueries _projectQueries;
        protected readonly IUserGrpcService _userGrpcService;
        protected readonly IConfiguration _config;
        protected readonly DepartmentCode DepartmentCode;

        public CUCancelledTransactionsCommandHandler(ITransactionRepository transactionRepository,
            IProjectQueries projectQueries,
            INotificationGrpcService notificationGrpcService,
            IUserGrpcService userGrpcService,
            IConfiguration config,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _notificationGrpcService = notificationGrpcService;
            _projectQueries = projectQueries;
            this._userGrpcService = userGrpcService;
            this._config = config;
            this.DepartmentCode = _config.GetSection("DepartmentCode").Get<DepartmentCode>();
            this._mapper = mapper;
        }

        public async Task<ActionResponse> Handle(CUCancelledTransactionSimplesCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse();
            var transactions = _transactionRepository.GetRootOnlyByIds(request.TransactionIds);
            foreach (var item in transactions)
            {
                if (item.StatusId != TransactionStatus.Acceptanced.Id)
                {
                    continue;
                }

                item.AcceptanceStaff = request.AcceptanceStaff;
                item.UpdatedDate = DateTime.Now;
                item.StatusId = TransactionStatus.Cancelled.Id;
                item.EffectiveDate = DateTime.Now;
                item.ReasonCancelAcceptance = request.ReasonContent;
                actionResp.CombineResponse(_transactionRepository.Update(item));
            }

            await _transactionRepository.SaveChangeAsync();

            if (actionResp.IsSuccess && transactions != null && transactions.Any())
            {
                foreach (Transaction tran in transactions)
                {
                    var notificationBuilder = new TransactionNotifyBuilder(_mapper.Map<TransactionDTO>(tran));
                    if (tran.IsTechnicalConfirmation == true)
                    {
                        var applicationNotify = notificationBuilder.Build<PushNotificationRequest>();
                        var SupporterIds = _projectQueries.GetAvaliableSupporterByOutContractId(projectId: tran.ProjectId.Value);
                        if (SupporterIds != null && SupporterIds.Count > 0)
                        {
                            _ = _notificationGrpcService.PushNotificationByUids(applicationNotify, string.Join(",", SupporterIds));
                        }

                        var supportManagers = await _userGrpcService.GetManagerByOrganization(DepartmentCode.SupporterDepartmentCode);
                        if (supportManagers != null && supportManagers.Count() > 0)
                        {
                            var emailContent = notificationBuilder.Build<SendMailRequest>();
                            emailContent.Emails = string.Join(",", supportManagers.Where(s => !string.IsNullOrEmpty(s.Email)).Select(s => s.Email));
                            _ = _notificationGrpcService.SendEmail(emailContent);
                        }
                    }

                    if (tran.IsSupplierConfirmation == true)
                    {
                        var emails = await this._userGrpcService.GetEmailsOfServiceProvider();
                        var emailContent = notificationBuilder.Build<SendMailRequest>();
                        emailContent.Emails = emails;
                        _ = _notificationGrpcService.SendEmail(emailContent);
                    }
                }
            }

            return actionResp;
        }
    }
}
