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
    public class UpdateContractorPropertyDomainEventHandler : INotificationHandler<UpdateContractorPropertyDomainEvent>
    {
        private readonly IApplicationUserIntegrationEventService _integrationService;

        public UpdateContractorPropertyDomainEventHandler(IApplicationUserIntegrationEventService integrationService)
        {
            this._integrationService = integrationService;
        }

        public async Task Handle(UpdateContractorPropertyDomainEvent notification, CancellationToken cancellationToken)
        {
            await this._integrationService.AddAndSaveEventAsync(
                new UpdateContractorPropertyIntegrationEvent(notification)
                );

            await _integrationService.PublishEventsThroughEventBusAsync(notification.TransactionId);
        }
    }
}
