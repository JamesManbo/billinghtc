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
    public class CreateFirstBillingReceiptIntegrationEventHandler
        : IIntegrationEventHandler<CreateFirstBillingReceiptIntegrationEvent>
    {
        private readonly ILogger<CreateFirstBillingReceiptIntegrationEventHandler> _logger;
        private readonly IMediator _mediator;

        public CreateFirstBillingReceiptIntegrationEventHandler(ILogger<CreateFirstBillingReceiptIntegrationEventHandler> logger,
            IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator;
        }

        public async Task Handle(CreateFirstBillingReceiptIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.OutContract.Id, Program.AppName, @event);

            var outContract = @event.OutContract;
            //var currentContractor = outContract.Contractor;  

            #region Create initialize receipt voucher of contract           
            var createVoutcherTemplateCommand = new CreateReceiptVoucherCommand()
            {
                OutContractId = outContract.Id,
                ContractCode = outContract.ContractCode,
                MarketAreaId = outContract.MarketAreaId,
                MarketAreaName = outContract.MarketAreaName,
                MarketAreaCode = @event.MarketAreaCode,
                ProjectId = outContract.ProjectId,
                ProjectName = outContract.ProjectName,
                ProjectCode = @event.ProjectCode,
                CashierUserId = @event.OutContract.CashierUserId,
                CashierUserName = @event.OutContract.CashierUserName,
                CashierFullName = @event.OutContract.CashierFullName,
                NumberBillingLimitDays = @event.OutContract.NumberBillingLimitDays,
                TypeId = ReceiptVoucherType.Signed.Id,
                IssuedDate = DateTime.UtcNow.AddHours(7),
                CreatedBy = "Hệ thống",
                Source = Domain.Commands.CommandSource.IntegrationEvent,
                StatusId = ReceiptVoucherStatus.Pending.Id,
                IsPaidAll = true,
                IsFirstVoucherOfContract = true,
                PromotionTotalAmount = outContract.PromotionTotalAmount,
                OpeningDebtAmount = 0,
                EquipmentTotalAmount = outContract.EquipmentAmount,
                Content = $"Thanh toán lần đầu hợp đồng số: {outContract.ContractCode}"
            };
            #endregion

            var groupedByTargetAndCurrency = outContract.ActiveServicePackages.GroupBy(g => new { 
                g.PaymentTarget.ApplicationUserIdentityGuid,
                g.CurrencyUnitId,
                g.CurrencyUnitCode
            });

            foreach (var group in groupedByTargetAndCurrency)
            {
                var createFirstBillingVchrCmd = createVoutcherTemplateCommand.Copy();
                var paymentTarget = group.FirstOrDefault(s => 
                    s.CurrencyUnitId == group.Key.CurrencyUnitId && 
                    s.PaymentTarget.ApplicationUserIdentityGuid == group.Key.ApplicationUserIdentityGuid);

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

                createFirstBillingVchrCmd.CurrencyUnitId = group.Key.CurrencyUnitId;
                createFirstBillingVchrCmd.CurrencyUnitCode = group.Key.CurrencyUnitCode;
                createFirstBillingVchrCmd.Target = receiptVoucherTarget;
                createFirstBillingVchrCmd.VoucherCode = "";

                foreach (var contractSrvPackage in group)
                {
                    var receiptVoucherLine = new CUReceiptVoucherDetailCommand
                    {
                        CurrencyUnitId = contractSrvPackage.CurrencyUnitId,
                        CurrencyUnitCode = contractSrvPackage.CurrencyUnitCode,
                        ServiceId = contractSrvPackage.ServiceId,
                        ServiceName = contractSrvPackage.ServiceName,
                        ServicePackageId = contractSrvPackage.ServicePackageId,
                        ServicePackageName = contractSrvPackage.PackageName,
                        CreatedBy = "Hệ thống",
                        CreatedDate = DateTime.UtcNow,
                        PackagePrice = contractSrvPackage.PackagePrice,
                        OrgPackagePrice = contractSrvPackage.OrgPackagePrice,
                        OutContractServicePackageId = contractSrvPackage.Id,
                        OtherFeeTotal = contractSrvPackage.OtherFee,
                        InstallationFee = contractSrvPackage.InstallationFee,
                        UsingMonths = contractSrvPackage.TimeLine.PrepayPeriod,
                        PromotionAmount = contractSrvPackage.PromotionAmount,
                        IsFirstDetailOfService = true,
                        CId = contractSrvPackage.CId,
                        TaxAmount = contractSrvPackage.TaxAmount,
                        TaxPercent = contractSrvPackage.TaxPercent,
                        PricingType = contractSrvPackage.FlexiblePricingTypeId
                    };

                    if (contractSrvPackage.TimeLine.NextBilling.HasValue)
                    {
                        if (contractSrvPackage.TimeLine.PaymentForm == PaymentMethodForm.Prepaid)
                        {
                            receiptVoucherLine.StartBillingDate = contractSrvPackage.TimeLine.StartBilling.Value;
                            receiptVoucherLine.EndBillingDate = contractSrvPackage.TimeLine.NextBilling.Value
                                .AddMonths(contractSrvPackage.TimeLine.PrepayPeriod)
                                .AddDays(-1);
                        }
                        else
                        {
                            receiptVoucherLine.StartBillingDate = contractSrvPackage.TimeLine.StartBilling.Value;
                            receiptVoucherLine.EndBillingDate = contractSrvPackage.TimeLine.NextBilling.Value.AddDays(-1);
                        }
                    }

                    if (contractSrvPackage.OutContractServicePackageTaxes != null
                        && contractSrvPackage.OutContractServicePackageTaxes.Any())
                    {
                        foreach (var channelTax in contractSrvPackage.OutContractServicePackageTaxes)
                        {
                            receiptVoucherLine.ReceiptVoucherLineTaxes.Add(
                                new CreateReceiptVoucherLineTaxCommand()
                                {
                                    TaxCode = channelTax.TaxCategoryCode,
                                    TaxName = channelTax.TaxCategoryName,
                                    TaxValue = channelTax.TaxValue,
                                    CreatedBy = "Hệ thống"
                                });
                        }
                    }

                    receiptVoucherLine.SubTotalBeforeTax = contractSrvPackage.PackagePrice * contractSrvPackage.TimeLine.PrepayPeriod;

                    receiptVoucherLine.TaxPercent = receiptVoucherLine.ReceiptVoucherLineTaxes.Sum(t => t.TaxValue);
                    receiptVoucherLine.TaxAmount = (decimal)receiptVoucherLine.TaxPercent * (receiptVoucherLine.SubTotalBeforeTax / 100);

                    receiptVoucherLine.InstallationFee = contractSrvPackage.InstallationFee;
                    receiptVoucherLine.EquipmentTotalAmount = contractSrvPackage.EquipmentAmount;
                    receiptVoucherLine.OtherFeeTotal = contractSrvPackage.OtherFee;

                    receiptVoucherLine.CalculateTotal();
                    createFirstBillingVchrCmd.ReceiptLines.Add(receiptVoucherLine);
                    createFirstBillingVchrCmd.Payment = new PaymentMethod(
                                    outContract.Payment.Form,
                                    outContract.Payment.Method,
                                    outContract.Payment.Address);
                }

                var createRsp = await _mediator.Send(createFirstBillingVchrCmd);
                if (!createRsp.IsSuccess)
                {
                    _logger.LogError("Error handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent}). Message: {Message}", @event.Id, Program.AppName, @event, createRsp.Errors);
                }
            }
        }
    }
}
