using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.MultipleTransactionCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Models.Auth;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.MultipleTransactionCommandHandler
{
    public class MultipleReclaimEquipmentCommandHandler
        : BaseMultipleTransactionHandler, IRequestHandler<MultipleReclaimEquipmentCommand, ActionResponse>
    {
        private readonly IOutContractServicePackageQueries _outContractServicePackageQueries;
        private readonly ITransactionRepository _transactionRepository;
        public MultipleReclaimEquipmentCommandHandler(
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

        public async Task<ActionResponse> Handle(MultipleReclaimEquipmentCommand request, CancellationToken cancellationToken)
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
                    ContractType = channel.ContractType ?? OutContractType.Individual.Id
                };

                if (channel.StartPoint != null)
                {
                    channel.StartPoint.Equipments = new List<ContractEquipmentDTO>();
                }

                var reclaimEquipment = channel
                    .EndPoint
                    .Equipments.Find(c => c.EquipmentId == request.EquipmentTypeId);
                if (reclaimEquipment == null || reclaimEquipment.ActivatedUnit < 1) continue;

                var transEquipmentCommand = new CUTransactionEquipmentCommand();
                transEquipmentCommand.Binding(reclaimEquipment);
                transEquipmentCommand.OutContractPackageId = channel.Id;
                transEquipmentCommand.ContractEquipmentId = reclaimEquipment.Id;
                transEquipmentCommand.WillBeReclaimUnit
                    = reclaimEquipment.ActivatedUnit > request.ReclaimUnit
                        ? request.ReclaimUnit
                        : reclaimEquipment.ActivatedUnit;

                var originChannelTranCmd = new CUTransactionServicePackageCommand()
                {
                    IsMultipleTransaction = true,
                    TransactionType = TransactionType.ReclaimEquipment.Id
                };
                originChannelTranCmd.Binding(channel);
                originChannelTranCmd.EndPoint.Equipments = new List<CUTransactionEquipmentCommand>();
                originChannelTranCmd.EndPoint.Equipments.Add(transEquipmentCommand);

                transaction.AddTransServicePackage(originChannelTranCmd);
                actionResponse.CombineResponse(_transactionRepository.Create(transaction));
                if (!actionResponse.IsSuccess) return actionResponse;
            }

            await _transactionRepository.SaveChangeAsync();
            await PushNotifyOrEmailToImplementers(TransactionType.ReclaimEquipment.Id, request);
            return actionResponse;
        }
    }
}
