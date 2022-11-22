using DebtManagement.Domain.Commands.ClearingCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Services;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;

namespace DebtManagement.API.Application.Commands.ClearingCommandHandler
{
    public class CUClearingCommandHandler : IRequestHandler<CUClearingCommand, ActionResponse<ClearingDTO>>
    {
        private const int TRY_GENERATE_NEW_VOUCHER_CODE_TIMES = 5;
        private readonly IClearingRepository _clearingRepository;
        private readonly IVoucherTargetQueries _voucherTargetQueries;
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;
        private readonly IPaymentVoucherQueries _paymentVoucherQueries;
        private readonly IProjectGrpcService _projectGrpcService;
        private readonly IMarketAreaGrpcService _marketAreaGrpcService;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;
        private readonly IReceiptVoucherService _receiptVoucherService;
        private readonly IPaymentVoucherService _paymentVoucherService;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IAttachmentFileRepository _attachmentFileRepository;
        private readonly IExchangeRateGrpcService _exchangeRateGrpcService;
        
        private readonly IConfigurationSystemParameterGrpcService _configurationSystemParameterService;
        
        public CUClearingCommandHandler(
            IClearingRepository clearingRepository,
            IVoucherTargetQueries voucherTargetQueries,
            IReceiptVoucherQueries receiptVoucherQueries,
            IProjectGrpcService projectGrpcService,
            IMarketAreaGrpcService marketAreaGrpcService,
            IReceiptVoucherRepository receiptVoucherRepository,
            IPaymentVoucherRepository paymentVoucherRepository,
            IPaymentVoucherQueries paymentVoucherQueries,
            IConfigurationSystemParameterGrpcService configurationSystemParameterService,
            IReceiptVoucherService receiptVoucherService,
            IPaymentVoucherService paymentVoucherService,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IAttachmentFileRepository attachmentFileRepository, 
            IExchangeRateGrpcService exchangeRateGrpcService)
        {
            this._clearingRepository = clearingRepository;
            this._voucherTargetQueries = voucherTargetQueries;
            this._receiptVoucherQueries = receiptVoucherQueries;
            this._projectGrpcService = projectGrpcService;
            this._marketAreaGrpcService = marketAreaGrpcService;
            this._receiptVoucherRepository = receiptVoucherRepository;
            this._paymentVoucherRepository = paymentVoucherRepository;
            this._paymentVoucherQueries = paymentVoucherQueries;
            this._configurationSystemParameterService = configurationSystemParameterService;
            this._receiptVoucherService = receiptVoucherService;
            this._paymentVoucherService = paymentVoucherService;
            this._attachmentFileService = attachmentFileService;
            this._attachmentFileRepository = attachmentFileRepository;
            this._exchangeRateGrpcService = exchangeRateGrpcService;
        }

