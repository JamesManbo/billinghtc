using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.ContractEventsCommand;
using DebtManagement.Domain.Commands.IntegrationEventCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Events.ContractEvents;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.IntegrationEventCommandHandler
{
    public class ChangeServicePackageIntegrationEventCommandHandler : IRequestHandler<ChangeServicePackageIntegrationEventCommand, ActionResponse>
    {
        private readonly IMediator _mediator;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly ITaxCategoryGrpcService _taxCategoryGrpcService;

        public ChangeServicePackageIntegrationEventCommandHandler(IMediator mediator, IReceiptVoucherRepository receiptVoucherRepository, ITaxCategoryGrpcService taxCategoryGrpcService)
        {
            _mediator = mediator;
            _receiptVoucherRepository = receiptVoucherRepository;
            _taxCategoryGrpcService = taxCategoryGrpcService;
        }

        public async Task<ActionResponse> Handle(ChangeServicePackageIntegrationEventCommand request, CancellationToken cancellationToken)
        {
            var outContractServicePackageClearingIntegrationEvents = new List<OutContractServicePackageClearingIntegrationEvent>();

            var oldReceiptVouchers = await _receiptVoucherRepository.GetByReceiptVoucherDetailStartDateAsync(request.OldOutContractServicePackage.Id
                , request.OldOutContractServicePackage.TimeLine.NextBilling.Value.AddDays(-request.OldOutContractServicePackage.TimeLine.PaymentPeriod));

            if (request.OutContract.Payment.Form == PaymentMethodForm.Prepay)
            {
                #region Tính số tiền còn lại của dịch vụ cũ
                decimal remainingAmountOldSP = 0;

                foreach (var oldRV in oldReceiptVouchers)
                {
                    if (oldRV.StatusId == ReceiptVoucherStatus.Pending.Id)
                    {
                        if (oldRV.ReceiptVoucherDetails.Count == 1)
                        {
                            oldRV.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
                        }
                        else
                        {
                            oldRV.RemoveReceiptVoucherLine(oldRV.ReceiptVoucherDetails.FirstOrDefault(e => e.OutContractServicePackageId == request.OldOutContractServicePackage.Id).IdentityGuid);
                        }
                        await _receiptVoucherRepository.UpdateAndSave(oldRV);
                        continue;
                    }

                    var voucherDetailLine = oldRV.ReceiptVoucherDetails.FirstOrDefault(e => e.OutContractServicePackageId == request.OldOutContractServicePackage.Id && e.EndBillingDate > request.NewOutContractServicePackage.TimeLine.Effective);

                    if (voucherDetailLine != null)
                    {
                        var startDay = request.NewOutContractServicePackage.TimeLine.Effective.Value.Day;
                        var startMonth = request.NewOutContractServicePackage.TimeLine.Effective.Value.Month;
                        var startYear = request.NewOutContractServicePackage.TimeLine.Effective.Value.Year;

                        var diffYears =
                            voucherDetailLine.EndBillingDate.Value.Year -
                            startYear;
                        var endDay = voucherDetailLine.EndBillingDate.Value.Day;
                        var endMonth =
                            voucherDetailLine.EndBillingDate.Value.Month + diffYears * 12;

                        if (startMonth == endMonth)
                        {
                            var daysOfMonth = DateTime.DaysInMonth(startYear, startMonth);
                            var usedDays = endDay - startDay + 1;
                            remainingAmountOldSP =
                                (voucherDetailLine.PackagePrice / daysOfMonth) * usedDays;
                        }
                        else
                        {
                            for (int idx = startMonth; idx <= endMonth; idx++)
                            {
                                int yearIdx = (int)Math.Floor((decimal)idx / 12);
                                int daysOfMonth = DateTime.DaysInMonth(
                                    idx - 12 * yearIdx,
                                    startYear + yearIdx
                                );
                                if (idx == startMonth)
                                {
                                    remainingAmountOldSP +=
                                        (voucherDetailLine.PackagePrice / daysOfMonth) *
                                        (daysOfMonth - startDay + 1);
                                }
                                else if (idx == endMonth)
                                {
                                    remainingAmountOldSP +=
                                        (voucherDetailLine.PackagePrice / daysOfMonth) * endDay;
                                }
                                else if (idx > startMonth && idx < endMonth)
                                {
                                    remainingAmountOldSP +=
                                        voucherDetailLine.PackagePrice;
                                }
                            }
                        }
                    }
                }

                #endregion
                var createVoucherCommand = new CreateReceiptVoucherCommand()
                {
                    OutContractId = request.NewOutContractServicePackage.OutContractId,
                    ContractCode = request.OutContract.ContractCode,
                    MarketAreaId = request.OutContract.MarketAreaId,
                    MarketAreaName = request.OutContract.MarketAreaName,
                    ProjectId = request.OutContract.ProjectId,
                    ProjectName = request.OutContract.ProjectName,
                    CashierUserId = request.OutContract.CashierUserId,
                    NumberBillingLimitDays = request.OutContract.NumberBillingLimitDays,
                    TypeId = ReceiptVoucherType.Billing.Id,
                    Payment = new PaymentMethod(
                                        request.OutContract.Payment.Form,
                                        request.OutContract.Payment.Method,
                                        request.OutContract.Payment.Address),
                    IssuedDate = request.NewOutContractServicePackage.TimeLine.Effective ?? DateTime.UtcNow,
                    CreatedBy = "Hệ thống",
                    Source = Domain.Commands.CommandSource.IntegrationEvent,
                    StatusId = ReceiptVoucherStatus.Pending.Id,
                    CashierReceivedDate = DateTime.Now,
                    IsPaidAll = true,
                    InvalidIssuedDate = !request.NewOutContractServicePackage.TimeLine.Effective.HasValue
                };

                var receiptVoucherTarget = new CUVoucherTargetCommand()
                {
                    IdentityGuid = request.OutContract.Contractor.IdentityGuid,
                    ApplicationUserIdentityGuid = request.OutContract.Contractor.ApplicationUserIdentityGuid,
                    UserIdentityGuid = request.OutContract.Contractor.UserIdentityGuid,
                    IsEnterprise = request.OutContract.Contractor.IsEnterprise,
                    IsPayer = true,
                    TargetAddress = request.OutContract.Contractor.ContractorAddress,
                    TargetPhone = request.OutContract.Contractor.ContractorPhone,
                    TargetFax = request.OutContract.Contractor.ContractorFax,
                    TargetIdNo = request.OutContract.Contractor.ContractorIdNo,
                    TargetTaxIdNo = request.OutContract.Contractor.ContractorTaxIdNo,
                    TargetEmail = request.OutContract.Contractor.ContractorEmail,
                    TargetFullName = request.OutContract.Contractor.ContractorFullName,
                    TargetCode = request.OutContract.Contractor.ContractorCode
                };

                createVoucherCommand.Target = receiptVoucherTarget;

                var receiptVoucherLine = new CUReceiptVoucherDetailCommand
                {
                    ServiceId = request.NewOutContractServicePackage.ServiceId,
                    ServiceName = request.NewOutContractServicePackage.ServiceName,
                    ServicePackageId = request.NewOutContractServicePackage.ServicePackageId,
                    ServicePackageName = request.NewOutContractServicePackage.PackageName,
                    CreatedBy = "Hệ thống",
                    CreatedDate = DateTime.Now,
                    PackagePrice = request.NewOutContractServicePackage.PackagePrice,
                    IsFirstDetailOfService = true,
                    OutContractServicePackageId = request.NewOutContractServicePackage.Id
                };

                outContractServicePackageClearingIntegrationEvents.Add(new OutContractServicePackageClearingIntegrationEvent()
                {
                    Change = remainingAmountOldSP,
                    OutContractServicePackageId = request.NewOutContractServicePackage.Id,
                    IsDaysPlus = request.NewOutContractServicePackage.IsOldOCSPCheaper != true
                });

                if (request.NewOutContractServicePackage.IsOldOCSPCheaper == true)
                {
                    if (request.NewOutContractServicePackage.TimeLine.Effective.HasValue)
                    {
                        receiptVoucherLine.StartBillingDate
                            = request.NewOutContractServicePackage.TimeLine.Effective.Value;
                        receiptVoucherLine.EndBillingDate
                            = receiptVoucherLine.StartBillingDate.Value.AddMonths(request.NewOutContractServicePackage.TimeLine.PaymentPeriod);
                    }

                    //receiptVoucherLine.SubTotal = request.NewOutContractServicePackage.PackagePrice
                    //        * request.NewOutContractServicePackage.TimeLine.PaymentPeriod - remainingAmountOldSP;
                    //receiptVoucherLine.InstallationFee = request.NewOutContractServicePackage.InstallationFee;
                    //receiptVoucherLine.OtherFeeTotal = request.NewOutContractServicePackage.OtherFee;
                    //receiptVoucherLine.GrandTotal = receiptVoucherLine.SubTotal + receiptVoucherLine.InstallationFee + receiptVoucherLine.OtherFeeTotal;
                }
                else
                {
                    if (request.NewOutContractServicePackage.TimeLine.Effective.HasValue)
                    {
                        receiptVoucherLine.StartBillingDate
                            = request.NewOutContractServicePackage.TimeLine.Effective.Value;
                        receiptVoucherLine.EndBillingDate
                            = receiptVoucherLine.StartBillingDate.Value.AddMonths(request.NewOutContractServicePackage.TimeLine.PaymentPeriod);
                        int monthsPlus = (int)Math.Floor(remainingAmountOldSP / request.NewOutContractServicePackage.PackagePrice);
                        receiptVoucherLine.EndBillingDate = receiptVoucherLine.EndBillingDate.Value.AddMonths(monthsPlus);
                        decimal change = remainingAmountOldSP - (request.NewOutContractServicePackage.PackagePrice * monthsPlus);
                        if (change > 0)
                        {
                            int daysInMonth = DateTime.DaysInMonth(receiptVoucherLine.EndBillingDate.Value.Year, receiptVoucherLine.EndBillingDate.Value.Month);
                            int dayEndBilling = receiptVoucherLine.EndBillingDate.Value.Day;
                            decimal monthsChange = change - (request.NewOutContractServicePackage.PackagePrice / daysInMonth) * (daysInMonth - dayEndBilling);
                            if (monthsChange > 0)
                            {
                                int daysInMonthChange = DateTime.DaysInMonth(receiptVoucherLine.EndBillingDate.Value.Year, receiptVoucherLine.EndBillingDate.Value.Month + 1);
                                receiptVoucherLine.EndBillingDate = receiptVoucherLine.EndBillingDate.Value.AddDays(daysInMonth - dayEndBilling);
                                int daysPlus = (int)Math.Floor(monthsChange / (request.NewOutContractServicePackage.PackagePrice / daysInMonthChange));
                                receiptVoucherLine.EndBillingDate = receiptVoucherLine.EndBillingDate.Value.AddDays(daysPlus);
                            }
                            else
                            {
                                int daysPlus = (int)Math.Floor(change / (request.NewOutContractServicePackage.PackagePrice / daysInMonth));
                                receiptVoucherLine.EndBillingDate = receiptVoucherLine.EndBillingDate.Value.AddDays(daysPlus);
                            }
                        }
                    }

                    //receiptVoucherLine.SubTotal = request.NewOutContractServicePackage.PackagePrice
                    //        * request.NewOutContractServicePackage.TimeLine.PaymentPeriod;
                    //receiptVoucherLine.InstallationFee = request.NewOutContractServicePackage.InstallationFee;
                    //receiptVoucherLine.OtherFeeTotal = request.NewOutContractServicePackage.OtherFee;
                    //receiptVoucherLine.GrandTotal = receiptVoucherLine.SubTotal + receiptVoucherLine.InstallationFee + receiptVoucherLine.OtherFeeTotal;
                }

                createVoucherCommand.ReceiptLines.Add(receiptVoucherLine);

                //foreach (var taxCategory in request.OutContract.TaxCategories)
                //{
                //    var createVoucherTax = new CreateReceiptVoucherLineTaxCommand()
                //    {
                //        CreatedBy = "Hệ thống",
                //        TaxCode = taxCategory.TaxCode,
                //        TaxName = taxCategory.TaxName,
                //        TaxValue = taxCategory.TaxValue
                //    };
                //    //createVoucherCommand.ReceiptVoucherTaxes.Add(createVoucherTax);
                //}

                //createVoucherCommand.InstallationFee = request.OutContract.InstallationFee;
                createVoucherCommand.EquipmentTotalAmount = request.OutContract.EquipmentAmount;
                createVoucherCommand.OtherFee = request.OutContract.OtherFee + request.Transaction.UpgradeFee ?? 0;

                createVoucherCommand.Content = $"Thanh toán phụ lục thêm mới dịch vụ " +
                    $"{request.NewOutContractServicePackage.ServiceName}, " +
                    $"gói cước {request.NewOutContractServicePackage.PackageName} vào hợp đồng số: {createVoucherCommand.ContractCode}";

                var createRsp = await _mediator.Send(createVoucherCommand);
                if (!createRsp.IsSuccess)
                {
                    throw new DebtDomainException(createRsp.Message);
                }
            }
            else if (request.OutContract.Payment.Form == PaymentMethodForm.Postpaid)
            {
                foreach (var oldRV in oldReceiptVouchers)
                {
                    if (oldRV.StatusId != ReceiptVoucherStatus.Pending.Id)
                    {
                        continue;
                    }

                    var voucherDetailLine = oldRV.ReceiptVoucherDetails.FirstOrDefault(e => e.OutContractServicePackageId == request.OldOutContractServicePackage.Id && e.EndBillingDate > request.NewOutContractServicePackage.TimeLine.Effective);

                    if (voucherDetailLine != null)
                    {
                        voucherDetailLine.SubTotal = 0;
                        voucherDetailLine.EndBillingDate = request.NewOutContractServicePackage.TimeLine.Effective.Value.AddDays(-1);

                        var startDay = voucherDetailLine.StartBillingDate.Value.Day;
                        var startMonth = voucherDetailLine.StartBillingDate.Value.Month;
                        var startYear = voucherDetailLine.StartBillingDate.Value.Year;

                        var diffYears =
                            voucherDetailLine.EndBillingDate.Value.Year -
                            startYear;
                        var endDay = voucherDetailLine.EndBillingDate.Value.Day;
                        var endMonth =
                            voucherDetailLine.EndBillingDate.Value.Month + diffYears * 12;

                        if (startMonth == endMonth)
                        {
                            var daysOfMonth = DateTime.DaysInMonth(startYear, startMonth);
                            var usedDays = endDay - startDay + 1;
                            voucherDetailLine.SubTotal =
                                (voucherDetailLine.PackagePrice / daysOfMonth) * usedDays;
                        }
                        else
                        {
                            for (int idx = startMonth; idx <= endMonth; idx++)
                            {
                                int yearIdx = (int)Math.Floor((decimal)idx / 12);
                                int daysOfMonth = DateTime.DaysInMonth(
                                    idx - 12 * yearIdx,
                                    startYear + yearIdx
                                );
                                if (idx == startMonth)
                                {
                                    voucherDetailLine.SubTotal +=
                                        (voucherDetailLine.PackagePrice / daysOfMonth) *
                                        (daysOfMonth - startDay + 1);
                                }
                                else if (idx == endMonth)
                                {
                                    voucherDetailLine.SubTotal +=
                                        (voucherDetailLine.PackagePrice / daysOfMonth) * endDay;
                                }
                                else if (idx > startMonth && idx < endMonth)
                                {
                                    voucherDetailLine.SubTotal +=
                                        voucherDetailLine.PackagePrice;
                                }
                            }
                        }

                        voucherDetailLine.GrandTotal =
                            voucherDetailLine.SubTotal -
                            voucherDetailLine.ReductionFee -
                            voucherDetailLine.PromotionAmount -
                            voucherDetailLine.DiscountAmountSuspend;
                    }

                    var receiptVoucherLine = new CUReceiptVoucherDetailCommand
                    {
                        ServiceId = request.NewOutContractServicePackage.ServiceId,
                        ServiceName = request.NewOutContractServicePackage.ServiceName,
                        ServicePackageId = request.NewOutContractServicePackage.ServicePackageId,
                        ServicePackageName = request.NewOutContractServicePackage.PackageName,
                        CreatedBy = "Hệ thống",
                        CreatedDate = DateTime.Now,
                        PackagePrice = request.NewOutContractServicePackage.PackagePrice,
                        IsFirstDetailOfService = true,
                        OutContractServicePackageId = request.NewOutContractServicePackage.Id
                    };

                    if (request.NewOutContractServicePackage.TimeLine.Effective.HasValue)
                    {
                        receiptVoucherLine.StartBillingDate
                            = request.NewOutContractServicePackage.TimeLine.Effective.Value;
                        receiptVoucherLine.EndBillingDate
                            = receiptVoucherLine.StartBillingDate.Value.AddMonths(request.NewOutContractServicePackage.TimeLine.PaymentPeriod);
                    }

                    //receiptVoucherLine.SubTotal = request.NewOutContractServicePackage.PackagePrice
                    //        * request.NewOutContractServicePackage.TimeLine.PaymentPeriod;
                    //receiptVoucherLine.InstallationFee = request.NewOutContractServicePackage.InstallationFee;
                    //receiptVoucherLine.OtherFeeTotal = request.NewOutContractServicePackage.OtherFee;
                    //receiptVoucherLine.GrandTotal = receiptVoucherLine.SubTotal + receiptVoucherLine.InstallationFee + receiptVoucherLine.OtherFeeTotal;
                    oldRV.OtherFee += request.Transaction.UpgradeFee ?? 0;

                    oldRV.AddReceiptVoucherDetail(receiptVoucherLine);
                    oldRV.CalculateTotal();

                    oldRV.Content += $"Thanh toán phụ lục thêm mới dịch vụ " +
                        $"{request.NewOutContractServicePackage.ServiceName}, " +
                        $"gói cước {request.NewOutContractServicePackage.PackageName} vào hợp đồng số: {oldRV.ContractCode}";

                    var updateRsp = await _receiptVoucherRepository.UpdateAndSave(oldRV);
                    if (!updateRsp.IsSuccess)
                    {
                        throw new DebtDomainException(updateRsp.Message);
                    }
                }

            }

            if (outContractServicePackageClearingIntegrationEvents.Any())
            {
                var updateOCSPClearingIntegrationEventCommand = new UpdateOCSPClearingIntegrationEventCommand(outContractServicePackageClearingIntegrationEvents);
                return await _mediator.Send(updateOCSPClearingIntegrationEventCommand);
            }

            return new ActionResponse();
        }
    }
}
