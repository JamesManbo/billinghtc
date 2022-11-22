using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events.ContractEvents;
using DebtManagement.Domain.Commands.DebtCommand;
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
    public class BillingPaymentCanceledEventHandler : INotificationHandler<BillingPaymentCanceledEvent>
    {
        private readonly IMediator _mediator;
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;
        private readonly IWrappedConfigAndMapper _wrapperMapperAndConfig;

        public BillingPaymentCanceledEventHandler(IDebtIntegrationEventService debtIntegrationEventService,
            IWrappedConfigAndMapper wrapperMapperAndConfig,
            IMediator mediator)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
            _wrapperMapperAndConfig = wrapperMapperAndConfig;
            this._mediator = mediator;
        }

        public async Task Handle(BillingPaymentCanceledEvent notification, CancellationToken cancellationToken)
        {
            if (notification.VoucherDetails == null || notification.VoucherDetails.Count() == 0)
                return;


            var decreaseDebtCmd = new UpdateCurrentDebtOfTargetCommand()
            {
                TargetId = notification.TargetId,
                DebtAmount = notification.GrandTotal,
                Increase = false
            };

            await this._mediator.Send(decreaseDebtCmd);

            var integrationEvent = new BillingPaymentCanceledIntegrationEvent
            {
                IsActiveSPST = notification.IsActiveSPST,
                IsFirstVoucherOfContract = notification.IsFirstVoucherOfContract,
                OutContractId = notification.OutContractId
            };
            foreach (var vchrDetail in notification.VoucherDetails)
            {
                integrationEvent.VoucherDetails.Add(vchrDetail.MapTo<ReceiptVoucherDetailDTO>(_wrapperMapperAndConfig.MapperConfig));
            }

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