        /// <summary>
        /// Xử lý thêm mới bù trừ
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<ClearingDTO>> Handle(CUClearingCommand request,
            CancellationToken cancellationToken)
        {
            var exchangeRates = await _exchangeRateGrpcService.GetExchangeRatesByNow();
            if ((exchangeRates?.Count ?? 0) == 0)
            {
                await _exchangeRateGrpcService.SynchronizeExchangeRates();
                exchangeRates = await _exchangeRateGrpcService.GetExchangeRatesByNow();
            }

            var commandResponse = new ActionResponse<ClearingDTO>();
            if (_clearingRepository.IsExistByCode(request.CodeClearing))
            {
                commandResponse.AddError("Mã bù trừ đã tồn tại ", nameof(request.CodeClearing));
                return commandResponse;
            }

            if (string.IsNullOrEmpty(request.Id))
            {
                var systemConfiguration = await _configurationSystemParameterService.GetSystemConfigurations();
                double exchangeRate = 0d;
                DateTime exchangeDate = DateTime.UtcNow;

                // Khởi tạo thể hiện của bù trừ
                if (request.CurrencyUnitCode != CurrencyUnit.VND.CurrencyUnitCode
                                   && exchangeRates?.Count > 0)
                {
                    var currentExchangeRate = exchangeRates
                        .Find(e => e.CurrencyCode.Equals(request.CurrencyUnitCode, StringComparison.OrdinalIgnoreCase));
                    exchangeRate = currentExchangeRate.TransferValue;
                    exchangeDate = currentExchangeRate.CreatedDate;
                }
                else
                {
                    exchangeRate = 1;
                    exchangeDate = DateTime.UtcNow.AddHours(7);
                }

                var voucherTarget = _voucherTargetQueries.Find(request.TargetId);

                if (voucherTarget == null)
                {
                    commandResponse.AddError("Khách hàng bù trừ không tồn tại ", nameof(request.TargetId));
                    return commandResponse;
                }

                var clearingEntity = new Clearing(request)
                {
                    CreatedBy = request.CreatedUserId,
                    StatusId = ClearingStatus.Pending.Id,
                    ExchangeRate = exchangeRate,
                    ExchangeRateApplyDate = exchangeDate
                };

                commandResponse.CombineResponse(await _clearingRepository.CreateAndSave(clearingEntity));

                if (!commandResponse.IsSuccess)
                {
                    throw new DebtDomainException(commandResponse.Message);
                }

                if (request.AttachmentFiles != null && request.AttachmentFiles.Any())
                {
                    var attachmentFiles =
                        await _attachmentFileService.PersistentFiles(request.AttachmentFiles.Select(c => c.TemporaryUrl)
                        .ToArray());

                    if (attachmentFiles == null || !attachmentFiles.Any())
                        throw new DebtDomainException("An error has occured when save the attachments");

                    foreach (var fileCommand in attachmentFiles)
                    {
                        fileCommand.CreatedBy = request.CreatedBy;
                        fileCommand.ClearingVoucherId = clearingEntity.Id;
                        var savedFileRsp = await _attachmentFileRepository.CreateAndSave(fileCommand);
                        if (!savedFileRsp.IsSuccess) throw new DebtDomainException(savedFileRsp.Message);
                    }
                }

                var receiptVouchers = await _receiptVoucherRepository.GetByIdsAsync(request.SelectionReceiptIds.ToArray());

                foreach(var receipt in receiptVouchers)
                {
                    if (receipt.StatusId == ReceiptVoucherStatus.Pending.Id)
                    {
                        receipt.Payment.Method = 2; // Bù trừ
                        receipt.ClearingId = clearingEntity.Id;
                        receipt.ClearingTotal = receipt.GrandTotal;
                        receipt.UpdatedBy = request.UpdatedBy;
                        receipt.UpdatedDate = DateTime.Now;
                        receipt.IsLock = true;
                        receipt.CalculateTotal();
                        commandResponse.CombineResponse(_receiptVoucherRepository.Update(receipt));
                    }
                }

                if (!commandResponse.IsSuccess)
                {
                    throw new DebtDomainException(commandResponse.Message);
                }

                var paymentVouchers = await _paymentVoucherRepository.GetByIdsAsync(request.SelectionPaymentIds.ToArray());

                foreach (var payment in paymentVouchers)
                {
                    if (payment.StatusId == PaymentVoucherStatus.New.Id)
                    {
                        payment.Payment.Method = 2; // Bù trừ
                        payment.ClearingId = clearingEntity.Id;
                        payment.ClearingTotal = payment.GrandTotal;
                        payment.UpdatedBy = request.UpdatedBy;
                        payment.UpdatedDate = DateTime.Now;
                        payment.IsLock = true;
                        payment.CalculateTotal();
                        commandResponse.CombineResponse(_paymentVoucherRepository.Update(payment));
                    }
                }

                if (!commandResponse.IsSuccess)
                {
                    throw new DebtDomainException(commandResponse.Message);
                }


                string marketAreaCode = await _marketAreaGrpcService.GetMarketAreaCode(clearingEntity.MarketAreaId);
                string marketAreaPart = !string.IsNullOrEmpty(marketAreaCode) ? marketAreaCode + "/" : "";

                if (clearingEntity.TotalReceipt > clearingEntity.TotalPayment)
                {
                    var firstVoucher = receiptVouchers[0];
                    var clearingRemainingTotal = clearingEntity.TotalReceipt - clearingEntity.TotalPayment;

                    var newReceiptVoucher = new ReceiptVoucher()
                    {
                        IdentityGuid = Guid.NewGuid().ToString(),
                        TargetId = voucherTarget.Id,
                        ClearingId = clearingEntity.Id,
                        CreatedDate = DateTime.Now,
                        CreatedBy = "Hệ thống",
                        Source = Domain.Commands.CommandSource.CMS.ToString(),
                        IsActive = true,
                        MarketAreaId = clearingEntity.MarketAreaId,
                        MarketAreaName = clearingEntity.MarketAreaName,
                        TypeId = ReceiptVoucherType.Clearing.Id,
                        Payment = new PaymentMethod(
                            firstVoucher.Payment.Form,
                            firstVoucher.Payment.Method,
                            firstVoucher.Payment.Address),
                        IssuedDate = DateTime.Now,
                        IsEnterprise = firstVoucher.IsEnterprise,
                        GrandTotal = clearingRemainingTotal,
                        SubTotal = clearingRemainingTotal,
                        PaidTotal = 0,
                        CashTotal = 0,
                        RemainingTotal = clearingRemainingTotal,
                        GrandTotalBeforeTax = clearingRemainingTotal,
                        Content = $"Thanh toán khoản dư cho biên bản bù trừ số: {clearingEntity.CodeClearing}",
                        ExchangeRate = exchangeRate,
                        ExchangeRateApplyDate = exchangeDate,
                        NumberBillingLimitDays = systemConfiguration.NumberDaysOverdue ?? 30,
                        CurrencyUnitCode = request.CurrencyUnitCode,
                        CurrencyUnitId = request.CurrencyUnitId,
                        CreatedUserId = request.CreatedUserId,
                        IsLock = true
                    };

                    newReceiptVoucher.VoucherCode = await GetVoucherCode(newReceiptVoucher.IssuedDate, 
                        newReceiptVoucher.ProjectId, 
                        request.MarketAreaId, 
                        voucherTarget.IsEnterprise);


                    newReceiptVoucher.SetStatusId(ReceiptVoucherStatus.Pending.Id);
                    newReceiptVoucher.UpdateStatusOverdue();
                    newReceiptVoucher.UpdateStatusBadDebt(systemConfiguration.NumberDaysBadDebt ?? 60);

                    newReceiptVoucher.AddReceiptVoucherDetail(new CUReceiptVoucherDetailCommand()
                    {
                        OtherFeeTotal = newReceiptVoucher.OtherFee,
                        GrandTotal = newReceiptVoucher.GrandTotal,
                        SubTotal = newReceiptVoucher.SubTotal
                    });

                    commandResponse.CombineResponse(_receiptVoucherRepository.Create(newReceiptVoucher));
                }
                else if (clearingEntity.TotalPayment > clearingEntity.TotalReceipt)
                {
                    var firstVoucher = _paymentVoucherQueries.Find(request.SelectionPaymentIds[0]);
                    var clearingRemainingTotal = clearingEntity.TotalPayment - clearingEntity.TotalReceipt;
                    var newPaymentVoucher = new PaymentVoucher()
                    {
                        IdentityGuid = Guid.NewGuid().ToString(),
                        TargetId = voucherTarget.Id,
                        ClearingId = clearingEntity.Id,
                        CreatedDate = DateTime.Now,
                        CreatedBy = "Hệ thống",
                        Source = Domain.Commands.CommandSource.CMS.ToString(),
                        IsActive = true,
                        MarketAreaId = clearingEntity.MarketAreaId,
                        MarketAreaName = clearingEntity.MarketAreaName,
                        TypeId = PaymentVoucherType.Clearing.Id,
                        Payment = new PaymentMethod(
                            firstVoucher.Payment.Form,
                            firstVoucher.Payment.Method,
                            firstVoucher.Payment.Address),
                        IssuedDate = DateTime.Now,
                        IsEnterprise = firstVoucher.IsEnterprise,
                        GrandTotal = clearingRemainingTotal,
                        SubTotal = clearingRemainingTotal,
                        PaidTotal = 0,
                        CashTotal = 0,
                        RemainingTotal = clearingRemainingTotal,
                        GrandTotalBeforeTax = clearingRemainingTotal,
                        Content = $"Thanh toán khoản dư cho biên bản bù trừ số: {clearingEntity.CodeClearing}",
                        ExchangeRate = exchangeRate,
                        ExchangeRateApplyDate = exchangeDate,
                        CurrencyUnitCode = request.CurrencyUnitCode,
                        CurrencyUnitId = request.CurrencyUnitId,
                        CreatedUserId = request.CreatedUserId,
                        IsLock = true
                    };

                    newPaymentVoucher.VoucherCode = await _paymentVoucherService
                        .GeneratePaymentVoucherCode(newPaymentVoucher.ProjectId, newPaymentVoucher.MarketAreaId);

                    newPaymentVoucher.SetStatusId(PaymentVoucherStatus.New.Id);

                    newPaymentVoucher.AddPaymentVoucherDetail(new CUPaymentVoucherDetailCommand()
                    {
                        GrandTotal = newPaymentVoucher.GrandTotal,
                        SubTotal = newPaymentVoucher.SubTotal
                    });

                    commandResponse.CombineResponse(_paymentVoucherRepository.Create(newPaymentVoucher));
                }

                if (!commandResponse.IsSuccess)
                {
                    throw new DebtDomainException(commandResponse.Message);
                }

                await _clearingRepository.SaveChangeAsync();
            }

            return commandResponse;
        }
        private async Task<string> GetVoucherCode(DateTime issuedDate, int? projectId, int? marketAreaId, bool isEnterprise = false, int tryTimes = 1)
        {

            var generatedCode = await _receiptVoucherService
                        .GenerateReceiptVoucherCode(issuedDate, projectId, marketAreaId, isEnterprise);

            if (_receiptVoucherRepository.IsExistByCode(generatedCode))
            {
                if (tryTimes > TRY_GENERATE_NEW_VOUCHER_CODE_TIMES)
                {
                    return $"{generatedCode}/{issuedDate.Ticks}";
                }
                return await GetVoucherCode(issuedDate, projectId, marketAreaId, isEnterprise, tryTimes + 1);
            }

            return generatedCode;
        }
    }
}
