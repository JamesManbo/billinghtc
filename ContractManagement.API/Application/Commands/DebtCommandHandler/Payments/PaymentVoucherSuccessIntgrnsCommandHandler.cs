using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler.Payments
{
    public class PaymentVoucherSuccessIntgrnsCommandHandler : IRequestHandler<PaymentVoucherSuccessIntegrationEvent>
    {
        public Task<Unit> Handle(PaymentVoucherSuccessIntegrationEvent request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
