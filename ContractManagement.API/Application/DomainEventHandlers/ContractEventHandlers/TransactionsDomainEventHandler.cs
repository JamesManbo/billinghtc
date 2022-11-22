using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents;
using ContractManagement.Domain.Events;
using ContractManagement.Domain.Events.ContractEvents;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.DomainEventHandlers.ContractEventHandlers
{
    public class TransactionsDomainEventHandler : IRequestHandler<TransactionDomainEvent, ActionResponse>
    {
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IWrappedConfigAndMapper _configAndMapperWapper;
        private readonly IContractorQueries _contractorQueires;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly ITaxCategoryQueries _taxCategoryQueries;

        public TransactionsDomainEventHandler(IContractIntegrationEventService integrationEventLogService,
            IWrappedConfigAndMapper configAndMapperWapper,
            IContractorQueries contractorQueires,
            IMarketAreaQueries marketAreaQueries,
            IProjectQueries projectQueries,
            ITaxCategoryQueries taxCategoryQueries)
        {
            _integrationEventLogService = integrationEventLogService;
            _configAndMapperWapper = configAndMapperWapper;
            _contractorQueires = contractorQueires;
            _marketAreaQueries = marketAreaQueries;
            _projectQueries = projectQueries;
            _taxCategoryQueries = taxCategoryQueries;
        }

        public async Task<ActionResponse> Handle(TransactionDomainEvent request, CancellationToken cancellationToken)
        {
            var response = new ActionResponse();

            var transactionsIntegrationEvent = new TransactionIntegrationEvent(request.TransactionDTO);

            await _integrationEventLogService.AddAndSaveEventAsync(transactionsIntegrationEvent);
            return response;
        }
    }
}
