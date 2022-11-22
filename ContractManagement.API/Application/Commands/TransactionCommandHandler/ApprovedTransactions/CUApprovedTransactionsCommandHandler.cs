using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Events.ContractEvents;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;
using ContractManagement.Domain.Models;
using ContractManagement.API.Application.Services.Radius;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.Events;
using AutoMapper;
using System.Collections.Generic;
using ContractManagement.Infrastructure.Repositories.InContractRepository;
using Microsoft.Extensions.Configuration;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.Domain.Models.Notification;
using Global.Models.Notification;
using Newtonsoft.Json;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Newtonsoft.Json.Serialization;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Utilities;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ApprovedTransactions
{
    public class CUApprovedTransactionsCommandHandler : ApprovedTransactionsBaseHandler,
        IRequestHandler<CUApprovedTransactionSimplesCommand, ActionResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IOutContractRepository _outContractRepository;
        private readonly IInContractRepository _inContractRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IRadiusAndBrasManagementService _radiusAndBrasManagementService;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IConfiguration _configuration;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly IServicePackageSuspensionTimeRepository _channelSuspensionRepository;

        public CUApprovedTransactionsCommandHandler(ITransactionRepository transactionRepository,
            IFileRepository fileRepository,
            IOutContractRepository contractRepository,
            IWrappedConfigAndMapper configAndMapper,
            IRadiusAndBrasManagementService radiusAndBrasManagementService,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IMapper mapper,
            IConfiguration configuration,
            INotificationGrpcService notificationGrpcService,
            ITransactionQueries transactionQueries,
            IMediator mediator,
            IInContractRepository inContractRepository,
            IServicePackageSuspensionTimeRepository channelSuspensionRepository)
        {
            this._transactionRepository = transactionRepository;
            this._fileRepository = fileRepository;
            this._outContractRepository = contractRepository;
            this._configAndMapper = configAndMapper;
            this._radiusAndBrasManagementService = radiusAndBrasManagementService;
            this._attachmentFileService = attachmentFileService;
            this._mapper = mapper;
            this._configuration = configuration;
            this._notificationGrpcService = notificationGrpcService;
            this._transactionQueries = transactionQueries;
            this._mediator = mediator;
            this._inContractRepository = inContractRepository;
            this._channelSuspensionRepository = channelSuspensionRepository;
        }

        private async Task AttachmentFileHandler(List<CreateUpdateFileCommand> attachmentFiles,
            int transactionId,
            string acceptanceUser,
            int? outContractId = null,
            int? inContractId = null)
        {
            var needToPersistents = attachmentFiles
                    ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl))
                    ?.Select(c => c.TemporaryUrl)
                    ?.ToArray();

            if (needToPersistents != null && needToPersistents.Any())
            {
                var persistedAttachmentFiles =
                    await _attachmentFileService.PersistentFiles(needToPersistents);

                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new ContractDomainException("An error has occured when save the attachment files");

                foreach (var fileCommand in persistedAttachmentFiles)
                {
                    fileCommand.Name = attachmentFiles
                           .Find(c => c.TemporaryUrl.Equals(fileCommand.TemporaryUrl, StringComparison.OrdinalIgnoreCase))
                           ?.Name;
                    fileCommand.Name = string.IsNullOrEmpty(fileCommand.Name) ? fileCommand.FileName : fileCommand.Name;
                    fileCommand.TransactionId = transactionId;
                    fileCommand.OutContractId = outContractId;
                    fileCommand.InContractId = inContractId;
                    fileCommand.CreatedBy = acceptanceUser;

                    var savedTranFileRsp = _fileRepository.Create(fileCommand);
                    if (!savedTranFileRsp.IsSuccess) throw new ContractManagementException(savedTranFileRsp.Message);
                }

                await _fileRepository.SaveChangeAsync();
            }
        }

        public async Task<ActionResponse> Handle(CUApprovedTransactionSimplesCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse();
            if (request.TransactionSimpleCommands.Count >= 1)
            {
                #region Single Transaction
                int[] arrTransactionIds = request
                    .TransactionSimpleCommands
                    .Select(c => c.Id)
                    .ToArray();

                switch (request.TransactionSimpleCommands[0].Type)
                {
                    case (int)TransactionTypeEnums.AddNewServicePackage:
                        actionResp.CombineResponse(await AddNewServicePackage(arrTransactionIds, request));
                        break;

                    case (int)TransactionTypeEnums.ChangeServicePackage:
                        actionResp.CombineResponse(await ChangeServicePackage(arrTransactionIds, request));
                        break;

                    case (int)TransactionTypeEnums.SuspendServicePackage:
                        actionResp.CombineResponse(await SuspendService(arrTransactionIds, request));
                        break;

                    case (int)TransactionTypeEnums.TerminateServicePackage:
                        actionResp.CombineResponse(await TerminateService(arrTransactionIds, request));
                        break;

                    case (int)TransactionTypeEnums.TerminateContract:
                        actionResp.CombineResponse(await TerminateContract(arrTransactionIds, request));
                        break;

                    case (int)TransactionTypeEnums.ChangeLocation:

                        actionResp.CombineResponse(await ChangeLocationServicePackages(arrTransactionIds, actionResp, request));
                        break;

                    case (int)TransactionTypeEnums.ChangeEquipment:

                        actionResp.CombineResponse(await ChangeEquipments(arrTransactionIds, actionResp, request));
                        break;

                    case (int)TransactionTypeEnums.ReclaimEquipment:

                        actionResp.CombineResponse(await ReclaimEquipments(arrTransactionIds, actionResp, request));
                        break;

                    case (int)TransactionTypeEnums.UpgradeEquipments:

                        if (!_transactionRepository
                            .UpgradeEquipments(arrTransactionIds, (int)TransactionTypeEnums.UpgradeEquipments, request.AcceptanceStaff))
                        {
                            actionResp.AddError("Đã xảy ra lỗi khi thực hiện thay đổi ");
                        }
                        break;

                    case (int)TransactionTypeEnums.UpgradeBandwidth:
                        actionResp.CombineResponse(await UpgradeBandwidth(arrTransactionIds, actionResp, request));
                        break;

                    case (int)TransactionTypeEnums.RestoreServicePackage:
                        actionResp.CombineResponse(await RestoreService(arrTransactionIds, request));
                        break;

                    case (int)TransactionTypeEnums.DeployNewOutContract:
                        actionResp.CombineResponse(await UpdateDeployNewOutContract(arrTransactionIds, actionResp, request));
                        break;

                    case (int)TransactionTypeEnums.RenewContract:
                        actionResp.CombineResponse(await RenewContract(arrTransactionIds, actionResp, request));
                        break;
                }
                #endregion
            }
            return actionResp;
        }

        async Task<ActionResponse> AddNewServicePackage(int[] arrTransactionId, CUApprovedTransactionSimplesCommand request)
        {
            var actionResp = new ActionResponse();
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            foreach (var item in transactions)
            {
                if (item.OutContractId > 0)
                {
                    var contractEntity = await _outContractRepository.GetByIdAsync(item.OutContractId);

                    var oldChannelIds = contractEntity.ActiveServicePackages.Select(c => c.Id).ToArray();
                    // Thêm gói cước vào hợp đồng
                    foreach (var transChannel
                        in item.TransactionServicePackages.Where(t => t.OutContractId == item.OutContractId))
                    {
                        transChannel.CreatedBy = request.AcceptanceStaff;
                        if (transChannel.TimeLine?.Effective == null)
                        {
                            transChannel.TimeLine.Effective = DateTime.UtcNow.AddHours(7);
                        }

                        if (request.StartBillingDate.HasValue)
                        {
                            transChannel.TimeLine.StartBilling = request.StartBillingDate?.ToExactLocalDate();
                        }
                        transChannel.StatusId = OutContractServicePackageStatus.Developed.Id;
                        transChannel.TransactionServicePackageId = transChannel.Id;

                        if (transChannel.HasStartAndEndPoint == true)
                        {
                            foreach (var equipment in transChannel.StartPoint.Equipments)
                            {
                                equipment.SetRealUnits(equipment.RealUnit > 0 ? equipment.RealUnit : equipment.ExaminedUnit);
                            }
                        }

                        foreach (var equipment in transChannel.EndPoint.Equipments)
                        {
                            equipment.SetRealUnits(equipment.RealUnit > 0 ? equipment.RealUnit : equipment.ExaminedUnit);
                        }

                        var createNewChannelCmd = this._mapper.Map<CUOutContractChannelCommand>(transChannel);
                        var addedChannel = contractEntity.AddServicePackage(createNewChannelCmd);
                        addedChannel.OutContractId = contractEntity.Id;
                    }

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    contractEntity.CalculateTotal();
                    var savedContractEntityRsp = await _outContractRepository.UpdateAndSave(contractEntity);
                    actionResp.CombineResponse(savedContractEntityRsp);
                    if (!actionResp.IsSuccess)
                    {
                        return actionResp;
                    }

                    foreach (var newChannel in contractEntity.ActiveServicePackages
                        .Where(c => !oldChannelIds.Contains(c.Id)))
                    {
                        /// Gửi sự kiện thêm mới kênh
                        /// xử lý các tác vụ: thêm mới phiếu thu lần đầu cho kênh, cập nhật thời gian kỳ cước tiếp theo
                        if (newChannel.TimeLine.PrepayPeriod > 0 &&
                            newChannel.FlexiblePricingTypeId == FlexiblePricingType.FixedPricing.Id)
                        {
                            var addednewServicePackageEvent = new AddedNewServicePackageDomainEvent(contractEntity, newChannel);
                            actionResp.CombineResponse(await _mediator.Send(addednewServicePackageEvent));
                        }
                    }

                    #region Add/update attachment files
                    await AttachmentFileHandler(request.AttachmentFiles, item.Id, item.AcceptanceStaff, item.OutContractId);
                    #endregion
                }
                else
                {
                    var contractEntity = await _inContractRepository.GetByIdAsync(item.InContractId);
                    // Thêm gói cước vào hợp đồng
                    foreach (var transChannel
                        in item.TransactionServicePackages.Where(t => t.InContractId == item.InContractId))
                    {
                        transChannel.CreatedBy = request.AcceptanceStaff;
                        if (transChannel.TimeLine?.Effective == null)
                        {
                            transChannel.TimeLine.Effective = DateTime.UtcNow.AddHours(7);
                        }

                        if (request.StartBillingDate.HasValue)
                        {
                            transChannel.TimeLine.StartBilling = request.StartBillingDate?.ToExactLocalDate();
                        }

                        transChannel.StatusId = OutContractServicePackageStatus.Developed.Id;
                        transChannel.TransactionServicePackageId = transChannel.Id;
                        if (transChannel.HasStartAndEndPoint == true)
                        {
                            foreach (var equipment in transChannel.StartPoint.Equipments)
                            {
                                equipment.SetRealUnits(equipment.RealUnit > 0 ? equipment.RealUnit : equipment.ExaminedUnit);
                            }
                        }

                        foreach (var equipment in transChannel.EndPoint.Equipments)
                        {
                            equipment.SetRealUnits(equipment.RealUnit > 0 ? equipment.RealUnit : equipment.ExaminedUnit);
                        }

                        var createContractServicePackageCmd = this._mapper.Map<CUOutContractChannelCommand>(transChannel);
                        var addedServicePackage = contractEntity.AddServicePackage(createContractServicePackageCmd);
                        addedServicePackage.InContractId = contractEntity.Id;
                    }

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    contractEntity.CalculateTotal();

                    var savedContractEntityRsp = await _inContractRepository.UpdateAndSave(contractEntity);
                    actionResp.CombineResponse(savedContractEntityRsp);
                    if (!actionResp.IsSuccess)
                    {
                        return actionResp;
                    }

                    #region Add/update attachment files

                    await AttachmentFileHandler(request.AttachmentFiles, item.Id, item.AcceptanceStaff, inContractId: item.InContractId);

                    #endregion
                }

                item.EffectiveDate = request.EffectiveDate;
                item.AcceptanceStaff = request.AcceptanceStaff;
                item.StatusId = TransactionStatus.AcceptanceApproved.Id;

                actionResp.CombineResponse(await _transactionRepository.UpdateAndSave(item));

                // Gửi notification đến nv trong kho và nv kỹ thuật đã triển khai
                if (actionResp.IsSuccess)
                {
                    await this.SendNotificationAfterApprove(item.Type, item.Id, item.Code, item.TechnicalStaffId, item.IsTechnicalConfirmation, item.HasEquipment, item.CreatorUserId);
                }
            }
            return actionResp;

        }

        async Task<ActionResponse> ChangeServicePackage(int[] arrTransactionId, CUApprovedTransactionSimplesCommand request)
        {
            var actionResponse = new ActionResponse();
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            var transactionDTOs = new List<TransactionDTO>();
            foreach (var transaction in transactions)
            {
                if (transaction.OutContractId.HasValue &&
                    transaction.OutContractId.Value > 0)
                {
                    var currenctOutContract = await _outContractRepository.GetByIdAsync(transaction.OutContractId);

                    /// Kênh ban đầu
                    var originalChannel = transaction.TransactionServicePackages
                        .First(o => o.IsOld.HasValue && o.IsOld.Value);
                    /// Kênh thay thế
                    var replacementChannel = transaction.TransactionServicePackages
                        .First(o => o.IsOld != true);

                    /// Khởi tạo 1 command thực hiện cập nhật thông tin điều chỉnh từ giao dịch/phụ lục
                    /// vào kênh đang hoạt động của hợp đồng
                    /// Lưu ý: Thông tin điều chỉnh sẽ chỉ bao gồm các thiết bị mới(không bao gồm các thiết bị ban đầu của kênh)
                    var updatingChannelCmd = this._mapper.Map<CUOutContractChannelCommand>(replacementChannel);

                    /// Gán lại khóa chính, khóa phụ của các đối tượng phụ thuộc để thực hiện update cho đúng.
                    /// Chuyển số lượng dự kiến của thiết bị thành số lượng đã triển khai sau khi đã duyệt giao dịch/phụ lục này
                    updatingChannelCmd.Id = replacementChannel.OutContractServicePackageId;
                    if (updatingChannelCmd.HasStartAndEndPoint)
                    {
                        updatingChannelCmd.StartPointChannelId = replacementChannel.StartPoint.ContractPointId ?? 0;
                        foreach (var equipment in updatingChannelCmd.StartPoint.Equipments)
                        {
                            equipment.OutputChannelPointId = updatingChannelCmd.StartPoint.Id;
                            equipment.OutContractPackageId = updatingChannelCmd.Id;
                            equipment.RealUnit = equipment.RealUnit > 0
                                ? equipment.RealUnit
                                : equipment.ExaminedUnit;
                        }
                    }

                    updatingChannelCmd.EndPointChannelId = replacementChannel.EndPoint.ContractPointId ?? 0;
                    foreach (var equipment in updatingChannelCmd.EndPoint.Equipments)
                    {
                        equipment.OutputChannelPointId = updatingChannelCmd.EndPoint.Id;
                        equipment.OutContractPackageId = updatingChannelCmd.Id;
                        equipment.RealUnit = equipment.RealUnit > 0
                            ? equipment.RealUnit
                            : equipment.ExaminedUnit;
                    }

                    /// Giữ lại thiết bị ban đầu của kênh nếu không thu hồi
                    /// và cập nhật số lượng thiết bị mới hoặc thu hồi (nếu có)
                    if (updatingChannelCmd.HasStartAndEndPoint)
                    {
                        foreach (var originEquipment in originalChannel.StartPoint.Equipments)
                        {
                            /// Nếu trong ds thiết bị thêm mới, có thiết bị trùng với thiết bị ban đầu của kênh,
                            /// sẽ lấy số lượng của thiết bị mới, cộng với số lượng của thiết bị ban đầu
                            if (updatingChannelCmd.StartPoint.Equipments
                                .Any(e => e.EquipmentId == originEquipment.EquipmentId))
                            {
                                var newEquipmentCmd =
                                    updatingChannelCmd.StartPoint.Equipments.First(e => e.EquipmentId == originEquipment.EquipmentId);
                                newEquipmentCmd.Id = originEquipment.ContractEquipmentId ?? 0;
                                if (!string.IsNullOrEmpty(originEquipment.SerialCode))
                                {
                                    newEquipmentCmd.SerialCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.SerialCode;
                                }
                                if (!string.IsNullOrEmpty(originEquipment.MacAddressCode))
                                {
                                    newEquipmentCmd.MacAddressCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.MacAddressCode;
                                }
                                // Cập nhật số lượng dự kiến thành đã triển khai(ưu tiên lấy nếu số lượng triển khai được nhập từ người dùng)
                                // sau khi duyệt giao dịch/phụ lục
                                newEquipmentCmd.RealUnit = newEquipmentCmd.RealUnit > 0 ? newEquipmentCmd.RealUnit : newEquipmentCmd.ExaminedUnit;

                                newEquipmentCmd.ExaminedUnit += originEquipment.ExaminedUnit;
                                newEquipmentCmd.RealUnit += originEquipment.RealUnit;
                                newEquipmentCmd.ReclaimedUnit += originEquipment.ReclaimedUnit;
                                // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                                if (originEquipment.WillBeReclaimUnit > 0)
                                {
                                    newEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                                }
                            }
                            /// Nếu không thì thực hiện cập nhật thiết bị ban đầu hoặc ghi nhận số lượng thu hồi(nếu có)
                            else
                            {
                                var reclaimEquipmentCmd = new CUContractEquipmentCommand(originEquipment)
                                {
                                    Id = originEquipment.ContractEquipmentId ?? 0
                                };
                                // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                                if (originEquipment.WillBeReclaimUnit > 0)
                                {
                                    reclaimEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                                }
                                updatingChannelCmd.StartPoint.Equipments.Add(reclaimEquipmentCmd);
                            }
                        }
                    }

                    foreach (var originEquipment in originalChannel.EndPoint.Equipments)
                    {
                        /// Nếu trong ds thiết bị thêm mới, có thiết bị trùng với thiết bị cũ của kênh.
                        /// Thì sẽ lấy thông tin của thiết bị cũ, cộng dồn với thiết bị mới được thêm vào
                        if (updatingChannelCmd.EndPoint.Equipments
                            .Any(e => e.EquipmentId == originEquipment.EquipmentId))
                        {
                            var newEquipmentCmd =
                                updatingChannelCmd.EndPoint.Equipments.First(e => e.EquipmentId == originEquipment.EquipmentId);
                            newEquipmentCmd.Id = originEquipment.ContractEquipmentId ?? 0;
                            if (!string.IsNullOrEmpty(originEquipment.SerialCode))
                            {
                                newEquipmentCmd.SerialCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.SerialCode;
                            }
                            if (!string.IsNullOrEmpty(originEquipment.MacAddressCode))
                            {
                                newEquipmentCmd.MacAddressCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.MacAddressCode;
                            }

                            // Cập nhật số lượng dự kiến thành đã triển khai(ưu tiên lấy nếu số lượng triển khai được nhập từ người dùng)
                            // sau khi duyệt giao dịch/phụ lục
                            newEquipmentCmd.RealUnit = newEquipmentCmd.RealUnit > 0 ? newEquipmentCmd.RealUnit : newEquipmentCmd.ExaminedUnit;
                            newEquipmentCmd.ExaminedUnit += originEquipment.ExaminedUnit;
                            newEquipmentCmd.RealUnit += originEquipment.RealUnit;
                            newEquipmentCmd.ReclaimedUnit += originEquipment.ReclaimedUnit;
                            // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                            if (originEquipment.WillBeReclaimUnit > 0)
                            {
                                newEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                            }
                        }
                        /// Nếu không thì thực hiện ghi nhận số lượng thu hồi(nếu có)
                        else
                        {
                            var reclaimEquipmentCmd = new CUContractEquipmentCommand(originEquipment)
                            {
                                Id = originEquipment.ContractEquipmentId ?? 0
                            };
                            // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                            if (originEquipment.WillBeReclaimUnit > 0)
                            {
                                reclaimEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                            }
                            updatingChannelCmd.EndPoint.Equipments.Add(reclaimEquipmentCmd);
                        }
                    }

                    if (updatingChannelCmd.ServiceLevelAgreements != null &&
                        updatingChannelCmd.ServiceLevelAgreements.Count > 0)
                    {
                        updatingChannelCmd.ServiceLevelAgreements
                            .ForEach(c => c.OutContractServicePackageId = updatingChannelCmd.Id);
                    }

                    if (updatingChannelCmd.OutContractServicePackageTaxes != null &&
                        updatingChannelCmd.OutContractServicePackageTaxes.Count > 0)
                    {
                        updatingChannelCmd.OutContractServicePackageTaxes
                            .ForEach(c => c.OutContractServicePackageId = updatingChannelCmd.Id);
                    }

                    if (updatingChannelCmd.PriceBusTables != null &&
                        updatingChannelCmd.PriceBusTables.Count > 0)
                    {
                        updatingChannelCmd.PriceBusTables
                            .ForEach(c => c.ChannelId = updatingChannelCmd.Id);
                    }

                    /// Ko cho phép xóa thiết bị nếu ko có trong danh sách được cập nhật
                    updatingChannelCmd.PreventRemoveEquipmentIfNotUpdate = true;
                    currenctOutContract.UpdateServicePackage(updatingChannelCmd, forceBind: true);

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    currenctOutContract.CalculateTotal();

                    var savedContractEntityRsp = await _outContractRepository.UpdateAndSave(currenctOutContract);
                    actionResponse.CombineResponse(savedContractEntityRsp);

                    if (!actionResponse.IsSuccess)
                    {
                        throw new ContractDomainException(actionResponse.Message);
                    }

                    #region Add/update attachment files

                    await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId);

                    #endregion

                    //Radius handler

                    if (!string.IsNullOrWhiteSpace(updatingChannelCmd.RadiusAccount) &&
                        updatingChannelCmd.ServicePackageId.HasValue)
                    {
                        var deactiveResponse = await _radiusAndBrasManagementService
                            .DeactivateUserByUserName(updatingChannelCmd.RadiusAccount);

                        var upgradeResponse = await _radiusAndBrasManagementService
                            .MultipleUpgradeSrvByUserName(new string[] { updatingChannelCmd.RadiusAccount }, updatingChannelCmd.ServicePackageId.Value);

                        if (!upgradeResponse.IsSuccess)
                        {
                            return upgradeResponse;
                        }
                    }

                    #region receipt voucher fee
                    if (transaction.ChangingPackageFee > 0)
                    {
                        transactionDTOs.Add(MapTransactionDTO(transaction));
                    }
                    #endregion
                }
                else
                {
                    var inContractEntity = await _inContractRepository.GetByIdAsync(transaction.InContractId);
                    /// Kênh ban đầu
                    var originalChannel = transaction.TransactionServicePackages
                        .First(o => o.IsOld.HasValue && o.IsOld.Value);
                    /// Kênh thay thế
                    var replacementChannel = transaction.TransactionServicePackages
                        .First(o => o.IsOld != true);

                    /// Khởi tạo 1 command thực hiện cập nhật thông tin điều chỉnh từ giao dịch/phụ lục
                    /// vào kênh đang hoạt động của hợp đồng
                    /// Lưu ý: Thông tin điều chỉnh sẽ chỉ bao gồm các thiết bị mới(không bao gồm các thiết bị ban đầu của kênh)
                    var updatingChannelCmd = this._mapper.Map<CUOutContractChannelCommand>(replacementChannel);

                    /// Gán lại khóa chính, khóa phụ của các đối tượng phụ thuộc để thực hiện update cho đúng.
                    /// Chuyển số lượng dự kiến của thiết bị thành số lượng đã triển khai sau khi đã duyệt giao dịch/phụ lục này
                    updatingChannelCmd.Id = replacementChannel.OutContractServicePackageId;
                    if (updatingChannelCmd.HasStartAndEndPoint)
                    {
                        updatingChannelCmd.StartPointChannelId = replacementChannel.StartPoint.ContractPointId ?? 0;
                        foreach (var equipment in updatingChannelCmd.StartPoint.Equipments)
                        {
                            equipment.OutputChannelPointId = updatingChannelCmd.StartPoint.Id;
                            equipment.OutContractPackageId = updatingChannelCmd.Id;
                            equipment.RealUnit = equipment.RealUnit > 0
                                ? equipment.RealUnit
                                : equipment.ExaminedUnit;
                        }
                    }

                    updatingChannelCmd.EndPointChannelId = replacementChannel.EndPoint.ContractPointId ?? 0;
                    foreach (var equipment in updatingChannelCmd.EndPoint.Equipments)
                    {
                        equipment.OutputChannelPointId = updatingChannelCmd.EndPoint.Id;
                        equipment.OutContractPackageId = updatingChannelCmd.Id;
                        equipment.RealUnit = equipment.RealUnit > 0
                            ? equipment.RealUnit
                            : equipment.ExaminedUnit;
                    }

                    /// Giữ lại thiết bị ban đầu của kênh nếu không thu hồi
                    /// và cập nhật số lượng thiết bị mới hoặc thu hồi (nếu có)
                    if (updatingChannelCmd.HasStartAndEndPoint)
                    {
                        foreach (var originEquipment in originalChannel.StartPoint.Equipments)
                        {
                            /// Nếu trong ds thiết bị thêm mới, có thiết bị trùng với thiết bị ban đầu của kênh,
                            /// sẽ lấy số lượng của thiết bị mới, cộng với số lượng của thiết bị ban đầu
                            if (updatingChannelCmd.StartPoint.Equipments
                                .Any(e => e.EquipmentId == originEquipment.EquipmentId))
                            {
                                var newEquipmentCmd =
                                    updatingChannelCmd.StartPoint.Equipments.First(e => e.EquipmentId == originEquipment.EquipmentId);
                                newEquipmentCmd.Id = originEquipment.ContractEquipmentId ?? 0;
                                if (!string.IsNullOrEmpty(originEquipment.SerialCode))
                                {
                                    newEquipmentCmd.SerialCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.SerialCode;
                                }
                                if (!string.IsNullOrEmpty(originEquipment.MacAddressCode))
                                {
                                    newEquipmentCmd.MacAddressCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.MacAddressCode;
                                }
                                // Cập nhật số lượng dự kiến thành đã triển khai(ưu tiên lấy nếu số lượng triển khai được nhập từ người dùng)
                                // sau khi duyệt giao dịch/phụ lục
                                newEquipmentCmd.RealUnit = newEquipmentCmd.RealUnit > 0 ? newEquipmentCmd.RealUnit : newEquipmentCmd.ExaminedUnit;

                                newEquipmentCmd.ExaminedUnit += originEquipment.ExaminedUnit;
                                newEquipmentCmd.RealUnit += originEquipment.RealUnit;
                                newEquipmentCmd.ReclaimedUnit += originEquipment.ReclaimedUnit;
                                // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                                if (originEquipment.WillBeReclaimUnit > 0)
                                {
                                    newEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                                }
                            }
                            /// Nếu không thì thực hiện cập nhật thiết bị ban đầu hoặc ghi nhận số lượng thu hồi(nếu có)
                            else
                            {
                                var reclaimEquipmentCmd = new CUContractEquipmentCommand(originEquipment)
                                {
                                    Id = originEquipment.ContractEquipmentId ?? 0
                                };
                                // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                                if (originEquipment.WillBeReclaimUnit > 0)
                                {
                                    reclaimEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                                }
                                updatingChannelCmd.StartPoint.Equipments.Add(reclaimEquipmentCmd);
                            }
                        }
                    }

                    foreach (var originEquipment in originalChannel.EndPoint.Equipments)
                    {
                        /// Nếu trong ds thiết bị thêm mới, có thiết bị trùng với thiết bị cũ của kênh.
                        /// Thì sẽ lấy thông tin của thiết bị cũ, cộng dồn với thiết bị mới được thêm vào
                        if (updatingChannelCmd.EndPoint.Equipments
                            .Any(e => e.EquipmentId == originEquipment.EquipmentId))
                        {
                            var newEquipmentCmd =
                                updatingChannelCmd.EndPoint.Equipments.First(e => e.EquipmentId == originEquipment.EquipmentId);
                            newEquipmentCmd.Id = originEquipment.ContractEquipmentId ?? 0;
                            if (!string.IsNullOrEmpty(originEquipment.SerialCode))
                            {
                                newEquipmentCmd.SerialCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.SerialCode;
                            }
                            if (!string.IsNullOrEmpty(originEquipment.MacAddressCode))
                            {
                                newEquipmentCmd.MacAddressCode += (string.IsNullOrEmpty(newEquipmentCmd.SerialCode) ? "" : ",") + originEquipment.MacAddressCode;
                            }

                            // Cập nhật số lượng dự kiến thành đã triển khai(ưu tiên lấy nếu số lượng triển khai được nhập từ người dùng)
                            // sau khi duyệt giao dịch/phụ lục
                            newEquipmentCmd.RealUnit = newEquipmentCmd.RealUnit > 0 ? newEquipmentCmd.RealUnit : newEquipmentCmd.ExaminedUnit;
                            newEquipmentCmd.ExaminedUnit += originEquipment.ExaminedUnit;
                            newEquipmentCmd.RealUnit += originEquipment.RealUnit;
                            newEquipmentCmd.ReclaimedUnit += originEquipment.ReclaimedUnit;
                            // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                            if (originEquipment.WillBeReclaimUnit > 0)
                            {
                                newEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                            }
                        }
                        /// Nếu không thì thực hiện ghi nhận số lượng thu hồi(nếu có)
                        else
                        {
                            var reclaimEquipmentCmd = new CUContractEquipmentCommand(originEquipment)
                            {
                                Id = originEquipment.ContractEquipmentId ?? 0
                            };
                            // Cộng thêm số lượng thu hồi của giao dịch/phụ lục
                            if (originEquipment.WillBeReclaimUnit > 0)
                            {
                                reclaimEquipmentCmd.ReclaimedUnit += originEquipment.WillBeReclaimUnit;
                            }
                            updatingChannelCmd.EndPoint.Equipments.Add(reclaimEquipmentCmd);
                        }
                    }

                    if (updatingChannelCmd.ServiceLevelAgreements != null &&
                        updatingChannelCmd.ServiceLevelAgreements.Count > 0)
                    {
                        updatingChannelCmd.ServiceLevelAgreements
                            .ForEach(c => c.OutContractServicePackageId = updatingChannelCmd.Id);
                    }

                    if (updatingChannelCmd.PriceBusTables != null &&
                        updatingChannelCmd.PriceBusTables.Count > 0)
                    {
                        updatingChannelCmd.PriceBusTables
                            .ForEach(c => c.ChannelId = updatingChannelCmd.Id);
                    }

                    inContractEntity.UpdateServicePackage(updatingChannelCmd, forceBind: true);

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    inContractEntity.CalculateTotal();

                    var savedContractEntityRsp = await _inContractRepository.UpdateAndSave(inContractEntity);
                    actionResponse.CombineResponse(savedContractEntityRsp);
                    if (!actionResponse.IsSuccess)
                    {
                        throw new ContractDomainException(actionResponse.Message);
                    }

                    #region Add/update attachment files

                    await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, inContractId: transaction.InContractId);

                    #endregion
                }

                if (transactionDTOs.Any())
                {
                    await CreateReceiptVoucherFee(transactionDTOs, actionResponse);
                }

                transaction.EffectiveDate = request.EffectiveDate;
                transaction.AcceptanceStaff = request.AcceptanceStaff;
                transaction.StatusId = TransactionStatus.AcceptanceApproved.Id;
                transaction.UpdatedDate = DateTime.Now;
                actionResponse.CombineResponse(_transactionRepository.Update(transaction));

                if (!transaction.IsMultipleTransaction)
                {
                    //gửi notification
                    await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
                }
            }

            await _transactionRepository.SaveChangeAsync();

            return actionResponse;
        }

        async Task<ActionResponse> SuspendService(int[] arrTransactionIds, CUApprovedTransactionSimplesCommand request)
        {
            var actionResponse = new ActionResponse();
            var transactionDTOs = new List<TransactionDTO>();
            if (_transactionRepository
                     .RestoreOrSuspendServices(arrTransactionIds, (int)TransactionTypeEnums.SuspendServicePackage, request.AcceptanceStaff))
            {
                var transactions = this._transactionQueries.FindByIds(arrTransactionIds);
                string[] radiusAccounts = transactions.SelectMany(t => t.TransactionServicePackages)
                    .Where(e => !string.IsNullOrWhiteSpace(e.RadiusAccount))
                    .Select(e => e.RadiusAccount).ToArray();

                if (radiusAccounts.Any())
                {
                    actionResponse.CombineResponse(await _radiusAndBrasManagementService.MultipleDeactivateUserByUserName(radiusAccounts));
                }

                if (!actionResponse.IsSuccess)
                {
                    return actionResponse;
                }

                if (transactions.Count() == 1)
                {
                    #region receipt voucher fee
                    foreach (var tran in transactions)
                    {
                        #region Add/update attachment files
                        await AttachmentFileHandler(request.AttachmentFiles, tran.Id, tran.AcceptanceStaff, tran.OutContractId, tran.InContractId);
                        #endregion

                        if (tran.SuspendHandleFee > 0)
                        {
                            transactionDTOs.Add(tran);
                            //gửi notification
                            await this.SendNotificationAfterApprove(tran.Type, tran.Id, tran.Code, tran.TechnicalStaffId, tran.IsTechnicalConfirmation, false, tran.CreatorUserId);
                        }
                    }
                    #endregion

                    if (transactionDTOs.Any())
                    {
                        await CreateReceiptVoucherFee(transactionDTOs, actionResponse);
                    }
                }
            }
            else { actionResponse.AddError("Đã có lỗi xảy ra khi thực hiện tạm ngưng dịch vụ, vui lòng thử lại sau"); }

            return actionResponse;
        }

        async Task<ActionResponse> RestoreService(int[] arrTransactionIds, CUApprovedTransactionSimplesCommand request)
        {
            var actionResponse = new ActionResponse();
            var transactionDTOs = new List<TransactionDTO>();
            if (_transactionRepository.RestoreOrSuspendServices(arrTransactionIds, (int)TransactionTypeEnums.RestoreServicePackage, request.AcceptanceStaff))
            {
                var transactions = this._transactionQueries.FindByIds(arrTransactionIds);
                var channelIds = transactions
                    .SelectMany(c => c.TransactionServicePackages)
                    .Select(t => t.OutContractServicePackageId)
                    .ToArray();

                var channelSuspensions = await this._channelSuspensionRepository.GetByChannelIdsAsync(channelIds);
                if (channelSuspensions.Count > 0)
                {
                    // Thời điểm khôi phục
                    var now = DateTime.UtcNow.AddHours(7);
                    foreach (var channelSuspension in channelSuspensions)
                    {
                        var suspendedDay = channelSuspension.SuspensionStartDate;
                        var srvPackagePrice = channelSuspension.Channel.PackagePrice;

                        channelSuspension.DiscountAmountBeforeTax = 0;
                        // Tính giá trị giảm trừ theo tháng
                        // Mỗi lần lặp là 1 tháng, bắt đầu từ ngày tạm ngưng đến thời điểm khôi phục
                        while (suspendedDay.Year < now.Year
                            || (suspendedDay.Year == now.Year && suspendedDay.Month <= now.Month))
                        {
                            // Số ngày trong tháng đang tính giảm trừ
                            var daysOfMonth = DateTime.DaysInMonth(suspendedDay.Year, suspendedDay.Month);
                            // Tìm ngày cuối cùng trong tháng đang tính giảm trừ hoặc ngày khôi phục
                            var lastDayInMonth = new DateTime(suspendedDay.Year, suspendedDay.Month, daysOfMonth);
                            if (now < lastDayInMonth)
                            {
                                lastDayInMonth = now;
                            }

                            // Số tiền giảm trừ theo ngày (chưa bao gồm thuế)
                            var discountAmountPerDay = srvPackagePrice / daysOfMonth;
                            // Số ngày tạm ngưng trong tháng
                            var suspendedDays = lastDayInMonth.Subtract(suspendedDay).Days + 1;

                            channelSuspension.DaysSuspended += suspendedDays;
                            channelSuspension.DiscountAmountBeforeTax += (discountAmountPerDay * suspendedDays)
                                .RoundByCurrency(channelSuspension.Channel.CurrencyUnitCode);

                            suspendedDay = suspendedDay.AddMonths(1);
                            suspendedDay = new DateTime(suspendedDay.Year, suspendedDay.Month, 1);
                        }

                        // Tính giá trị thuế
                        channelSuspension.TaxAmount = (channelSuspension.DiscountAmountBeforeTax
                            * (decimal)channelSuspension.Channel.TaxPercent / 100)
                            .RoundByCurrency(channelSuspension.Channel.CurrencyUnitCode);

                        // Tính giá trị sau thuế
                        channelSuspension.DiscountAmount = channelSuspension.DiscountAmountBeforeTax + channelSuspension.TaxAmount;
                        channelSuspension.RemainingAmountBeforeTax = channelSuspension.DiscountAmountBeforeTax;
                        channelSuspension.RemainingAmount = channelSuspension.DiscountAmount;
                        channelSuspension.SuspensionEndDate = now;
                        channelSuspension.Description = $"Giảm trừ tạm ngưng kênh {channelSuspension.Channel.CId}" +
                            $" {channelSuspension.DaysSuspended:D2} ngày" +
                            $" từ ngày {channelSuspension.SuspensionStartDate:dd/MM/yyyy} đến ngày {channelSuspension.SuspensionEndDate.Value:dd/MM/yyyy}";
                        if (channelSuspension.Channel.CurrencyUnitCode == CurrencyUnit.VND.CurrencyUnitCode)
                        {
                            channelSuspension.Description += $"({channelSuspension.DiscountAmount.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-vn"))})";
                        }
                        else
                        {
                            channelSuspension.Description += $"({channelSuspension.DiscountAmount.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("en-us"))})";
                        }
                    }
                }

                string[] radiusAccounts = transactions.SelectMany(t => t.TransactionServicePackages)
                    .Where(e => !string.IsNullOrWhiteSpace(e.RadiusAccount))
                    .Select(e => e.RadiusAccount)
                    .ToArray();

                if (radiusAccounts.Any())
                {
                    var radiusHandleResponse = await _radiusAndBrasManagementService.MultipleActivateUserByUserName(radiusAccounts);
                    if (!radiusHandleResponse.IsSuccess)
                    {
                        return radiusHandleResponse;
                    }
                }

                if (transactions.Count() == 1)
                {
                    #region Attachment, notification & receipt voucher fee handler
                    foreach (var tran in transactions)
                    {
                        #region Add/update attachment files                
                        await AttachmentFileHandler(request.AttachmentFiles, tran.Id, tran.AcceptanceStaff, tran.OutContractId, tran.InContractId);
                        #endregion

                        if (tran.RestoreHandleFee.HasValue && tran.RestoreHandleFee.Value > 0)
                        {
                            transactionDTOs.Add(tran);
                        }

                        //gửi notification
                        await this.SendNotificationAfterApprove(tran.Type, tran.Id, tran.Code, tran.TechnicalStaffId, tran.IsTechnicalConfirmation, tran.HasEquipment, tran.CreatorUserId);

                    }
                    if (transactionDTOs.Any())
                    {
                        await CreateReceiptVoucherFee(transactionDTOs, actionResponse);
                    }
                    #endregion
                }
            }
            else { actionResponse.AddError("Đã có lỗi xảy ra khi thực hiện khôi phục dịch vụ, vui lòng thử lại sau"); }
            return actionResponse;
        }

        async Task<ActionResponse> TerminateService(int[] arrTransactionIds, CUApprovedTransactionSimplesCommand request)
        {
            var actionResp = new ActionResponse();
            if (_transactionRepository.TerminateServices(arrTransactionIds, (int)TransactionTypeEnums.TerminateServicePackage, request.AcceptanceStaff))
            {
                var transactions = _transactionRepository.GetByIds(arrTransactionIds);
                foreach (var transaction in transactions)
                {
                    if (transaction.OutContractId.HasValue && transaction.OutContractId > 0)
                    {
                        var outContract = await _outContractRepository.GetByIdAsync(transaction.OutContractId);
                        outContract.CalculateTotal();

                        if (outContract.ActiveServicePackages.Count == 0
                            && outContract.ContractStatusId != ContractStatus.Liquidated.Id)
                        {
                            outContract.SetTerminatedStatus();
                            outContract.TimeLine.Liquidation = DateTime.UtcNow.AddHours(7);
                        }

                        actionResp.CombineResponse(_outContractRepository.Update(outContract));
                        if (!actionResp.IsSuccess)
                        {
                            throw new ContractDomainException(actionResp.Message);
                        }

                        // Domain Event
                        var ocspIds = outContract.ServicePackages
                            .Where(e => e.StatusId == OutContractServicePackageStatus.Terminate.Id)
                            .Select(e => e.Id).ToList();

                        var terminateServicePackagesDomainEvent = new TerminateServicePackagesDomainEvent()
                        {
                            OutContractServicePackageIds = ocspIds
                        };

                        outContract.AddDomainEvent(terminateServicePackagesDomainEvent);

                        // Radius
                        string[] radiusAccounts = outContract.ServicePackages
                                .Where(e => e.StatusId == OutContractServicePackageStatus.Terminate.Id
                                    && !string.IsNullOrWhiteSpace(e.RadiusAccount))
                                .Select(e => e.RadiusAccount).ToArray();

                        if (radiusAccounts.Any())
                        {
                            var radiusHandleResponse = await _radiusAndBrasManagementService.MultipleTerminateUserByUserName(radiusAccounts);
                            if (!radiusHandleResponse.IsSuccess)
                            {
                                return radiusHandleResponse;
                            }
                        }
                        await _outContractRepository.SaveChangeAsync();
                    }
                    else
                    {
                        var inContract = await _inContractRepository.GetByIdAsync(transaction.InContractId);
                        inContract.CalculateTotal();

                        if (inContract.ActiveServicePackages.Count == 0
                            && inContract.ContractStatusId != ContractStatus.Liquidated.Id)
                        {
                            inContract.SetTerminatedStatus();
                        }

                        actionResp.CombineResponse(_inContractRepository.Update(inContract));
                        if (!actionResp.IsSuccess)
                        {
                            throw new ContractDomainException(actionResp.Message);
                        }
                    }

                    if (transactions.Count() == 1)
                    {
                        #region Add/update attachment files                
                        await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId, transaction.InContractId);
                        #endregion

                        //gửi notification
                        await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
                    }
                }
            }
            else { actionResp.AddError("Đã có lỗi xảy ra khi thực hiện hủy dịch vụ, vui lòng thử lại sau."); }

            return actionResp;
        }

        async Task<ActionResponse> TerminateContract(int[] arrTransactionIds, CUApprovedTransactionSimplesCommand request)
        {
            var actionResp = new ActionResponse();
            if (_transactionRepository
                .TerminateContracts(arrTransactionIds, (int)TransactionTypeEnums.TerminateContract, request.AcceptanceStaff, request.EffectiveDate, request.IsOutContract))
            {
                if (request.IsOutContract)
                {
                    // Radius & Radius Handler
                    IEnumerable<(string RadiusAccount, string RadiusPassword)> radiusAccounts =
                       _transactionQueries.GetRadiusAccountOfTransactions(arrTransactionIds);

                    if (radiusAccounts.Any())
                    {
                        var radiusUserNames = radiusAccounts.Select(c => c.RadiusAccount).ToArray();
                        var radiusHandleResponse = await _radiusAndBrasManagementService.MultipleTerminateUserByUserName(radiusUserNames);
                        if (!radiusHandleResponse.IsSuccess)
                        {
                            return radiusHandleResponse;
                        }
                    }

                    //gửi notification
                    var transactions = this._transactionQueries.FindByIds(arrTransactionIds);
                    foreach (var transaction in transactions)
                    {
                        #region Add/update attachment files                
                        await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId, transaction.InContractId);
                        #endregion
                        await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
                    }
                }
            }
            else { actionResp.AddError("Đã có lỗi xảy ra khi thực hiện thanh lý hợp đồng. Vui lòng thử lại sau."); }

            return actionResp;
        }

        async Task<ActionResponse> ChangeLocationServicePackages(int[] arrTransactionId, ActionResponse actionResponse, CUApprovedTransactionSimplesCommand request)
        {
            var transactionDTOs = new List<TransactionDTO>();
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            foreach (var transaction in transactions)
            {
                if (transaction.OutContractId.HasValue && transaction.OutContractId > 0)
                {
                    var contractEntity = await _outContractRepository.GetByIdAsync(transaction.OutContractId);
                    //Cập nhật gói cước nếu đã tồn tại trong hợp đồng
                    foreach (var transactionChannel in transaction.TransactionServicePackages.Where(s => s.IsOld != true))
                    {
                        var contractChannel = contractEntity.ServicePackages
                            .First(e => e.Id == transactionChannel.OutContractServicePackageId);
                        contractChannel.UpdatedBy = request.AcceptanceStaff;
                        contractChannel.UpdatedDate = DateTime.Now;

                        if (contractChannel.HasStartAndEndPoint)
                        {
                            contractChannel.StartPoint.UpdateInstallationAddress(transactionChannel.StartPoint.InstallationAddress);
                        }

                        contractChannel.EndPoint.UpdateInstallationAddress(transactionChannel.EndPoint.InstallationAddress);
                    }

                    var savedContractEntityRsp = await _outContractRepository.UpdateAndSave(contractEntity);
                    actionResponse.CombineResponse(savedContractEntityRsp);
                    if (!actionResponse.IsSuccess)
                    {
                        throw new ContractDomainException(actionResponse.Message);
                    }
                }
                else
                {
                    var contractEntity = await _inContractRepository.GetByIdAsync(transaction.InContractId);
                    //Cập nhật gói cước nếu đã tồn tại trong hợp đồng
                    foreach (var transactionChannel in transaction.TransactionServicePackages.Where(s => s.IsOld != true))
                    {
                        var contractChannel = contractEntity.ServicePackages
                            .First(e => e.Id == transactionChannel.OutContractServicePackageId);
                        contractChannel.UpdatedBy = request.AcceptanceStaff;
                        contractChannel.UpdatedDate = DateTime.Now;

                        if (contractChannel.HasStartAndEndPoint)
                        {
                            contractChannel.StartPoint.UpdateInstallationAddress(transactionChannel.StartPoint.InstallationAddress);
                        }

                        contractChannel.EndPoint.UpdateInstallationAddress(transactionChannel.EndPoint.InstallationAddress);
                    }

                    var savedContractEntityRsp = await _inContractRepository.UpdateAndSave(contractEntity);
                    actionResponse.CombineResponse(savedContractEntityRsp);
                    if (!actionResponse.IsSuccess)
                    {
                        throw new ContractDomainException(actionResponse.Message);
                    }
                }

                #region Add/update attachment files                
                await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId, transaction.InContractId);
                #endregion

                #region receipt voucher fee

                if (transaction.ChaningLocationFee.HasValue && transaction.ChaningLocationFee.Value > 0)
                {
                    transactionDTOs.Add(MapTransactionDTO(transaction));
                }

                #endregion

                transaction.EffectiveDate = request.EffectiveDate;
                transaction.AcceptanceStaff = request.AcceptanceStaff;
                transaction.StatusId = TransactionStatus.AcceptanceApproved.Id;
                actionResponse.CombineResponse(await _transactionRepository.UpdateAndSave(transaction));

                if (!actionResponse.IsSuccess)
                {
                    throw new ContractDomainException(actionResponse.Message);
                }

                //gửi notification
                await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
            }

            if (transactionDTOs.Any())
            {
                await CreateReceiptVoucherFee(transactionDTOs, actionResponse);
            }
            return actionResponse;
        }

        async Task<ActionResponse> ChangeEquipments(int[] arrTransactionId, ActionResponse actionResponse, CUApprovedTransactionSimplesCommand request)
        {
            var transactionDTOs = new List<TransactionDTO>();
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            foreach (var transaction in transactions)
            {
                if (transaction.OutContractId.HasValue
                    && transaction.OutContractId > 0)
                {
                    var contractEntity = await _outContractRepository.GetByIdAsync(transaction.OutContractId);

                    foreach (var transChannel in transaction.TransactionServicePackages)
                    {
                        var contractChannel = contractEntity.ServicePackages
                            .First(c => c.Id == transChannel.OutContractServicePackageId);
                        if (contractChannel.HasStartAndEndPoint)
                        {
                            foreach (var transactionEquipment in transChannel.StartPoint.Equipments)
                            {
                                /// Thiết bị đc đánh dấu là IsOld, là những thiết bị ban đầu của kênh.
                                /// Những thiết bị này có thể thực hiện hành động thu hồi
                                if (transactionEquipment.IsOld == true)
                                {
                                    var contractEquipment = contractChannel.StartPoint.Equipments
                                        .FirstOrDefault(e => e.Id == transactionEquipment.ContractEquipmentId);
                                    if (contractEquipment == null) continue;
                                    if (transactionEquipment.WillBeReclaimUnit > 0)
                                    {
                                        var additionRsp = contractEquipment
                                            .AddReclaimedUnits(transactionEquipment.WillBeReclaimUnit);

                                        if (!additionRsp.IsSuccess) throw new ContractDomainException(additionRsp.Message);
                                    }
                                }
                                /// Những thiết bị đc đánh dấu là !IsOld, là những thiết bị mới
                                else
                                {
                                    var addNewEquipmentCmd = new CUContractEquipmentCommand(transactionEquipment)
                                    {
                                        TransactionEquipmentId = transactionEquipment.Id
                                    };
                                    /// Nếu người dùng có nhập số lượng thực tế triển khai
                                    /// thì sẽ lấy giá trị đó, nếu không thì lấy giá trị dự kiến làm giá trị thực tế khi
                                    /// phụ lục/giao dịch đc duyệt
                                    addNewEquipmentCmd.RealUnit = addNewEquipmentCmd.RealUnit > 0
                                        ? addNewEquipmentCmd.RealUnit
                                        : addNewEquipmentCmd.ExaminedUnit;
                                    contractChannel.StartPoint.AddOrUpdateEquipment(addNewEquipmentCmd);
                                    transactionEquipment.SetRealUnits(addNewEquipmentCmd.RealUnit);
                                }
                            }
                        }

                        foreach (var transactionEquipment in transChannel.EndPoint.Equipments)
                        {
                            if (transactionEquipment.IsOld == true)
                            {
                                var contractEquipment = contractChannel.EndPoint.Equipments
                                        .FirstOrDefault(e => e.Id == transactionEquipment.ContractEquipmentId);
                                if (contractEquipment == null) continue;
                                if (transactionEquipment.WillBeReclaimUnit > 0)
                                {
                                    var addReclaimUnitResp = contractEquipment
                                        .AddReclaimedUnits(transactionEquipment.WillBeReclaimUnit);

                                    if (!addReclaimUnitResp.IsSuccess)
                                        throw new ContractDomainException(addReclaimUnitResp.Message);
                                }
                            }
                            else
                            {
                                var addNewEquipmentCmd = new CUContractEquipmentCommand(transactionEquipment)
                                {
                                    TransactionEquipmentId = transactionEquipment.Id
                                };
                                addNewEquipmentCmd.RealUnit = addNewEquipmentCmd.RealUnit > 0
                                    ? addNewEquipmentCmd.RealUnit
                                    : addNewEquipmentCmd.ExaminedUnit;
                                contractChannel.EndPoint.AddOrUpdateEquipment(addNewEquipmentCmd);
                                transactionEquipment.SetRealUnits(addNewEquipmentCmd.RealUnit);
                            }
                        }
                        contractChannel.CalculateTotal();
                    }

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    contractEntity.CalculateTotal();

                    var savedContractEntityRsp = await _outContractRepository.UpdateAndSave(contractEntity);
                    actionResponse.CombineResponse(savedContractEntityRsp);
                    if (!actionResponse.IsSuccess)
                    {
                        throw new ContractDomainException(actionResponse.Message);
                    }
                    if (transaction.ChangeEquipmentFee.HasValue && transaction.ChangeEquipmentFee.Value > 0)
                    {
                        transactionDTOs.Add(MapTransactionDTO(transaction));
                    }
                }
                else
                {
                    var inContractEntity = await _inContractRepository.GetByIdAsync(transaction.InContractId);
                    foreach (var transChannel in transaction.TransactionServicePackages)
                    {
                        var contractChannel = inContractEntity.ServicePackages.First(c => c.Id == transChannel.OutContractServicePackageId);
                        if (contractChannel.HasStartAndEndPoint)
                        {
                            foreach (var transactionEquipment in transChannel.StartPoint.Equipments)
                            {
                                if (transactionEquipment.IsOld.HasValue && transactionEquipment.IsOld.Value)
                                {
                                    var contractEquipment = contractChannel.StartPoint.Equipments
                                        .FirstOrDefault(e => e.Id == transactionEquipment.ContractEquipmentId);

                                    if (contractEquipment == null) continue;

                                    if (transactionEquipment.WillBeReclaimUnit > 0)
                                    {
                                        var additionRsp = contractEquipment
                                            .AddReclaimedUnits(transactionEquipment.WillBeReclaimUnit);

                                        if (!additionRsp.IsSuccess)
                                            throw new ContractDomainException(additionRsp.Message);
                                    }
                                }
                                else
                                {
                                    var addNewEquipmentCmd = new CUContractEquipmentCommand(transactionEquipment)
                                    {
                                        TransactionEquipmentId = transactionEquipment.Id
                                    };
                                    addNewEquipmentCmd.RealUnit = addNewEquipmentCmd.RealUnit > 0
                                        ? addNewEquipmentCmd.RealUnit
                                        : addNewEquipmentCmd.ExaminedUnit;
                                    contractChannel.StartPoint.AddOrUpdateEquipment(addNewEquipmentCmd);
                                    transactionEquipment.SetRealUnits(addNewEquipmentCmd.RealUnit);
                                }
                            }
                        }

                        foreach (var transactionEquipment in transChannel.EndPoint.Equipments)
                        {
                            if (transactionEquipment.IsOld.HasValue && transactionEquipment.IsOld.Value)
                            {
                                var contractEquipment = contractChannel.EndPoint.Equipments
                                    .FirstOrDefault(e => e.Id == transactionEquipment.ContractEquipmentId);

                                if (contractEquipment == null) continue;

                                var additionRsp = contractEquipment.AddReclaimedUnits(transactionEquipment.WillBeReclaimUnit);
                                if (!additionRsp.IsSuccess) throw new ContractDomainException(additionRsp.Message);
                            }
                            else
                            {
                                var addNewEquipmentCmd = new CUContractEquipmentCommand(transactionEquipment)
                                {
                                    TransactionEquipmentId = transactionEquipment.Id
                                };

                                addNewEquipmentCmd.RealUnit = addNewEquipmentCmd.RealUnit > 0
                                    ? addNewEquipmentCmd.RealUnit
                                    : addNewEquipmentCmd.ExaminedUnit;
                                contractChannel.EndPoint.AddOrUpdateEquipment(addNewEquipmentCmd);
                                transactionEquipment.SetRealUnits(addNewEquipmentCmd.RealUnit);
                            }
                        }
                        contractChannel.CalculateTotal();
                    }

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    inContractEntity.CalculateTotal();

                    var savedContractEntityRsp = await _inContractRepository.UpdateAndSave(inContractEntity);
                    actionResponse.CombineResponse(savedContractEntityRsp);
                    if (!actionResponse.IsSuccess)
                    {
                        throw new ContractDomainException(actionResponse.Message);
                    }
                }

                #region Add/update attachment files                
                await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId, transaction.InContractId);
                #endregion

                #region receipt voucher fee
                if (transactionDTOs.Any())
                {
                    await CreateReceiptVoucherFee(transactionDTOs, actionResponse);
                }

                #endregion

                transaction.EffectiveDate = request.EffectiveDate;
                transaction.AcceptanceStaff = request.AcceptanceStaff;
                transaction.StatusId = TransactionStatus.AcceptanceApproved.Id;
                var savedRsp = await _transactionRepository.UpdateAndSave(transaction);
                actionResponse.CombineResponse(savedRsp);

                if (!actionResponse.IsSuccess)
                {
                    throw new ContractDomainException(actionResponse.Message);
                }

                //gửi notification
                await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
            }
            return actionResponse;
        }
        async Task<ActionResponse> ReclaimEquipments(int[] arrTransactionId, ActionResponse actionResp, CUApprovedTransactionSimplesCommand request)
        {
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            foreach (var transaction in transactions)
            {
                if (transaction.OutContractId.HasValue &&
                    transaction.OutContractId > 0)
                {
                    var contractEntity = await _outContractRepository.GetByIdAsync(transaction.OutContractId);

                    foreach (var transactionChannel in transaction.TransactionServicePackages)
                    {
                        var contractChannel = contractEntity
                            .ServicePackages.First(s => s.Id == transactionChannel.OutContractServicePackageId);
                        if (contractChannel.HasStartAndEndPoint)
                        {
                            foreach (var equipment in contractChannel.StartPoint.Equipments
                                .Where(e => transactionChannel.StartPoint.Equipments.Any(te => te.ContractEquipmentId == e.Id)))
                            {
                                var transEquipment = transactionChannel.StartPoint.Equipments
                                    .First(te => te.ContractEquipmentId == equipment.Id);

                                var additionResp = equipment.AddReclaimedUnits(transEquipment.WillBeReclaimUnit);
                                if (!additionResp.IsSuccess) throw new ContractDomainException(additionResp.Message);

                                equipment.UpdatedBy = request.AcceptanceStaff;
                                equipment.UpdatedDate = DateTime.Now;
                                equipment.CalculateTotal();
                                contractChannel.StartPoint.CalculateTotal();
                            }
                        }

                        foreach (var equipment in contractChannel.EndPoint.Equipments
                                .Where(e => transactionChannel.EndPoint.Equipments.Any(te => te.ContractEquipmentId == e.Id)))
                        {
                            var transEquipment = transactionChannel.EndPoint.Equipments
                                .First(te => te.ContractEquipmentId == equipment.Id);

                            var additionResp = equipment.AddReclaimedUnits(transEquipment.WillBeReclaimUnit);
                            if (!additionResp.IsSuccess) throw new ContractDomainException(additionResp.Message);

                            equipment.UpdatedBy = request.AcceptanceStaff;
                            equipment.UpdatedDate = DateTime.Now;
                            equipment.CalculateTotal();
                            contractChannel.EndPoint.CalculateTotal();
                        }
                        contractChannel.CalculateTotal();
                    }
                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    contractEntity.CalculateTotal();

                    actionResp.CombineResponse(_outContractRepository.Update(contractEntity));
                    if (!actionResp.IsSuccess)
                    {
                        throw new ContractDomainException(actionResp.Message);
                    }
                }
                else
                {
                    var inContractEntity = await _inContractRepository.GetByIdAsync(transaction.InContractId);

                    foreach (var transactionChannel in transaction.TransactionServicePackages)
                    {
                        var contractChannel = inContractEntity
                            .ServicePackages.First(s => s.Id == transactionChannel.OutContractServicePackageId);
                        if (contractChannel.HasStartAndEndPoint)
                        {
                            foreach (var equipment in contractChannel.StartPoint.Equipments
                                .Where(e => transactionChannel.StartPoint.Equipments.Any(te => te.ContractEquipmentId == e.Id)))
                            {
                                var transEquipment = transactionChannel.EndPoint.Equipments
                                   .First(te => te.ContractEquipmentId == equipment.Id);

                                var additionResp = equipment.AddReclaimedUnits(transEquipment.WillBeReclaimUnit);
                                if (!additionResp.IsSuccess) throw new ContractDomainException(additionResp.Message);

                                equipment.UpdatedBy = request.AcceptanceStaff;
                                equipment.UpdatedDate = DateTime.Now;
                                equipment.CalculateTotal();
                                contractChannel.StartPoint.CalculateTotal();
                            }
                        }

                        foreach (var equipment in contractChannel.EndPoint.Equipments
                                .Where(e => transactionChannel.EndPoint.Equipments.Any(te => te.ContractEquipmentId == e.Id)))
                        {
                            var transEquipment = transactionChannel.EndPoint.Equipments
                                .First(te => te.ContractEquipmentId == equipment.Id);

                            var additionResp = equipment.AddReclaimedUnits(transEquipment.WillBeReclaimUnit);
                            if (!additionResp.IsSuccess) throw new ContractDomainException(additionResp.Message);

                            equipment.UpdatedBy = request.AcceptanceStaff;
                            equipment.UpdatedDate = DateTime.Now;
                            equipment.CalculateTotal();
                            contractChannel.EndPoint.CalculateTotal();
                        }
                        contractChannel.CalculateTotal();
                    }
                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    inContractEntity.CalculateTotal();

                    actionResp.CombineResponse(_inContractRepository.Update(inContractEntity));
                    if (!actionResp.IsSuccess)
                    {
                        throw new ContractDomainException(actionResp.Message);
                    }
                }

                #region Add/update attachment files                
                await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId, transaction.InContractId);
                #endregion

                transaction.EffectiveDate = request.EffectiveDate;
                transaction.AcceptanceStaff = request.AcceptanceStaff;
                transaction.StatusId = TransactionStatus.AcceptanceApproved.Id;
                actionResp.CombineResponse(_transactionRepository.Update(transaction));

                if (!actionResp.IsSuccess)
                {
                    throw new ContractDomainException(actionResp.Message);
                }

                //gửi notification
                await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
            }
            await _transactionRepository.SaveChangeAsync();
            return actionResp;
        }

        async Task<ActionResponse> UpgradeBandwidth(int[] arrTransactionId, ActionResponse actionResp, CUApprovedTransactionSimplesCommand request)
        {
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            foreach (var item in transactions)
            {
                if (item.Type != (int)TransactionTypeEnums.UpgradeBandwidth
                    || item.StatusId != TransactionStatus.WaitAcceptanced.Id)
                {
                    continue;
                }

                if (item.IsTechnicalConfirmation != true)
                {
                    #region Add/update attachment files

                    var needToPersistentAttachments = request.AttachmentFiles
                            ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl))
                            ?.Select(c => c.TemporaryUrl)
                            ?.ToArray();

                    if (needToPersistentAttachments != null && needToPersistentAttachments.Length > 0)
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
                            fileCommand.TransactionId = item.Id;
                            fileCommand.OutContractId = item.OutContractId;
                            fileCommand.CreatedBy = request.AcceptanceStaff;

                            var savedTranFileRsp = _fileRepository.Create(fileCommand);
                            if (!savedTranFileRsp.IsSuccess) throw new ContractManagementException(savedTranFileRsp.Message);
                        }

                        await _fileRepository.SaveChangeAsync();
                    }

                    #endregion
                    var outContractEntity = await _outContractRepository.GetByIdAsync(item.OutContractId);

                    //Domain event
                    var upgradeSrvPackageEvent = new UpgradeServicePackageDomainEvent
                    {
                        Transaction = item.MapTo<TransactionDTO>(_configAndMapper.MapperConfig)
                    };

                    //Dịch vụ mới
                    var newTransactionServicePackage = item.TransactionServicePackages.FirstOrDefault(o => o.IsOld != true);
                    //Dịch vụ cũ
                    var oldTransactionServicePackage = item.TransactionServicePackages.FirstOrDefault(o => o.IsOld == true);
                    var contractPackageEntities = outContractEntity.ServicePackages.Where(e => e.ServiceId == oldTransactionServicePackage.ServiceId
                        && e.ServicePackageId == oldTransactionServicePackage.ServicePackageId && e.StatusId == OutContractServicePackageStatus.Developed.Id);

                    if (contractPackageEntities == null)
                    {
                        continue;
                    }

                    foreach (var contractPackageEntity in contractPackageEntities)
                    {
                        var updatePackageCommand = contractPackageEntity.MapTo<CUOutContractChannelCommand>(_configAndMapper.MapperConfig);
                        if (updatePackageCommand.ServicePackageId.HasValue)
                        {
                            updatePackageCommand.StatusId = OutContractServicePackageStatus.UpgradeBandwidths.Id;
                            updatePackageCommand.UpdatedBy = request.AcceptanceStaff;
                            updatePackageCommand.UpdatedDate = DateTime.Now;
                        }
                        var oldOutContractSrvPackage = contractPackageEntity.Update(updatePackageCommand);


                        // Thêm gói cước vào hợp đồng
                        var addPackageCommand = contractPackageEntity.MapTo<CUOutContractChannelCommand>(_configAndMapper.MapperConfig);

                        //TODO Outlet channel point logic changes
                        //addPackageCommand.InstallationAddress = (InstallationAddress)contractPackageEntity.InstallationAddress.GetCopy();
                        addPackageCommand.CreatedBy = request.AcceptanceStaff;
                        //TODO Outlet channel point logic changes
                        // Thêm danh sách thiết bị vào hợp đồng
                        //if (newTransactionServicePackage.TransactionEquipments != null)
                        //{
                        //    for (int j = 0;
                        //        j < newTransactionServicePackage.TransactionEquipments.Count();
                        //        j++)
                        //    {
                        //        var equipmentCommand = newTransactionServicePackage.TransactionEquipments
                        //            .ElementAt(j).MapTo<CUContractEquipmentCommand>(_configAndMapper.MapperConfig);
                        //        equipmentCommand.Id = 0;

                        //        // Lấy thông tin thiết bị
                        //        var eqInfo = _equipmentTypeQueries.Find(equipmentCommand.EquipmentId);
                        //        MapEquipmentCommand(ref equipmentCommand, packageInfo, eqInfo);
                        //        equipmentCommand.CreatedBy = outContractEntity.CreatedBy;
                        //        equipmentCommand.CreatedDate = DateTime.Now;
                        //        //TODO Outlet channel point logic changes
                        //        //addPackageCommand.EquipmentCommands.Add(equipmentCommand);
                        //    }
                        //}

                        // Thêm gói cước vào hợp đồng
                        addPackageCommand.StatusId = OutContractServicePackageStatus.Developed.Id;
                        var newOutContractSrvPackage = outContractEntity.AddServicePackage(addPackageCommand);
                        var newOutContractSrvPackageDomainEvent = newOutContractSrvPackage.MapTo<OutContractServicePackageDTO>(_configAndMapper.MapperConfig);
                        newOutContractSrvPackageDomainEvent.OldId = contractPackageEntity.Id;
                        newOutContractSrvPackageDomainEvent.IsOldOCSPCheaper = newOutContractSrvPackage.PackagePrice > contractPackageEntity.PackagePrice;
                        upgradeSrvPackageEvent.NewOutContractServicePackages.Add(newOutContractSrvPackageDomainEvent);
                    }

                    // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                    outContractEntity.CalculateTotal();

                    var savedContractEntityRsp = await _outContractRepository.UpdateAndSave(outContractEntity);
                    actionResp.CombineResponse(savedContractEntityRsp);
                    if (!actionResp.IsSuccess)
                    {
                        throw new ContractDomainException(actionResp.Message);
                    }

                    //Add domain event
                    //foreach (var nOCSP in upgradeSrvPackageEvent.NewOutContractServicePackages)
                    //{
                    //    var ocspEntity = savedContractEntityRsp.Result.ServicePackages.FirstOrDefault(e => e.OldId == nOCSP.OldId);
                    //    if (ocspEntity != null)
                    //    {
                    //        nOCSP.Id = ocspEntity.Id;
                    //    }
                    //}

                    upgradeSrvPackageEvent.Transaction = item.MapTo<TransactionDTO>(_configAndMapper.MapperConfig); ;
                    upgradeSrvPackageEvent.OutContract = outContractEntity.MapTo<OutContractDTO>(_configAndMapper.MapperConfig);
                    if (upgradeSrvPackageEvent.NewOutContractServicePackages.Any() && upgradeSrvPackageEvent.NewOutContractServicePackages.All(a => a.OldId.HasValue))
                    {
                        outContractEntity.AddDomainEvent(upgradeSrvPackageEvent);
                    }

                    //Radius
                    if (newTransactionServicePackage.ServicePackageId.HasValue)
                    {
                        string[] radiusAccounts = contractPackageEntities.Where(e => !string.IsNullOrWhiteSpace(e.RadiusAccount)).Select(e => e.RadiusAccount).ToArray();
                        if (radiusAccounts.Any())
                        {
                            await _radiusAndBrasManagementService.MultipleDeactivateUserByUserName(radiusAccounts);
                        }

                        string[] radiusAccountsUpgrade = upgradeSrvPackageEvent.NewOutContractServicePackages.Where(e => !string.IsNullOrWhiteSpace(e.RadiusAccount)).Select(e => e.RadiusAccount).ToArray();
                        if (radiusAccountsUpgrade.Any())
                        {
                            var upgradeHandleResponse = await _radiusAndBrasManagementService
                                .MultipleUpgradeSrvByUserName(radiusAccountsUpgrade, newTransactionServicePackage.ServicePackageId.Value);
                            if (!upgradeHandleResponse.IsSuccess)
                            {
                                return upgradeHandleResponse;
                            }
                        }
                    }

                    item.EffectiveDate = request.EffectiveDate;
                    item.StatusId = TransactionStatus.Acceptanced.Id;
                    actionResp.CombineResponse(await _transactionRepository.UpdateAndSave(item));

                    if (!actionResp.IsSuccess)
                    {
                        throw new ContractDomainException(actionResp.Message);
                    }
                }
            }
            return actionResp;
        }

        async Task<ActionResponse> UpdateDeployNewOutContract(int[] arrTransactionId, ActionResponse actionResp, CUApprovedTransactionSimplesCommand request)
        {
            var transactions = _transactionRepository.GetByIds(arrTransactionId);
            foreach (var transaction in transactions)
            {
                var contractEntity = await _outContractRepository.GetByIdAsync(transaction.OutContractId);
                //Update nghiệm thu dịch vụ và thiết bị phụ lục
                foreach (var transactionChannel in transaction.TransactionServicePackages)
                {
                    var contractChannel = contractEntity.ServicePackages.First(c => c.Id == transactionChannel.OutContractServicePackageId);

                    transactionChannel.IsAcceptanced = true;
                    transactionChannel.StatusId = OutContractServicePackageStatus.Developed.Id;

                    if (transactionChannel.TimeLine?.Effective == null)
                    {
                        transactionChannel.TimeLine.Effective = DateTime.UtcNow.AddHours(7);
                    }
                    contractChannel.SetEffectiveDate(transactionChannel.TimeLine.Effective.Value);
                    if (request.StartBillingDate.HasValue)
                    {
                        transactionChannel.TimeLine.StartBilling = request.StartBillingDate?.ToExactLocalDate();
                        contractChannel.SetStartBillingDate(transactionChannel.TimeLine.StartBilling.Value);
                    }

                    if (contractChannel.HasStartAndEndPoint)
                    {
                        foreach (var equipment in transactionChannel.StartPoint.Equipments)
                        {
                            if (equipment.ContractEquipmentId.HasValue)
                            {
                                var contractEquipment = contractChannel.StartPoint.Equipments
                                    .First(e => e.Id == equipment.ContractEquipmentId);
                                equipment.SetRealUnits(equipment.RealUnit > 0 ? equipment.RealUnit : equipment.ExaminedUnit);
                                contractEquipment.SetExaminedUnits(equipment.ExaminedUnit);
                                contractEquipment.SetRealUnits(equipment.RealUnit);
                                contractEquipment.UpdateMacCodes(equipment.MacAddressCode);
                                contractEquipment.UpdateSerialCode(equipment.SerialCode);
                                contractEquipment.CalculateTotal();
                            }
                            else
                            {
                                contractChannel.StartPoint.AddOrUpdateEquipment(new CUContractEquipmentCommand(equipment));
                            }
                        }

                        var removedSpEquipmentIds = contractChannel.StartPoint.Equipments
                            .Where(c => c.Id > 0 && transactionChannel.StartPoint.Equipments.All(ep => ep.ContractEquipmentId != c.Id))
                            .Select(c => c.Id);
                        contractChannel.StartPoint.RemoveEquipments(removedSpEquipmentIds);
                    }

                    foreach (var equipment in transactionChannel.EndPoint.Equipments)
                    {
                        if (equipment.ContractEquipmentId.HasValue)
                        {
                            var contractEquipment = contractChannel.EndPoint.Equipments
                            .First(e => e.Id == equipment.ContractEquipmentId);
                            equipment.SetRealUnits(equipment.RealUnit > 0 ? equipment.RealUnit : equipment.ExaminedUnit);
                            contractEquipment.SetExaminedUnits(equipment.ExaminedUnit);
                            contractEquipment.SetRealUnits(equipment.RealUnit);
                            contractEquipment.UpdateMacCodes(equipment.MacAddressCode);
                            contractEquipment.UpdateSerialCode(equipment.SerialCode);
                            contractEquipment.CalculateTotal();
                        }
                        else
                        {
                            contractChannel.EndPoint.AddOrUpdateEquipment(new CUContractEquipmentCommand(equipment));
                        }
                    }

                    var removedEpEquipmentIds = contractChannel.EndPoint.Equipments
                        .Where(c => c.Id > 0 && transactionChannel.EndPoint.Equipments.All(ep => ep.ContractEquipmentId != c.Id))
                        .Select(c => c.Id);
                    contractChannel.EndPoint.RemoveEquipments(removedEpEquipmentIds);
                    contractChannel.CalculateTotal();
                }

                contractEntity.SetSignedStatus();
                // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
                contractEntity.CalculateTotal();

                var savedContractEntityRsp = await _outContractRepository.UpdateAndSave(contractEntity);
                actionResp.CombineResponse(savedContractEntityRsp);
                if (!actionResp.IsSuccess)
                {
                    throw new ContractDomainException(actionResp.Message);
                }

                #region Add/update attachment files                
                await AttachmentFileHandler(request.AttachmentFiles, transaction.Id, transaction.AcceptanceStaff, transaction.OutContractId, transaction.InContractId);
                #endregion

                #region Add/update attachment files

                #endregion
                //Radius
                string[] radiusAccounts = contractEntity.ServicePackages
                    .Where(e => !string.IsNullOrWhiteSpace(e.RadiusAccount))
                    .Select(e => e.RadiusAccount)
                    .ToArray();

                if (radiusAccounts.Any())
                {
                    var activeHandleResponse = await _radiusAndBrasManagementService.MultipleActivateUserByUserName(radiusAccounts);
                    if (!activeHandleResponse.IsSuccess)
                    {
                        return activeHandleResponse;
                    }
                }

                transaction.EffectiveDate = request.EffectiveDate;
                transaction.AcceptanceStaff = request.AcceptanceStaff;
                transaction.StatusId = TransactionStatus.AcceptanceApproved.Id;
                var savedRsp = await _transactionRepository.UpdateAndSave(transaction);
                actionResp.CombineResponse(savedRsp);
            }

            return actionResp;
        }

        async Task<ActionResponse> RenewContract(int[] arrTransactionId, ActionResponse actionResp, CUApprovedTransactionSimplesCommand request)
        {
            var actionResponse = new ActionResponse();
            var transactions = _transactionRepository.GetByIds(arrTransactionId);

            foreach (var transaction in transactions)
            {
                if (transaction.OutContractId.HasValue)
                {
                    var outContract = await _outContractRepository.GetByIdAsync(transaction.OutContractId);
                    outContract.TimeLine.RenewPeriod = transaction.ContractRenewMonths;
                    outContract.TimeLine.Expiration = transaction.ContractNewExpirationDate;
                    actionResp.CombineResponse(_outContractRepository.Update(outContract));
                }
                else if (transaction.InContractId.HasValue)
                {
                    var inContract = await _inContractRepository.GetByIdAsync(transaction.InContractId);
                    inContract.TimeLine.RenewPeriod = transaction.ContractRenewMonths;
                    inContract.TimeLine.Expiration = transaction.ContractNewExpirationDate;
                    actionResp.CombineResponse(_inContractRepository.Update(inContract));
                }

                transaction.EffectiveDate = request.EffectiveDate;
                transaction.AcceptanceStaff = request.AcceptanceStaff;
                transaction.StatusId = TransactionStatus.AcceptanceApproved.Id;
                actionResponse.CombineResponse(await _transactionRepository.UpdateAndSave(transaction));

                if (!actionResponse.IsSuccess)
                {
                    throw new ContractDomainException(actionResponse.Message);
                }

                //gửi notification
                await this.SendNotificationAfterApprove(transaction.Type, transaction.Id, transaction.Code, transaction.TechnicalStaffId, transaction.IsTechnicalConfirmation, transaction.HasEquipment, transaction.CreatorUserId);
            }

            return actionResponse;
        }

        async Task<bool> SendNotificationAfterApprove(int type, int id, string code,
            string technicalStaffId, bool? isTechnicalConfirmation, bool? hasEquipment, string creatorUserId)
        {
            var warehouseRole = _configuration.GetValue<string>("RoleCodeWarehouse");
            var notiReq = new PushNotificationRequest()
            {
                Zone = NotificationZone.Contract,
                Type = NotificationType.Private,
                Category = NotificationCategory.ContractTransaction,
                Title = $"Phụ lục {code} chuyển trạng thái sang Đã nghiệm thu",
                Content = $"Phụ lục {code} chuyển trạng thái từ Đã triển khai sang Đã nghiệm thu.",
                Payload = JsonConvert.SerializeObject(new
                {
                    Type = type,
                    TypeName = TransactionType.GetTypeName(type),
                    Id = id,
                    Code = code,
                    Category = NotificationCategory.ContractTransaction
                },
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
            };


            if (!string.IsNullOrEmpty(technicalStaffId) && isTechnicalConfirmation == true) // gửi notification đến nv kĩ thuật
            {
                await _notificationGrpcService.PushNotificationByUids(notiReq, technicalStaffId);
            }

            if (!string.IsNullOrEmpty(warehouseRole) && hasEquipment == true)// có thiết bị => gửi notìication đến nv kho
            {
                await _notificationGrpcService.PushNotificationByRole(notiReq, warehouseRole);
            }
            else
            {
                await _notificationGrpcService.PushNotificationByUids(notiReq, creatorUserId);
            }

            return true;
        }

        private TransactionDTO MapTransactionDTO(Transaction tran)
        {
            var tranDTO = this._mapper.Map<TransactionDTO>(tran);
            tranDTO.CurrencyUnitId = tranDTO.CurrencyUnitId;
            tranDTO.CurrencyUnitCode = tranDTO.CurrencyUnitCode;
            tranDTO.MarketAreaId = tranDTO.MarketAreaId;
            tranDTO.MarketAreaName = tranDTO.MarketAreaName;
            return tranDTO;
        }

        async Task<ActionResponse> CreateReceiptVoucherFee(List<TransactionDTO> transactionDTOs, ActionResponse actionResponse)
        {
            var tranEvent = new TransactionDomainEvent(transactionDTOs);
            actionResponse.CombineResponse(await _mediator.Send(tranEvent));
            if (!actionResponse.IsSuccess)
            {
                throw new ContractDomainException(actionResponse.Message);
            }

            return actionResponse;
        }
    }
}
