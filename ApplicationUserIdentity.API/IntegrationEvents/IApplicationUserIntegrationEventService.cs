using System;
using System.Threading.Tasks;
using EventBus.Events;

namespace ApplicationUserIdentity.API.IntegrationEvents
{
    public interface IApplicationUserIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
