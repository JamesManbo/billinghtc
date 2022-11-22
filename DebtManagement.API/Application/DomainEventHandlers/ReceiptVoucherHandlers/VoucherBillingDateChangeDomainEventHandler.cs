using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers
{
    public class VoucherBillingDateChangeDomainEventHandler
        : INotificationHandler<VoucherBillingDateChangeDomainEvent>
    {
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;

        public VoucherBillingDateChangeDomainEventHandler(IDebtIntegrationEventService debtIntegrationEventService)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
        }
        public async Task Handle(VoucherBillingDateChangeDomainEvent notification, CancellationToken cancellationToken)
        {
            if (notification.ChannelId == 0)
                return;

            var integrationEvent = new VoucherBillingDateChangeIntegrationEvent(notification);

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
