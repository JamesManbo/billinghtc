using System;
using System.Threading.Tasks;
using EventBus.Events;

namespace OrganizationUnit.API.Application.IntegrationEvents
{
    public interface IUserIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
