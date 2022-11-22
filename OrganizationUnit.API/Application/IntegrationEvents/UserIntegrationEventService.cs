using System;
using System.Data.Common;
using System.Threading.Tasks;
using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrganizationUnit.Infrastructure;

namespace OrganizationUnit.API.Application.IntegrationEvents
{
    public class UserIntegrationEventService : IUserIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly OrganizationUnitDbContext _contractContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<UserIntegrationEventService> _logger;

        public UserIntegrationEventService(
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            IEventBus eventBus,
            OrganizationUnitDbContext contractContext,
            ILogger<UserIntegrationEventService> logger)
        {
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ??
                                                 throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _contractContext = contractContext ?? throw new ArgumentNullException(nameof(contractContext));
            _eventLogService = _integrationEventLogServiceFactory(_contractContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvent in pendingLogEvents)
            {
                _logger.LogInformation(
                    "----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent}",
                    logEvent.EventId, Program.AppName, logEvent.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvent.EventId);
                    _eventBus.Publish(logEvent.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvent.EventId);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "ERROR publishing integration event: {IntegrationEventId} from {AppName}",
                        logEvent.EventId, Program.AppName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvent.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- En-queuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventLogService.SaveEventAsync(evt, _contractContext.GetCurrentTransaction());
        }
    }
}