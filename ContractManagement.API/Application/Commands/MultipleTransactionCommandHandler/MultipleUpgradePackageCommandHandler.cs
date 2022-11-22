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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.MultipleTransactionCommandHandler
{
    public class MultipleUpgradePackageCommandHandler :
        BaseMultipleTransactionHandler,
        IRequestHandler<MultipleUpgradePackageCommand, ActionResponse>
    {
        private readonly IOutContractServicePackageQueries _outContractServicePackageQueries;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IServicePackageQueries _servicePackageQueries;

        public MultipleUpgradePackageCommandHandler(
            IMediator mediator,
            ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IFileRepository fileRepository,
            UserIdentity userIdentity,
            INotificationGrpcService notificationGrpcService,
            IUserGrpcService userGrpcService,
            IContractorQueries contractorQueries,
            IOutContractServicePackageQueries outContractServicePackageQueries,
            ITransactionRepository transactionRepository,
            IServicePackageQueries servicePackageQueries,
            IConfiguration hostConfiguration)
            : base(mediator, transactionQueries,
                  projectQueries,
                  attachmentFileService,
                  fileRepository,
                  userIdentity,
                  notificationGrpcService,
                  userGrpcService,
                  contractorQueries,
                  hostConfiguration)
        {
            _outContractServicePackageQueries = outContractServicePackageQueries;
            _transactionRepository = transactionRepository;
            _servicePackageQueries = servicePackageQueries;
        }

        public async Task<ActionResponse> Handle(MultipleUpgradePackageCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = this.PreHandler(request);
            if (!actionResponse.IsSuccess) return actionResponse;

            var targetChannels = _outContractServicePackageQueries.GetAllAvailableByIds(string.Join(',', request.ChannelIds));

            if (!targetChannels.Any())
            {
                return ActionResponse
                    .Failed("Không tìm thấy kênh hợp lệ trên hệ thống. Vui lòng thử lại!");
            }

            var newPackageItem = _servicePackageQueries.Find(request.PackageId);

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
                channel.EndPoint.Equipments = new List<ContractEquipmentDTO>();

                var originChannelTranCmd = new CUTransactionServicePackageCommand()
                {
                    IsOld = true,
                    IsMultipleTransaction = true,
                    TransactionType = TransactionType.ChangeServicePackage.Id
                };
                originChannelTranCmd.Binding(channel);
                transaction.AddTransServicePackage(originChannelTranCmd);

                var newChannelTranCmd = new CUTransactionServicePackageCommand()
                {
                    IsOld = false,
                    IsMultipleTransaction = true
                };

                newChannelTranCmd.Binding(channel);
                newChannelTranCmd.PackageName = newPackageItem.PackageName;
                newChannelTranCmd.PackagePrice = newPackageItem.Price;
                newChannelTranCmd.ServicePackageId = newPackageItem.Id;
                newChannelTranCmd.DomesticBandwidth = newPackageItem.DomesticBandwidth;
                newChannelTranCmd.DomesticBandwidthUom = newPackageItem.DomesticBandwidthUom;
                newChannelTranCmd.InternationalBandwidth = newPackageItem.InternationalBandwidth;
                newChannelTranCmd.InternationalBandwidthUom = newPackageItem.InternationalBandwidthUom;
                newChannelTranCmd.BandwidthLabel = newPackageItem.BandwidthLabel;

                transaction.AddTransServicePackage(newChannelTranCmd);

                var addEntityResponse = _transactionRepository.Create(transaction);
                actionResponse.CombineResponse(addEntityResponse);
                if (!actionResponse.IsSuccess) return actionResponse;

                this.SavedTransactions.Add(transaction);
            }

            await _transactionRepository.SaveChangeAsync();

            if (request.AutoConfirmation)
            {
                actionResponse.CombineResponse(await this.ApproveTransactions());
                if (!actionResponse.IsSuccess) return actionResponse;
            }
            else
            {
                await PushNotifyOrEmailToImplementers(TransactionType.ChangeServicePackage.Id, request);
            }

            return actionResponse;
        }
    }
}
