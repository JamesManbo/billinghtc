using DebtManagement.Domain.Events.ContractEvents;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ContractEventsCommand
{
    public class UpdateOCSPClearingIntegrationEventCommand : IRequest<ActionResponse>
    {
        public UpdateOCSPClearingIntegrationEventCommand(List<OutContractServicePackageClearingIntegrationEvent> outContractServicePackageClearingIntegrationEvents)
        {
            OutContractServicePackageClearingIntegrationEvents = outContractServicePackageClearingIntegrationEvents;
        }
        public List<OutContractServicePackageClearingIntegrationEvent> OutContractServicePackageClearingIntegrationEvents { get; set; }
    }
}
