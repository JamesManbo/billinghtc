using AutoMapper;
using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events.InContractEvents;
using DebtManagement.Domain.Events.InContractEvents;
using DebtManagement.Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.PaymentVoucherHandlers
{
    public class PaymentVoucherCreatedEventHandler : INotificationHandler<PaymentVoucherCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;

        public PaymentVoucherCreatedEventHandler(IDebtIntegrationEventService debtIntegrationEventService,
            IMapper mapper)
        {
            this._debtIntegrationEventService = debtIntegrationEventService;
            this._mapper = mapper;
        }

        public async Task Handle(PaymentVoucherCreatedEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new PaymentVoucherCreatedIntegrationEvent
            {
                InContractId = notification.InContractId,
                GrandTotal = notification.GrandTotal,
                TargetId = notification.TargetId,
                VoucherDetails = _mapper.Map<List<PaymentVoucherDetailDTO>>(notification.VoucherDetails),
                NextBillingDate = notification.NextBillingDate
            };

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
