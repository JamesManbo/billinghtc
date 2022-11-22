using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.DebtCommand
{
    public class CollectingOpeningDebtCommand : IRequest<ActionResponse>
    {
        public CollectingOpeningDebtDomainEvent Event { get; set; }

        public CollectingOpeningDebtCommand(CollectingOpeningDebtDomainEvent @event)
        {
            Event = @event;
        }
    }
}
