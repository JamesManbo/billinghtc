using ContractManagement.BackgroundTasks.Services.Grpc;
using ContractManagement.Infrastructure.Queries;
using Global.Configs.SystemArgument;
using Global.Models.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.BackgroundTasks.Tasks
{
    public class NotifyChannelToRenewalPeriodTask : BackgroundService
    {
        private readonly ILogger<NotifyChannelToRenewalPeriodTask> _logger;
        private readonly IConfiguration _config;
        private bool IsDelayForFirstTime = true;
        public IServiceScopeFactory _serviceScopeFactory;

        public NotifyChannelToRenewalPeriodTask(
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration config,
            ILogger<NotifyChannelToRenewalPeriodTask> logger)
        {
            this._serviceScopeFactory = serviceScopeFactory;
            this._config = config;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var exitTaskFlag = ChannelRechargeExpirationBehavior.Done;
                    if (IsDelayForFirstTime)
                    {
                        exitTaskFlag = ChannelRechargeExpirationBehavior.Delay;
                        goto Finish;
                    }

                    var contractQueries = scope.ServiceProvider.GetRequiredService<IOutContractQueries>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationGrpcService>();
                    var systemConfigurationGrpcService = scope.ServiceProvider.GetRequiredService<ISystemConfigurationGrpcService>();

                    try
                    {
                        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        var systemConfigs = await systemConfigurationGrpcService.GetSystemConfigurations();
                        if (systemConfigs == null)
                        {
                            exitTaskFlag = ChannelRechargeExpirationBehavior.Retry;
                            goto Finish;
                        }

                        var daysBeforeExpiration = systemConfigs.NotifyChannelExpirationDays;

                        var expireSoonChannels = await contractQueries
                            .CountChannelExpirationSoon(daysBeforeExpiration.Value);

                        if (expireSoonChannels == 0)
                        {
                            exitTaskFlag = ChannelRechargeExpirationBehavior.Retry;
                            goto Finish;
                        }

                        var departmentCodes = _config.GetSection("DepartmentCode");
                        var departmentCode = departmentCodes.Get<DepartmentCode>();

                        var expiringContractNotiTemplate = new Domain.Models.Notification.PushNotificationRequest()
                        {
                            Content = string.Format("Có {0} kênh truyền sắp đến kỳ gia hạn cước",
                                expireSoonChannels),
                            Sender = "Hệ thống",
                            Title = "Thông báo",
                            Type = NotificationType.SystemAlert,
                            Zone = NotificationZone.Contract,
                            Category = NotificationCategory.ChannelRechargeExpiration
                        };

                        await notificationService.PushNotificationByDepartment(expiringContractNotiTemplate, departmentCode.CustomerCareDepartmentCode);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(200, ex, ex.Message);
                        exitTaskFlag = ChannelRechargeExpirationBehavior.Retry;

                        goto Finish;
                    }

                Finish:
                    switch (exitTaskFlag)
                    {
                        case ChannelRechargeExpirationBehavior.Delay:
                            this.IsDelayForFirstTime = false;
                            await Task.Delay(1 * 60 * 1000, stoppingToken);
                            break;
                        case ChannelRechargeExpirationBehavior.Retry:
                            await Task.Delay(5 * 60 * 1000, stoppingToken);
                            break;
                        case ChannelRechargeExpirationBehavior.Done:
                            var nextScheduleDate = DateTime.Now.Date
                                .AddDays(1).AddHours(2);
                            
                            //var nextScheduleDate = DateTime.Now.Date
                            //    .AddMinutes(1);
                            var nextScheduleSpan = nextScheduleDate - DateTime.Now;
                            await Task.Delay(nextScheduleSpan, stoppingToken);
                            break;
                    }
                }
            }
        }
    }

    public enum ChannelRechargeExpirationBehavior
    {
        Delay = 1,
        Retry,
        Unscheduled,
        Done
    }
}
