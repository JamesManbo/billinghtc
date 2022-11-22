using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class AddedNewServicePackageIntegrationEventHandler : IIntegrationEventHandler<AddedNewServicePackageIntegrationEvent>
    {
        private readonly ILogger<CreateFirstBillingReceiptIntegrationEventHandler> _logger;
        private readonly IMediator _mediator;

        public AddedNewServicePackageIntegrationEventHandler(ILogger<CreateFirstBillingReceiptIntegrationEventHandler> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator;
        }

        public async Task Handle(AddedNewServicePackageIntegrationEvent @event)
        {
            var newChannel = @event.NewServicePackage;
            if (newChannel.TimeLine.PrepayPeriod > 0)
            {
                var createVoucherCommand = new CreateReceiptVoucherCommand()
                {
                    OutContractId = @event.OutContractId,
                    ContractCode = @event.ContractCode,
                    CurrencyUnitId = @event.CurrencyUnitId,
                    CurrencyUnitCode = @event.CurrencyUnitCode,
                    MarketAreaId = @event.MarketAreaId,
                    MarketAreaName = @event.MarketAreaName,
                    MarketAreaCode = @event.MarketAreaCode,
                    ProjectId = @event.ProjectId,
                    ProjectName = @event.ProjectName,
                    ProjectCode = @event.ProjectCode,
                    CashierUserId = @event.CashierUserId,
                    CashierUserName = @event.CashierUserName,
                    CashierFullName = @event.CashierFullName,
                    NumberBillingLimitDays = @event.NumberBillingLimitDays,
                    TypeId = ReceiptVoucherType.Signed.Id,
                    Payment = new PaymentMethod(
                                    @event.Payment.Form,
                                    @event.Payment.Method,
                                    @event.Payment.Address),
                    IssuedDate = DateTime.UtcNow.AddHours(7),
                    CreatedBy = "Hệ thống",
                    Source = Domain.Commands.CommandSource.IntegrationEvent,
                    StatusId = ReceiptVoucherStatus.Pending.Id,
                    IsPaidAll = true,
                    IsFirstVoucherOfContract = true,
                    OpeningDebtAmount = 0,
                    EquipmentTotalAmount = @event.EquipmentAmount,
                    Content = $"Thanh toán lần đầu phụ lục triển khai dịch vụ mới hợp đồng {@event.ContractCode}"
                };

                var receiptVoucherTarget = new CUVoucherTargetCommand()
                {
                    ApplicationUserIdentityGuid = @event.Contractor.ApplicationUserIdentityGuid,
                    UserIdentityGuid = @event.Contractor.UserIdentityGuid,
                    IdentityGuid = @event.Contractor.IdentityGuid,
                    IsEnterprise = @event.Contractor.IsEnterprise,
                    IsPayer = true,
                    TargetAddress = @event.Contractor.ContractorAddress,
                    TargetPhone = @event.Contractor.ContractorPhone,
                    TargetFax = @event.Contractor.ContractorFax,
                    TargetIdNo = @event.Contractor.ContractorIdNo,
                    TargetTaxIdNo = @event.Contractor.ContractorTaxIdNo,
                    TargetEmail = @event.Contractor.ContractorEmail,
                    TargetFullName = @event.Contractor.ContractorFullName,
                    TargetCode = @event.Contractor.ContractorCode
                };

                createVoucherCommand.Target = receiptVoucherTarget;

                var receiptVoucherLine = new CUReceiptVoucherDetailCommand
                {
                    CurrencyUnitCode = @event.CurrencyUnitCode,
                    CurrencyUnitId = @event.CurrencyUnitId,
                    ServiceId = newChannel.ServiceId,
                    ServiceName = newChannel.ServiceName,
                    ServicePackageId = newChannel.ServicePackageId,
                    ServicePackageName = newChannel.PackageName,
                    CreatedBy = "Hệ thống",
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    PackagePrice = newChannel.PackagePrice,
                    UsingMonths = newChannel.TimeLine.PrepayPeriod,
                    OutContractServicePackageId = newChannel.Id,
                    OtherFeeTotal = newChannel.OtherFee,
                    PromotionAmount = newChannel.PromotionAmount,
                    ReductionFee = 0,
                    CId = newChannel.CId,
                    DiscountAmountSuspend = 0,
                    TaxAmount = newChannel.TaxAmount,
                    TaxPercent = newChannel.TaxPercent,
                    InstallationFee = newChannel.InstallationFee,
                    EquipmentTotalAmount = newChannel.EquipmentAmount,
                    IsFirstDetailOfService = true,
                    SubTotal = newChannel.SubTotal,
                    SubTotalBeforeTax = newChannel.SubTotalBeforeTax,
                    GrandTotalBeforeTax = newChannel.GrandTotalBeforeTax,
                    GrandTotal = newChannel.GrandTotal,
                    PricingType = newChannel.FlexiblePricingTypeId
                };

                createVoucherCommand.PromotionTotalAmount = receiptVoucherLine.PromotionAmount;

                if (newChannel.TimeLine.NextBilling.HasValue)
                {
                    if (newChannel.TimeLine.PaymentForm == PaymentMethodForm.Prepaid)
                    {
                        receiptVoucherLine.StartBillingDate = newChannel.TimeLine.StartBilling.Value;
                        receiptVoucherLine.EndBillingDate = newChannel.TimeLine.NextBilling.Value
                            .AddMonths(newChannel.TimeLine.PrepayPeriod)
                            .AddDays(-1);
                    }
                    else
                    {
                        receiptVoucherLine.StartBillingDate = newChannel.TimeLine.StartBilling.Value;
                        receiptVoucherLine.EndBillingDate = newChannel.TimeLine.NextBilling.Value.AddDays(-1);
                    }
                }

                foreach (var taxCategory in newChannel.OutContractServicePackageTaxes)
                {
                    var createVoucherTax = new CreateReceiptVoucherLineTaxCommand()
                    {
                        CreatedBy = "Hệ thống",
                        TaxCode = taxCategory.TaxCategoryCode,
                        TaxName = taxCategory.TaxCategoryName,
                        TaxValue = taxCategory.TaxValue
                    };
                    receiptVoucherLine.ReceiptVoucherLineTaxes.Add(createVoucherTax);
                }

                receiptVoucherLine.SubTotalBeforeTax = newChannel.PackagePrice * newChannel.TimeLine.PrepayPeriod;
                receiptVoucherLine.TaxPercent = receiptVoucherLine.ReceiptVoucherLineTaxes.Sum(t => t.TaxValue);
                receiptVoucherLine.TaxAmount = (decimal)receiptVoucherLine.TaxPercent * (receiptVoucherLine.SubTotalBeforeTax / 100);
                receiptVoucherLine.InstallationFee = newChannel.InstallationFee;
                receiptVoucherLine.EquipmentTotalAmount = newChannel.EquipmentAmount;
                receiptVoucherLine.OtherFeeTotal = newChannel.OtherFee;
                receiptVoucherLine.CalculateTotal();
                createVoucherCommand.ReceiptLines.Add(receiptVoucherLine);

                //var taxAmount = receiptVoucherLine.GrandTotal * (decimal)(taxPercentValue / 100);
                //createVoucherCommand.CashTotal = receiptVoucherLine.GrandTotal + taxAmount;

                createVoucherCommand.InstallationFee = @event.InstallationFee;
                createVoucherCommand.EquipmentTotalAmount = @event.EquipmentAmount;
                createVoucherCommand.OtherFee = @event.OtherFee; ;

                createVoucherCommand.Content = $"Thanh toán phụ lục thêm mới dịch vụ " +
                    $"{newChannel.ServiceName}, " +
                    $"gói cước {newChannel.PackageName} của hợp đồng số: {createVoucherCommand.ContractCode}";

                try
                {
                    var createRsp = await _mediator.Send(createVoucherCommand);
                    if (!createRsp.IsSuccess)
                    {
                        _logger.LogError("Error handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent}). Message: {Message}", @event.Id, Program.AppName, @event, createRsp.Errors);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
