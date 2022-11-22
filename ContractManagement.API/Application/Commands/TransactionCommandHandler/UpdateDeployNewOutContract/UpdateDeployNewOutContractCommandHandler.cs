using AutoMapper;
using ContractManagement.API.Application.Commands.TransactionCommandHandler.AcceptancedTransactions;
using ContractManagement.API.Application.Services.Radius;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
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

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpdateDeployNewOutContract
{
    public class UpdateDeployNewOutContractCommandHandler : AcceptancedTransactionsBaseHandler, IRequestHandler<UpdateDeployNewOutContractCommand, ActionResponse<TransactionDTO>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IFileRepository _fileRepository;
        private readonly IServicePackageQueries _servicePackageQueries;
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        private readonly IOutContractRepository _contractRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMapper _mapper;
        private readonly IPromotionDetailQueries _promotionDetailQueries;
        private readonly IRadiusAndBrasManagementService _radiusAndBrasManagementService;

        public UpdateDeployNewOutContractCommandHandler(ITransactionRepository transactionRepository,
             ITransactionQueries transactionQueries,
             IAttachmentFileResourceGrpcService attachmentFileService,
             IFileRepository fileRepository,
             IServicePackageQueries servicePackageQueries,
             IEquipmentTypeQueries equipmentTypeQueries,
             IOutContractRepository contractRepository,
             IWrappedConfigAndMapper configAndMapper,
             IMapper mapper,
             IPromotionDetailQueries promotionDetailQueries,
             IRadiusAndBrasManagementService radiusAndBrasManagementService)
        {
            _transactionRepository = transactionRepository;
            _transactionQueries = transactionQueries;
            _attachmentFileService = attachmentFileService;
            _fileRepository = fileRepository;
            _servicePackageQueries = servicePackageQueries;
            _equipmentTypeQueries = equipmentTypeQueries;
            _contractRepository = contractRepository;
            _configAndMapper = configAndMapper;
            _mapper = mapper;
            _promotionDetailQueries = promotionDetailQueries;
            _radiusAndBrasManagementService = radiusAndBrasManagementService;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(UpdateDeployNewOutContractCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionDTO>();
            var transactionEntity = await _transactionRepository.GetByIdAsync(request.Id);

            var equipsOld = request.TransactionEquipments.Where(o => o.IsOld == true).ToList();


            //Update nghiệm thu dịch vụ và thiết bị phụ lục
            foreach (var e in transactionEntity.TransactionServicePackages)
            {
                if (e.IsAcceptanced != true)
                {
                    if (request.TransactionServicePackages.Any(a => a.Id == e.Id))
                    {
                        e.IsAcceptanced = true;
                        e.TimeLine.Effective = DateTime.Now;
                        e.StatusId = OutContractServicePackageStatus.Developed.Id;

                        e.CId = request.TransactionServicePackages.First(tsv => tsv.Id == e.Id).CId;
                        
                        //TODO Outlet channel point logic changes
                        //e.TransactionEquipments = e.TransactionEquipments.Where(o => request.TransactionEquipments.Any(a => a.Id == o.Id)).ToList();
                        //if (equipsOld.Count > 0)
                        //{
                        //    e.TransactionEquipments.ForEach(equip =>
                        //    {
                        //        var equipOld = equipsOld.FirstOrDefault(f => f.Id == equip.Id);
                        //        if (equipOld != null)
                        //        {
                        //            equip.SerialCode = equipOld.SerialCode;
                        //            equip.RealUnit = equipOld.RealUnit;
                        //        }
                        //    });
                        //}

                        //var equips = request.TransactionEquipments.Where(o => o.TransactionServicePackageId == e.Id && o.IsOld != true).ToList();

                        //foreach (var equip in equips)
                        //{
                        //    equip.TransactionId = transactionEntity.Id;
                        //    e.TransactionEquipments.Add(new TransactionEquipment(equip));
                        //}
                    }
                }
            }

            if (transactionEntity.TransactionServicePackages.All(a => a.IsAcceptanced == true))
            {
                transactionEntity.StatusId = TransactionStatus.Acceptanced.Id;
                transactionEntity.EffectiveDate = DateTime.Now;
            }

            var transaction = _transactionQueries.FindCanAcceptancedById(transactionEntity.Id);

            var contractEntity = await _contractRepository.GetByIdAsync(transaction.OutContractId);

            //Cập nhật gói cước nếu đã tồn tại trong hợp đồng
            for (int i = 0;
                i < transaction.TransactionServicePackages.Count(o => request.TransactionServicePackages.Any(a => a.Id == o.Id));
                i++)
            {
                var transactionServicePackage = transaction.TransactionServicePackages.Where(o => request.TransactionServicePackages.Any(a => a.Id == o.Id)).ElementAt(i);
                var updatePackageCommand = transactionServicePackage.MapTo<CUContractServicePackageCommand>(_configAndMapper.MapperConfig);
                updatePackageCommand.CId = request.TransactionServicePackages.First(tsv => tsv.Id == transactionServicePackage.Id).CId;
                updatePackageCommand.Id = transactionServicePackage.OutContractServicePackageId;
                var contractPackageEntity = contractEntity.ServicePackages.First(e => e.Id == updatePackageCommand.Id);
                // Lấy thông tin gói cước
                var packageInfo = _servicePackageQueries.Find(updatePackageCommand.ServicePackageId);
                MapPackageCommand(ref updatePackageCommand, packageInfo);
                updatePackageCommand.UpdatedBy = request.AcceptanceStaff;
                updatePackageCommand.UpdatedDate = DateTime.Now;

                //TODO Outlet channel point logic changes
                //var equipRemoveIds = contractPackageEntity.Equipments.Select(o => o.Id)
                //    .Where(o => !request.TransactionEquipments.Any(a => a.ContractEquipmentId == o))
                //    .ToArray();

                //Tuấn sửa phần này trong OutContractPackageEntity chỉ cần cập nhật ngày nghiệm thu
                ////Ngày tính cước tiếp theo sau khi dịch vụ đã được nghiệm thu                    
                //updatePackageCommand.TimeLine.NextBilling = DateTime.Now.AddMonths(updatePackageCommand.TimeLine.PaymentPeriod);
                ////Khuyến mãi ngày hoặc tháng 
                //var promotionDetail = _promotionDetailQueries.GetPromotionDetailByOCIdAndSPId(updatePackageCommand.Id);
                //if (promotionDetail.PromotionValueType == PromotionValueType.UseTimeDay.Id)
                //{
                //    updatePackageCommand.TimeLine.NextBilling = updatePackageCommand.TimeLine.NextBilling.Value.AddDays(promotionDetail.Quantity);
                //}
                //else if (promotionDetail.PromotionValueType == PromotionValueType.UseTimeMonth.Id)
                //{
                //    updatePackageCommand.TimeLine.NextBilling = updatePackageCommand.TimeLine.NextBilling.Value.AddMonths(promotionDetail.Quantity);
                //}

                //TODO Outlet channel point logic changes
                //contractPackageEntity.RemoveEquipments(equipRemoveIds);
                updatePackageCommand.TimeLine.Effective = DateTime.Now;
                updatePackageCommand.StatusId = OutContractServicePackageStatus.Developed.Id;
                contractPackageEntity.Update(updatePackageCommand);

                // Thêm/cập nhật/xóa thiết bị của dịch vụ
                for (int j = 0;
                    j < request.TransactionEquipments.Count(equip => equip.TransactionServicePackageId == transactionServicePackage.Id);
                    j++)
                {
                    var equipmentCommand = request.TransactionEquipments.Where(equip => equip.TransactionServicePackageId == transactionServicePackage.Id)
                        .ElementAt(j).MapTo<CUContractEquipmentCommand>(_configAndMapper.MapperConfig);

                    var eqInfo = _equipmentTypeQueries.Find(equipmentCommand.EquipmentId);
                    MapEquipmentCommand(ref equipmentCommand, packageInfo, eqInfo);
                    if (equipmentCommand.Id > 0)
                    {
                        //TODO Outlet channel point logic changes
                        //var equipmentEntity = contractPackageEntity.Equipments.First(e => e.Id == equipmentCommand.Id);
                        //equipmentEntity.Update(equipmentCommand);
                        //equipmentEntity.CalculateTotal();
                    }
                    else
                    {
                        equipmentCommand.OutContractPackageId = updatePackageCommand.Id;

                        //TODO Outlet channel point logic changes
                        //contractPackageEntity.AddContractEquipment(equipmentCommand);
                    }
                }
                contractPackageEntity.CalculateTotal();
            }

            // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
            contractEntity.CalculateTotal();

            var savedContractEntityRsp = await _contractRepository.UpdateAndSave(contractEntity);
            actionResp.CombineResponse(savedContractEntityRsp);
            if (!actionResp.IsSuccess)
            {
                throw new ContractDomainException(actionResp.Message);
            }


            if (request.AttachmentFiles != null)
            {
                var attachmentFiles =
                    await _attachmentFileService.PersistentFiles(request.AttachmentFiles.Select(c => c.TemporaryUrl)
                        .ToArray());
                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new ContractDomainException("An error has occured when save the attachment files");

                foreach (var fileCommand in attachmentFiles)
                {
                    fileCommand.CreatedBy = request.AcceptanceStaff;
                    fileCommand.TransactionId = transactionEntity.Id;
                    fileCommand.OutContractId = contractEntity.Id;
                    var savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                    if (!savedFileRsp.IsSuccess) throw new ContractDomainException(savedFileRsp.Message);

                }
            }

            //Radius
            //string[] radiusAccounts = contractEntity.ServicePackages.Where(e => !string.IsNullOrWhiteSpace(e.RadiusAccount)).Select(e => e.RadiusAccount).ToArray();
            //if (radiusAccounts.Any())
            //{
            //    bool activateSuccess = await _radiusAndBrasManagementService.MultipleActivateUserByUserName(radiusAccounts);
            //}

            transactionEntity.AcceptanceNotes = string.IsNullOrEmpty(transactionEntity.AcceptanceNotes) ? request.AcceptanceNotes : transactionEntity.AcceptanceNotes + "; " + request.AcceptanceNotes;
            var savedRsp = await _transactionRepository.UpdateAndSave(transactionEntity);
            actionResp.CombineResponse(savedRsp);

            if (!actionResp.IsSuccess)
            {
                throw new ContractDomainException(actionResp.Message);
            }

            return actionResp;
        }
    }
}
