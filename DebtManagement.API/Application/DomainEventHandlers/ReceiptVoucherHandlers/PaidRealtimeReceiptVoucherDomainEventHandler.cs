using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PaidRealtimeServicePackage = DebtManagement.API.Application.IntegrationEvents.Events.PaidRealtimeServicePackage;

namespace DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers
{
    public class PaidRealtimeReceiptVoucherDomainEventHandler : INotificationHandler<PaidRealtimeReceiptVoucherDomainEvent>
    {
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;

        public PaidRealtimeReceiptVoucherDomainEventHandler(IDebtIntegrationEventService debtIntegrationEventService)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
        }

        public async Task Handle(PaidRealtimeReceiptVoucherDomainEvent notification, CancellationToken cancellationToken)
        {
            if (notification.PaidRealtimeServicePackages == null || notification.PaidRealtimeServicePackages.Count() == 0)
                return;

            var integrationEvent = new PaidRealtimeReceiptVoucherIntegrationEvent();
            foreach (var vchrDetail in notification.PaidRealtimeServicePackages)
            {
                integrationEvent.PaidRealtimeServicePackages.Add(new PaidRealtimeServicePackage()
                {
                    EndDate = vchrDetail.EndDate,
                    StartDate = vchrDetail.StartDate,
                    OutContractServicePackageId = vchrDetail.OutContractServicePackageId
                });
            }

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);

        }
    }
}
