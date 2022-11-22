using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ExchangeRateRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContractManagement.BackgroundTasks.Tasks
{
    public class ExchangeRateManagementTask : BackgroundService
    {
        private readonly ILogger<ExchangeRateManagementTask> _logger;
        private bool IsDelayForFirstTime = true;
        public IServiceScopeFactory _serviceScopeFactory;
        public ExchangeRateManagementTask(ILogger<ExchangeRateManagementTask> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var exchangeRateRepository = scope.ServiceProvider.GetRequiredService<IExchangeRateRepository>();
                    var exchangeRateQueries = scope.ServiceProvider.GetRequiredService<IExchangeRateQueries>();

                    if (IsDelayForFirstTime)
                    {
                        this.IsDelayForFirstTime = false;
                        goto Finish;
                    }

                    _logger.LogInformation("ExchangeRate running at: {time}", DateTimeOffset.Now);

                    var listExist = exchangeRateQueries.GetAllInDate(DateTime.Now);
                    if (listExist != null && listExist.Count() > 0)
                    {
                        await Task.Delay(CalculateDelayTime(), stoppingToken);
                        goto Finish;
                    }

                    if (await exchangeRateRepository.SyncExchangeRate())
                    {
                        await Task.Delay(CalculateDelayTime(), stoppingToken);
                        goto Finish;
                    }

                    await Task.Delay(3 * 60 * 1000, stoppingToken);

                Finish:
                    await Task.Delay(5 * 60 * 1000, stoppingToken);
                }
            }
        }

        private TimeSpan CalculateDelayTime()
        {
            var currentDatetime = DateTime.Now.Date;
            var tomorrow = currentDatetime.AddDays(1);
            var span = tomorrow.Date.AddHours(1) - DateTime.Now;
            _logger.LogInformation("ExchangeRate CalculateDelay: {time}", span);
            return span;
        }
    }
}
