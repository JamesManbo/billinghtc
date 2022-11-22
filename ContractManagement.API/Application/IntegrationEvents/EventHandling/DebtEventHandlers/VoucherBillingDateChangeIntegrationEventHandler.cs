using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Receipts;
using ContractManagement.Domain.Commands.DebtCommand;
using EventBus.Abstractions;
using MediatR;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers
{
    public class VoucherBillingDateChangeIntegrationEventHandler
        : IIntegrationEventHandler<VoucherBillingDateChangeIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public VoucherBillingDateChangeIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(VoucherBillingDateChangeIntegrationEvent @event)
        {
            var updateNextBillingCmd = new UpdateChannelNextBillingCommand()
            {
                ChannelId = @event.ChannelId,
                NewEndingBillingDate = @event.NewEndingBillingDate,
                OldEndingBillingDate = @event.OldEndingBillingDate
            };

            await _mediator.Send(updateNextBillingCmd);
        }
    }
}
