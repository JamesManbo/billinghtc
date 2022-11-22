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
    public class ChangeServicePackageDomainEventHandler : INotificationHandler<ChangeServicePackageDomainEvent>
    {
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IWrappedConfigAndMapper _configAndMapperWapper;
        public ChangeServicePackageDomainEventHandler(IContractIntegrationEventService integrationEventLogService, IWrappedConfigAndMapper configAndMapperWapper)
        {
            _integrationEventLogService = integrationEventLogService;
            _configAndMapperWapper = configAndMapperWapper;
        }

        public async Task Handle(ChangeServicePackageDomainEvent notification, CancellationToken cancellationToken)
        {
            var changeServicePackageIntegrationEvent = new ChangeServicePackageIntegrationEvent() { 
                Transaction = notification.Transaction.MapTo<TransactionDTO>(_configAndMapperWapper.MapperConfig),
                OutContract = notification.OutContract,
                OldOutContractServicePackage = notification.OldOutContractServicePackage,
                NewOutContractServicePackage = notification.NewOutContractServicePackage
            };
            await _integrationEventLogService.AddAndSaveEventAsync(changeServicePackageIntegrationEvent);
        }
    }
}
