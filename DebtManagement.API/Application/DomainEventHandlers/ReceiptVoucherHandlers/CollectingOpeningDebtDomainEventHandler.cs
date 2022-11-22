using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers
{
    public class CollectingOpeningDebtDomainEventHandler : INotificationHandler<CollectingOpeningDebtDomainEvent>
    {
        private readonly IMediator _mediator;
        public CollectingOpeningDebtDomainEventHandler(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(CollectingOpeningDebtDomainEvent noti,
            CancellationToken cancellationToken)
        {
            await this._mediator.Send(new CollectingOpeningDebtCommand(noti));
        }
    }
}
