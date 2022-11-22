using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Events.ContractEvents;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.DomainEventHandlers.ContractEventHandlers
{
    public class UpdateTimelineReceiptVoucherDomainEventHandler : INotificationHandler<UpdateTimelineReceiptVoucherDomainEvent>
    {
        private readonly IOutContractQueries _outContractQueries;
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IWrappedConfigAndMapper _configAndMapperWapper;

        public UpdateTimelineReceiptVoucherDomainEventHandler(IContractIntegrationEventService integrationEventLogService,
            IOutContractQueries outContractQueries,
            IWrappedConfigAndMapper configAndMapperWapper)
        {
            _integrationEventLogService = integrationEventLogService;
            _outContractQueries = outContractQueries;
            _configAndMapperWapper = configAndMapperWapper;
        }

        public async Task Handle(UpdateTimelineReceiptVoucherDomainEvent notification, CancellationToken cancellationToken)
        {
            #region Update first receipt voucher start, end billing date

            var outContractStatus = _outContractQueries.FindStatusById(notification.ContractId);
            if (outContractStatus == ContractStatus.Signed.Id)
            {
                var integrationEvent = new UpdateTimelineReceiptVoucherIntegrationEvent
                {
                    ContractId = notification.ContractId,
                    OutServicePackageId = notification.OutServicePackageId,
                    TimeLine = notification.TimeLine
                };

                await _integrationEventLogService.AddAndSaveEventAsync(integrationEvent);
            }

            #endregion
        }
    }
}
