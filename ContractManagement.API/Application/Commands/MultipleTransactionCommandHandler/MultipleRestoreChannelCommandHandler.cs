using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.MultipleTransactionCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Models.Auth;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.MultipleTransactionCommandHandler
{
    public class MultipleRestoreChannelCommandHandler :
        BaseMultipleTransactionHandler,
        IRequestHandler<MultipleRestoreChannelCommand, ActionResponse>
    {
        private readonly IOutContractServicePackageQueries _outContractServicePackageQueries;
        private readonly ITransactionRepository _transactionRepository;

        public MultipleRestoreChannelCommandHandler(
            IMediator mediator,
            ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IFileRepository fileRepository,
            UserIdentity userIdentity,
            INotificationGrpcService notificationGrpcService,
            IUserGrpcService userGrpcService,
            IContractorQueries contractorQueries,
            IConfiguration config,
            IOutContractServicePackageQueries outContractServicePackageQueries,
            ITransactionRepository transactionRepository) : base(mediator, transactionQueries, projectQueries, attachmentFileService, fileRepository, userIdentity, notificationGrpcService, userGrpcService, contractorQueries, config)
        {
            _outContractServicePackageQueries = outContractServicePackageQueries;
            _transactionRepository = transactionRepository;
        }

        public async Task<ActionResponse> Handle(MultipleRestoreChannelCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = this.PreHandler(request);
            if (!actionResponse.IsSuccess) return actionResponse;

            var targetChannels = _outContractServicePackageQueries.GetAllAvailableByIds(string.Join(',', request.ChannelIds));

            if (!targetChannels.Any())
            {
                return ActionResponse
                    .Failed("Không tìm thấy kênh hợp lệ trên hệ thống. Vui lòng thử lại!");
            }


            foreach (var channel in targetChannels)
            {
                var transaction = new Transaction(request)
                {
                    Code = this.GenerateTransactionCode(channel.ContractCode),
                    OutContractId = channel.OutContractId,
                    ContractorId = channel.ContractorId,
                    CurrencyUnitCode = channel.CurrencyUnitCode,
                    CurrencyUnitId = channel.CurrencyUnitId,
                    ContractCode = channel.ContractCode,
                    ContractType = channel.ContractType ?? OutContractType.Individual.Id,
                    AcceptanceStaff = UserIdentity.UserName
                };

                if (channel.StartPoint != null)
                {
                    channel.StartPoint.Equipments = new List<ContractEquipmentDTO>();
                }
                channel.EndPoint.Equipments = new List<ContractEquipmentDTO>();

                var restoreChannelCmd = new CUTransactionServicePackageCommand()
                {
                    IsOld = false,
                    IsMultipleTransaction = true,
                    TransactionType = TransactionType.RestoreServicePackage.Id
                };

                restoreChannelCmd.Binding(channel);
                if (request.AutoConfirmation)
                {
                    restoreChannelCmd.TimeLine.SuspensionEndDate = DateTime.UtcNow.AddHours(7);
                }

                transaction.AddTransServicePackage(restoreChannelCmd);

                var saveTransResponse = _transactionRepository.Create(transaction);
                actionResponse.CombineResponse(saveTransResponse);
                if (!actionResponse.IsSuccess) return actionResponse;

                this.SavedTransactions.Add(transaction);
            }

            await _transactionRepository.SaveChangeAsync();

            if (request.AutoConfirmation)
            {
                actionResponse.CombineResponse(await ApproveTransactions());
                if (!actionResponse.IsSuccess) return actionResponse;
            }
            await PushNotifyOrEmailToImplementers(TransactionType.RestoreServicePackage.Id, request);
            return actionResponse;
        }
    }
}
