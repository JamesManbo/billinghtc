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
    public class UpdateOCSPClearingIntegrationEventCommandHandler : IRequestHandler<UpdateOCSPClearingIntegrationEventCommand, ActionResponse>
    {
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;

        public UpdateOCSPClearingIntegrationEventCommandHandler(IDebtIntegrationEventService debtIntegrationEventService)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
        }

        public async Task<ActionResponse> Handle(UpdateOCSPClearingIntegrationEventCommand request, CancellationToken cancellationToken)
        {
            var uOCSPCIE = new UpdateOutContractServicePackageClearingIntegrationEvent(request.OutContractServicePackageClearingIntegrationEvents);
            await _debtIntegrationEventService.AddAndSaveEventAsync(uOCSPCIE);
            return new ActionResponse();
        }
    }
}
