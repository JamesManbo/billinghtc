using AutoMapper;
using DebtManagement.BackgroundTasks.Services.Grpc;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.BackgroundTasks.Tasks
{
    public class BadDebtScannerTask : BackgroundService
    {
        private readonly ILogger<BadDebtScannerTask> _logger;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IConfigurationSystemParameterGrpcService _configurationSystemParamGrpcService;
        private readonly INotificationGrpcService _notificationGrpcService;
        private bool IsDelayForFirstTime = true;


        public BadDebtScannerTask(
            ILogger<BadDebtScannerTask> logger,
            IReceiptVoucherRepository receiptVoucherRepository,
            IConfigurationSystemParameterGrpcService configurationSystemParamGrpcService,
            INotificationGrpcService notificationGrpcService)
        {
            this._logger = logger;
            this._receiptVoucherRepository = receiptVoucherRepository;
            this._configurationSystemParamGrpcService = configurationSystemParamGrpcService;
            this._notificationGrpcService = notificationGrpcService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var exitBehavior = BadDebtScannerExitBehavior.Done;
                if (IsDelayForFirstTime)
                {
                    exitBehavior = BadDebtScannerExitBehavior.Delay;
                    goto Finish;
                }
                var dueReceiptVouchers = await _receiptVoucherRepository.GetDueVouchers();
                var overdueReceiptVouchers = await _receiptVoucherRepository.GetOverdueVouchers();
                var systemConfiguration = await _configurationSystemParamGrpcService.GetSystemConfigurations();

                try
                {
                    // Chuyển các phiếu thu đã quá ngày thanh toán sang trạng thái quá hạn
                    if (dueReceiptVouchers != null && dueReceiptVouchers.Any())
                    {
                        foreach (var voucher in dueReceiptVouchers)
                        {
                            voucher.SetStatusId(ReceiptVoucherStatus.Overdue.Id);
                            // Tính số tuổi nợ(ngày)
                            var overdueDays = DateTime.UtcNow.AddHours(7)
                                .Subtract(voucher.IssuedDate.AddDays(voucher.NumberBillingLimitDays));
                            voucher.NumberDaysOverdue = overdueDays.Days;
                        }
                        _receiptVoucherRepository.UpdateRange(dueReceiptVouchers);
                        await _receiptVoucherRepository.SaveChangeAsync();
                    }

                    // Chuyển các phiếu thu có tuổi nợ quá hạn cho phép thành nợ xấu
                    if (overdueReceiptVouchers != null && overdueReceiptVouchers.Any())
                    {
                        foreach (var voucher in overdueReceiptVouchers)
                        {
                            // Tuổi nợ cho phép của phiếu thu(ngày)
                            var debtAge = systemConfiguration.NumberDaysOverdue ?? 60;

                            // Tính số tuổi nợ(ngày)
                            var overdueDays = DateTime.UtcNow.AddHours(7)
                                .Subtract(voucher.IssuedDate.AddDays(voucher.NumberBillingLimitDays));

                            voucher.NumberDaysOverdue = overdueDays.Days > 0 ? overdueDays.Days : 0;
                            
                            // Nếu phiếu thu của k/h cá nhân có tuổi nợ vượt quá số ngày cho phép, chuyển nợ thành nợ xấu
                            if (voucher.NumberDaysOverdue >= systemConfiguration.NumberDaysBadDebt 
                                && !voucher.IsEnterprise)
                            {
                                voucher.SetStatusId(ReceiptVoucherStatus.BadDebt.Id);
                            }
                        }
                        _receiptVoucherRepository.UpdateRange(overdueReceiptVouchers);
                        await _receiptVoucherRepository.SaveChangeAsync();
                    }
                    else
                    {
                        goto Finish;
                    }


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    exitBehavior = BadDebtScannerExitBehavior.Retry;
                    goto Finish;
                }

                Finish:
                switch (exitBehavior)
                {
                    case BadDebtScannerExitBehavior.Delay:
                        this.IsDelayForFirstTime = false;
                        await Task.Delay(5 * 60 * 1000, stoppingToken);
                        break;
                    case BadDebtScannerExitBehavior.Done:
                        await Task.Delay(15 * 60 * 1000, stoppingToken);
                        break;
                    case BadDebtScannerExitBehavior.Retry:
                        var nextRunningTime = DateTime.Now.AddDays(1).AddHours(1);
                        await Task.Delay(nextRunningTime - DateTime.Now, stoppingToken);
                        break;
                    default:
                        await Task.Delay(30 * 60 * 1000, stoppingToken);
                        break;
                }
            }
        }
    }

    public enum BadDebtScannerExitBehavior
    {
        Delay,
        Retry,
        Done,
        Unscheduleed
    }
}
