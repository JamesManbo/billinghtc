using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.Events.ContractEvents;
using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Events.ContractEvents;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers
{
    public class BillingPaymentPendingEventHandler : INotificationHandler<BillingPaymentPendingEvent>
    {
        private readonly IMediator _mediator;
        private readonly IDebtIntegrationEventService _debtIntegrationEventService;
        private readonly IWrappedConfigAndMapper _wrapperMapperAndConfig;

        public BillingPaymentPendingEventHandler(IDebtIntegrationEventService debtIntegrationEventService,
            IWrappedConfigAndMapper wrapperMapperAndConfig,
            IMediator mediator)
        {
            _debtIntegrationEventService = debtIntegrationEventService;
            _wrapperMapperAndConfig = wrapperMapperAndConfig;
            this._mediator = mediator;
        }

        public async Task Handle(BillingPaymentPendingEvent notification, CancellationToken cancellationToken)
        {
            var increaseDebtCmd = new UpdateCurrentDebtOfTargetCommand()
            {
                TargetId = notification.TargetId,
                DebtAmount = notification.GrandTotal
            };
            await this._mediator.Send(increaseDebtCmd);
            var integrationEvent = new BillingPaymentPendingIntegrationEvent
            {
                IsActiveSPST = notification.IsActiveSPST
            };

            foreach (var vchrDetail in notification.VoucherDetails)
            {
                integrationEvent.VoucherDetails.Add(vchrDetail.MapTo<ReceiptVoucherDetailDTO>(_wrapperMapperAndConfig.MapperConfig));
            }
            foreach (var item in notification.Promotions)
            {
                integrationEvent.Promotions.Add(item);
            }

            await _debtIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        }
    }
}
