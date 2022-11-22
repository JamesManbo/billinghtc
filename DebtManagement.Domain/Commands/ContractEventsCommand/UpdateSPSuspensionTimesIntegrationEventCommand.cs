using DebtManagement.Domain.Events.ContractEvents;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ContractEventsCommand
{
    public class UpdateSPSuspensionTimesIntegrationEventCommand : IRequest<ActionResponse>
    {
        public UpdateSPSuspensionTimesIntegrationEventCommand(List<ServicePackageSuspensionTimeEvent> servicePackageSuspensionTimeEvents)
        {
            ServicePackageSuspensionTimeEvents = servicePackageSuspensionTimeEvents;
        }
        public List<ServicePackageSuspensionTimeEvent> ServicePackageSuspensionTimeEvents { get; set; }
    }
}
