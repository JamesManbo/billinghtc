using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.BackgroundTasks.Services.Grpc;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using Global.Configs.SystemArgument;
using Global.Models.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContractManagement.BackgroundTasks.Tasks
{
    public class ContractExpirationManagementTask : BackgroundService
    {
        private readonly ILogger<ContractExpirationManagementTask> _logger;
        private readonly IConfiguration _config;
        private bool IsDelayForFirstTime = true;
        public IServiceScopeFactory _serviceScopeFactory;

        public ContractExpirationManagementTask(
            ILogger<ContractExpirationManagementTask> logger,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration config)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var outContractQueries = scope.ServiceProvider.GetRequiredService<IOutContractQueries>();
                    var outContractRepository = scope.ServiceProvider.GetRequiredService<IOutContractRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationGrpcService>();

                    var exitTaskFlag = ContractExpirationManagementBehavior.Done;
                    if (IsDelayForFirstTime)
                    {
                        exitTaskFlag = ContractExpirationManagementBehavior.Delay;
                        goto Finish;
                    }

                    try
                    {
                        await outContractRepository.AutoRenewExpirationContract();
                        var expireSoonContracts = await outContractQueries.CountingContractExpirationSoon(enterpriseOnly: true);

                        if (expireSoonContracts == 0) goto Finish;

                        var departmentCodes = _config.GetSection("DepartmentCode");
                        var departmentCode = departmentCodes.Get<DepartmentCode>();

                        var expiringContractNotiTemplate = new PushNotificationRequest()
                        {
                            Content = string.Format("Có {0} hợp đồng doanh nghiệp chuẩn bị hết hạn", expireSoonContracts),
                            Sender = "Hệ thống",
                            Title = "Thông báo hợp đồng chuẩn bị hết hạn",
                            Type = NotificationType.SystemAlert,
                            Zone = NotificationZone.Contract,
                            Category = NotificationCategory.EnterpriseContractExpiration,
                        };

                        await notificationService.PushNotificationByDepartment(expiringContractNotiTemplate, departmentCode.CustomerCareDepartmentCode);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(200, ex, ex.Message);
                        exitTaskFlag = ContractExpirationManagementBehavior.Retry;

                        goto Finish;
                    }

                Finish:
                    switch (exitTaskFlag)
                    {
                        case ContractExpirationManagementBehavior.Delay:
                            this.IsDelayForFirstTime = false;
                            await Task.Delay(5 * 60 * 1000, stoppingToken);
                            break;
                        case ContractExpirationManagementBehavior.Retry:
                            await Task.Delay(5 * 60 * 1000, stoppingToken);
                            break;
                        case ContractExpirationManagementBehavior.Done:
                            var nextScheduleDate = DateTime
                                .UtcNow.AddHours(7).Date
                                .AddDays(1).AddHours(2);
                            var nextScheduleSpan = nextScheduleDate - DateTime.Now;
                            await Task.Delay(nextScheduleSpan, stoppingToken);
                            break;
                    }
                }
            }
        }
    }

    public enum ContractExpirationManagementBehavior
    {
        Delay,
        Retry,
        Unscheduled,
        Done
    }
}