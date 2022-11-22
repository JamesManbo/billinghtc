using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents;
using ContractManagement.Domain.Events.ContractEvents;
using GenericRepository.Configurations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.DomainEventHandlers.ContractEventHandlers
{
    public class TerminateServicePackagesDomainEventHandler : INotificationHandler<TerminateServicePackagesDomainEvent>
    {
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IWrappedConfigAndMapper _configAndMapperWapper;
        public TerminateServicePackagesDomainEventHandler(IContractIntegrationEventService integrationEventLogService, IWrappedConfigAndMapper configAndMapperWapper)
        {
            _integrationEventLogService = integrationEventLogService;
            _configAndMapperWapper = configAndMapperWapper;
        }

        public async Task Handle(TerminateServicePackagesDomainEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null || !notification.OutContractServicePackageIds.Any())
            {
                return;
            }

            var terminateServicePackagesIntegrationEvent = new TerminateServicePackagesIntegrationEvent()
            {
                OutContractServicePackageIds = notification.OutContractServicePackageIds
            };
            await _integrationEventLogService.AddAndSaveEventAsync(terminateServicePackagesIntegrationEvent);
        }
    }
}
