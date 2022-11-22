using DebtManagement.BackgroundTasks.Services.Grpc;
using DebtManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.BackgroundTasks.Tasks
{
    public class UpdateOverdueAndBadDebtTask : BackgroundService
    {
        private readonly ILogger<UpdateOverdueAndBadDebtTask> _logger;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IConfigurationSystemParameterGrpcService _configurationSystemParameterService;
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;

        public UpdateOverdueAndBadDebtTask(ILogger<UpdateOverdueAndBadDebtTask> logger,
            IReceiptVoucherRepository receiptVoucherRepository,
            IConfigurationSystemParameterGrpcService configurationSystemParameterService, 
            IPaymentVoucherRepository paymentVoucherRepository)
        {
            _logger = logger;
            _receiptVoucherRepository = receiptVoucherRepository;
            _configurationSystemParameterService = configurationSystemParameterService;
            _paymentVoucherRepository = paymentVoucherRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.UtcNow.AddHours(7).Hour == 0)
                    {
                        _receiptVoucherRepository.ReceiptVouchersUpdateOverdueAndBadDebt(await _configurationSystemParameterService.GetNumberDaysBadDebt());
                        _logger.LogInformation("Update overdue and bad debt for ReceiptVouchers on {0}", DateTime.Now.ToString("dd/MM/yyyy"));
                        _paymentVoucherRepository.PaymentVouchersUpdateOverdue();
                        _logger.LogInformation("Update overdue for PaymentVouchers on {0}", DateTime.Now.ToString("dd/MM/yyyy"));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    await Task.Delay(60 * 60 * 1000, stoppingToken);
                }

            }
        }
    }
}
