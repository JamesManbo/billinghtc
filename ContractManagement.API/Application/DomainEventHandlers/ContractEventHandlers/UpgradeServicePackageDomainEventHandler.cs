using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents;
using ContractManagement.Domain.Events.ContractEvents;
using ContractManagement.Domain.Models;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.DomainEventHandlers.ContractEventHandlers
{
    public class UpgradeServicePackageDomainEventHandler : INotificationHandler<UpgradeServicePackageDomainEvent>
    {
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IWrappedConfigAndMapper _configAndMapperWapper;
        public UpgradeServicePackageDomainEventHandler(IContractIntegrationEventService integrationEventLogService, IWrappedConfigAndMapper configAndMapperWapper)
        {
            _integrationEventLogService = integrationEventLogService;
            _configAndMapperWapper = configAndMapperWapper;
        }

        public async Task Handle(UpgradeServicePackageDomainEvent notification, CancellationToken cancellationToken)
        {
            var upgradeServicePackageIntegrationEvent = new UpgradeServicePackageIntegrationEvent()
            {
                Transaction = notification.Transaction,
                OutContract = notification.OutContract,
                NewOutContractServicePackages = notification.NewOutContractServicePackages
            };
            await _integrationEventLogService.AddAndSaveEventAsync(upgradeServicePackageIntegrationEvent);
        }
    }
}
