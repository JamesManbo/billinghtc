using ApplicationUserIdentity.API.IntegrationEvents;
using ApplicationUserIdentity.API.IntegrationEvents.Events;
using ApplicationUserIdentity.API.Models.Commands.ContractorCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.DomainEventHandlers.Contractor
{
    public class RemoveContractorPropertyDomainEventHandler : INotificationHandler<RemoveContractorPropertyDomainEvent>
    {
        private readonly IApplicationUserIntegrationEventService _integrationService;

        public RemoveContractorPropertyDomainEventHandler(IApplicationUserIntegrationEventService integrationService)
        {
            this._integrationService = integrationService;
        }

        public async Task Handle(RemoveContractorPropertyDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new RemoveContractorPropertyIntegrationEvent(notification);
            await this._integrationService.AddAndSaveEventAsync(integrationEvent);
            await this._integrationService.PublishEventsThroughEventBusAsync(notification.TransactionId);
        }
    }
}
