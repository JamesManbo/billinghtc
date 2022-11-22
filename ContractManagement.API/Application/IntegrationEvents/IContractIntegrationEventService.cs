using System;
using System.Threading.Tasks;
using EventBus.Events;

namespace ContractManagement.API.Application.IntegrationEvents
{
    public interface IContractIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
