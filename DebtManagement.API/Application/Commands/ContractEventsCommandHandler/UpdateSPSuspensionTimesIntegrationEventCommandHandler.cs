using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.Domain.Commands.ContractEventsCommand;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.ContractEventsCommandHandler
{
    public class UpdateSPSuspensionTimesIntegrationEventCommandHandler : IRequestHandler<UpdateSPSuspensionTimesIntegrationEventCommand, ActionResponse>
    {
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;
        public UpdateSPSuspensionTimesIntegrationEventCommandHandler(IDebtIntegrationEventService debtIntegrationEventService)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
        }
        public async Task<ActionResponse> Handle(UpdateSPSuspensionTimesIntegrationEventCommand request, CancellationToken cancellationToken)
        {
            var uSPSTIE = new UpdateServicePackageSuspensionTimesIntegrationEvent(request.ServicePackageSuspensionTimeEvents);
            await _debtIntegrationEventService.AddAndSaveEventAsync(uSPSTIE);
            return new ActionResponse();
        }
    }
}
