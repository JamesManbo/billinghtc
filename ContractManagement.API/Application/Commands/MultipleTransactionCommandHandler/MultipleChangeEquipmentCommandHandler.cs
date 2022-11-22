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
    public class MultipleChangeEquipmentCommandHandler
        : BaseMultipleTransactionHandler, IRequestHandler<MultipleChangeEquipmentCommand, ActionResponse>
    {
        private readonly IOutContractServicePackageQueries _outContractServicePackageQueries;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        public MultipleChangeEquipmentCommandHandler(
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
            ITransactionRepository transactionRepository,
            IEquipmentTypeQueries equipmentTypeQueries) : base(mediator, transactionQueries, projectQueries, attachmentFileService, fileRepository, userIdentity, notificationGrpcService, userGrpcService, contractorQueries, config)
        {
            _outContractServicePackageQueries = outContractServicePackageQueries;
            _transactionRepository = transactionRepository;
            _equipmentTypeQueries = equipmentTypeQueries;
        }

        public async Task<ActionResponse> Handle(MultipleChangeEquipmentCommand request, CancellationToken cancellationToken)
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

                var channelOriginEquipment = channel
                    .EndPoint
                    .Equipments.Find(c => c.EquipmentId == request.ReclaimEquipmentTypeId);


                var originChannelTranCmd = new CUTransactionServicePackageCommand()
                {
                    IsMultipleTransaction = true,
                    TransactionType = TransactionType.ReclaimEquipment.Id
                };
                originChannelTranCmd.Binding(channel);

                if (channelOriginEquipment == null || channelOriginEquipment.ActivatedUnit <= 0)
                {
                    continue;
                }
                /// Khởi tạo command thu hồi thiết bị
                var reclaimEquipmentCommand = new CUTransactionEquipmentCommand();
                reclaimEquipmentCommand.Binding(channelOriginEquipment);
                reclaimEquipmentCommand.IsOld = true;
                reclaimEquipmentCommand.OutContractPackageId = channel.Id;
                reclaimEquipmentCommand.ContractEquipmentId = channelOriginEquipment.Id;
                reclaimEquipmentCommand.WillBeReclaimUnit
                    = channelOriginEquipment.ActivatedUnit > request.ReclaimUnit
                        ? request.ReclaimUnit
                        : channelOriginEquipment.ActivatedUnit;

                originChannelTranCmd.EndPoint.Equipments = new List<CUTransactionEquipmentCommand>
                    {
                        reclaimEquipmentCommand
                    };

                /// Thông tin thiết bị thay thế
                var alternativeEquipment = this._equipmentTypeQueries.Find(request.NewEquipmentTypeId);
                /// Khởi tạo command triển khai thiết bị mới
                var newEquipmentCommand = new CUTransactionEquipmentCommand
                {
                    IsOld = false,

                    OutputChannelPointId = channel.EndPoint.Id,
                    OutContractPackageId = channel.Id,
                    EquipmentId = alternativeEquipment.Id,
                    EquipmentName = alternativeEquipment.Name,
                    CurrencyUnitId = alternativeEquipment.CurrencyUnitId,
                    CurrencyUnitCode = alternativeEquipment.CurrencyUnitCode,
                    UnitPrice = alternativeEquipment.Price,
                    DeviceCode = alternativeEquipment.Code,
                    Manufacturer = alternativeEquipment.Manufacturer,
                    Specifications = alternativeEquipment.Specifications,
                    HasToReClaim = true,
                    EquipmentUom = alternativeEquipment.UnitOfMeasurement,
                    OldEquipmentId = originChannelTranCmd.Id,

                    ExaminedUnit = request.NewUnit,// Số lượng khảo sát
                    RealUnit = 0,// Số lượng triển khai thực tế
                    ReclaimedUnit = 0,// Số lượng đã thu hồi
                    SupporterHoldedUnit = 0,// Số lượng đang tạm giữ

                    ContractEquipmentId = channelOriginEquipment.Id
                };

                originChannelTranCmd.EndPoint.Equipments.Add(newEquipmentCommand);

                transaction.AddTransServicePackage(originChannelTranCmd);

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
                await PushNotifyOrEmailToImplementers(TransactionType.ChangeEquipment.Id, request);
            }
            return actionResponse;
        }
    }
}
