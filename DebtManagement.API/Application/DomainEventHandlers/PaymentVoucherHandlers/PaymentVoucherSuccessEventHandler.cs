using AutoMapper;
using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events.InContractEvents;
using DebtManagement.Domain.Events.InContractEvents;
using DebtManagement.Domain.Models;
using GenericRepository.Configurations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.PaymentVoucherHandlers
{
    public class PaymentVoucherSuccessEventHandler : INotificationHandler<PaymentVoucherSuccessEvent>
    {
        private readonly IMapper _mapper;
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;

        public PaymentVoucherSuccessEventHandler(IDebtIntegrationEventService debtIntegrationEventService,
            IMapper mapper)
        {
            this._debtIntegrationEventService = debtIntegrationEventService;
            this._mapper = mapper;
        }

        public async Task Handle(PaymentVoucherSuccessEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new PaymentVoucherSuccessIntegrationEvent
            {
                InContractId = notification.InContractId,
                GrandTotal = notification.GrandTotal,
                TargetId = notification.TargetId,
                VoucherDetails = _mapper.Map<List<PaymentVoucherDetailDTO>>(notification.VoucherDetails)
            };

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
