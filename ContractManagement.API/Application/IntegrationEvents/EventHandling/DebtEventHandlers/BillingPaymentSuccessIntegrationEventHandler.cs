using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.Domain.Commands.DebtCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Utility;
using EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers
{
    public class BillingPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<BillingPaymentSuccessIntegrationEvent>
    {
        private readonly IMediator _mediator;
        public BillingPaymentSuccessIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(BillingPaymentSuccessIntegrationEvent @event)
        {
            if (@event.VoucherDetails == null || @event.VoucherDetails.Count == 0) return;
            
            var updateOCSPNextBillingCommand
                = new UpdateContractAfterPaymentCommand()
                {
                    IsActiveSPST = @event.IsActiveSPST,
                    IsFirstVoucherOfContract = @event.IsFirstVoucherOfContract,
                    OutContractId = @event.OutContractId,
                    VoucherDetails = @event.VoucherDetails,
                    Promotions = @event.Promotions
                };
            var commandResponse = await _mediator.Send(updateOCSPNextBillingCommand);
            if (!commandResponse.IsSuccess)
            {
                throw new ContractDomainException(commandResponse.Message);
            }
        }
    }
}
