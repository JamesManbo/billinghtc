using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events.ContractEvents;
using DebtManagement.Domain.Events.ContractEvents;
using DebtManagement.Domain.Models;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers
{
    public class BillingPaymentSuccessEventHandler : INotificationHandler<BillingPaymentSuccessEvent>
    {

        private readonly IDebtIntegrationEventService _debtIntegrationEventService;
        private readonly IWrappedConfigAndMapper _wrapperMapperAndConfig;

        public BillingPaymentSuccessEventHandler(IDebtIntegrationEventService debtIntegrationEventService, IWrappedConfigAndMapper wrapperMapperAndConfig)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
            _wrapperMapperAndConfig = wrapperMapperAndConfig;
        }

        public async Task Handle(BillingPaymentSuccessEvent notification, CancellationToken cancellationToken)
        {
            if (notification.VoucherDetails == null || notification.VoucherDetails.Count() == 0)
                return;

            var integrationEvent = new BillingPaymentSuccessIntegrationEvent();
            integrationEvent.IsActiveSPST = notification.IsActiveSPST;
            integrationEvent.OutContractId = notification.OutContractId;
            integrationEvent.IsFirstVoucherOfContract = notification.IsFirstVoucherOfContract;
            integrationEvent.Promotions = notification.Promotions;
            foreach (var vchrDetail in notification.VoucherDetails)
            {
                integrationEvent.VoucherDetails.Add(vchrDetail.MapTo<ReceiptVoucherDetailDTO>(_wrapperMapperAndConfig.MapperConfig));
            }

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
