using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Resouces;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models;
using EventBus.Abstractions;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class TransactionIntergrationEventHandler : IIntegrationEventHandler<TransactionIntegrationEvent>
    {
        private readonly ILogger<TransactionIntergrationEventHandler> _logger;
        private readonly IMediator _mediator;

        public TransactionIntergrationEventHandler(ILogger<TransactionIntergrationEventHandler> logger,
            IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator;
        }

        public async Task Handle(TransactionIntegrationEvent @event)
        {

            foreach (var transaction in @event.TransactionDTO)
            {
                var svPackage = transaction.TransactionServicePackages.GroupBy(g => new { g.PaymentTarget.ApplicationUserIdentityGuid, g.CurrencyUnitId });

                decimal fee = 0;
                string feeName = "";
                if (transaction.SuspendHandleFee.HasValue)
                {
                    fee = transaction.SuspendHandleFee.Value;
                    feeName = "tạm ngưng dịch vụ";
                }
                else if (transaction.RestoreHandleFee.HasValue)
                {
                    fee = transaction.RestoreHandleFee.Value;
                    feeName = "khôi phục dịch vụ";
                }
                else if (transaction.UpgradeFee.HasValue)
                {
                    fee = transaction.UpgradeFee.Value;
                    feeName = "nâng cấp";
                }
                else if (transaction.ChangeEquipmentFee.HasValue)
                {
                    fee = transaction.ChangeEquipmentFee.Value;
                    feeName = "thay đổi thiết bị";
                }
                else if (transaction.ChaningLocationFee.HasValue)
                {
                    fee = transaction.ChaningLocationFee.Value;
                    feeName = "thay đổi địa điểm";
                }
                else if (transaction.ChangingPackageFee.HasValue)
                {
                    fee = transaction.ChangingPackageFee.Value;
                    feeName = "thay đổi gói cước";
                }
                else
                {
                    fee = 0;
                    feeName = "không đồng";
                }    

                #region Create initialize receipt voucher of contract           
                var createInitReceiptVoucherCommand = new CreateReceiptVoucherCommand()
                {
                    CurrencyUnitId = transaction.CurrencyUnitId,
                    CurrencyUnitCode = transaction.CurrencyUnitCode,
                    OutContractId = transaction.OutContractId ?? 0,
                    ContractCode = transaction.ContractCode,
                    MarketAreaId = transaction.MarketAreaId,
                    MarketAreaName = transaction.MarketAreaName,
                    //MarketAreaCode = @event.MarketAreaCode,
                    ProjectId = transaction.ProjectId,
                    ProjectName = transaction.ProjectName,
                    CashierUserId = "",
                    CashierUserName = "",
                    CashierFullName = "",
                    NumberBillingLimitDays = 30,
                    TypeId = ReceiptVoucherType.Other.Id,
                    IssuedDate = DateTime.UtcNow.AddHours(7),
                    CreatedBy = "Hệ thống",
                    Source = Domain.Commands.CommandSource.IntegrationEvent,
                    StatusId = ReceiptVoucherStatus.Pending.Id,
                    OpeningDebtAmount = 0,
                    OtherFee = fee,
                    Content = $"Phiếu thu phí {feeName} cho hợp đồng số: {transaction.ContractCode}",
                    IsAutomaticGenerate = true
                };
                #endregion

                // var svPackage = outContract.TransactionServicePackages.GroupBy(g => new { g.PaymentTarget.ApplicationUserIdentityGuid, g.CurrencyUnitId });
                foreach (var group in svPackage)
                {
                    createInitReceiptVoucherCommand.ReceiptLines = new List<CUReceiptVoucherDetailCommand>();
                    var paymentTarget = group.FirstOrDefault(s => s.CurrencyUnitId == group.Key.CurrencyUnitId
                                                                        && s.PaymentTarget.ApplicationUserIdentityGuid == group.Key.ApplicationUserIdentityGuid);

                    var receiptVoucherTarget = new CUVoucherTargetCommand()
                    {
                        IdentityGuid = paymentTarget.PaymentTarget.IdentityGuid,
                        UserIdentityGuid = paymentTarget.PaymentTarget.UserIdentityGuid,
                        ApplicationUserIdentityGuid = paymentTarget.PaymentTarget.ApplicationUserIdentityGuid,
                        IsEnterprise = paymentTarget.PaymentTarget.IsEnterprise,
                        IsPayer = true,
                        TargetAddress = paymentTarget.PaymentTarget.ContractorAddress,
                        TargetPhone = paymentTarget.PaymentTarget.ContractorPhone,
                        TargetFax = paymentTarget.PaymentTarget.ContractorFax,
                        TargetIdNo = paymentTarget.PaymentTarget.ContractorIdNo,
                        TargetTaxIdNo = paymentTarget.PaymentTarget.ContractorTaxIdNo,
                        TargetEmail = paymentTarget.PaymentTarget.ContractorEmail,
                        TargetFullName = paymentTarget.PaymentTarget.ContractorFullName,
                        TargetCode = paymentTarget.PaymentTarget.ContractorCode
                    };

                    var receiptVoucherLine = new CUReceiptVoucherDetailCommand();
                    foreach (var contractSrvPackage in group)
                    {
                        receiptVoucherLine = new CUReceiptVoucherDetailCommand
                        {
                            CurrencyUnitId = contractSrvPackage.CurrencyUnitId,
                            CurrencyUnitCode = contractSrvPackage.CurrencyUnitCode,
                            ServiceId = contractSrvPackage.ServiceId,
                            ServiceName = contractSrvPackage.ServiceName,
                            ServicePackageId = contractSrvPackage.ServicePackageId,
                            ServicePackageName = contractSrvPackage.PackageName,
                            CreatedBy = "Hệ thống",
                            CreatedDate = DateTime.UtcNow,
                            OutContractServicePackageId = contractSrvPackage.OutContractServicePackageId,
                            SubTotalBeforeTax = fee,
                            IsFirstDetailOfService = true,
                            CId = contractSrvPackage.CId,
                            TaxAmount = contractSrvPackage.TaxAmount,
                        };
                        receiptVoucherLine.CalculateTotal();

                        createInitReceiptVoucherCommand.ReceiptLines.Add(receiptVoucherLine);

                        createInitReceiptVoucherCommand.Target = receiptVoucherTarget;
                        createInitReceiptVoucherCommand.VoucherCode = "";
                        createInitReceiptVoucherCommand.Payment = new PaymentMethod(
                                        PaymentMethodForm.Prepaid,
                                        1,
                                        "");
                        var createRsp = await _mediator.Send(createInitReceiptVoucherCommand);
                        if (!createRsp.IsSuccess)
                        {
                            _logger.LogError("Error handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent}). Message: {Message}", @event.Id, Program.AppName, @event, createRsp.Errors);
                        }
                    }
                }
            }          

        }
    }
}
