using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.Domain.Events;
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
    public class AddedNewServicePackageDomainEventHandler : IRequestHandler<AddedNewServicePackageDomainEvent, ActionResponse>
    {
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IWrappedConfigAndMapper _configAndMapperWapper;
        private readonly IContractorQueries _contractorQueires;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IProjectQueries _projectQueries;
        private readonly ITaxCategoryQueries _taxCategoryQueries;

        public AddedNewServicePackageDomainEventHandler(IContractIntegrationEventService integrationEventLogService,
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

        public async Task<ActionResponse> Handle(AddedNewServicePackageDomainEvent request, CancellationToken cancellationToken)
        {
            var addedNewSrvPackageEvent = request.OutContract.MapTo<AddedNewServicePackageIntegrationEvent>(_configAndMapperWapper.MapperConfig);
            if (addedNewSrvPackageEvent.Contractor == null)
            {
                addedNewSrvPackageEvent.Contractor = _contractorQueires.FindById(addedNewSrvPackageEvent.ContractorId.Value);
            }

            if (addedNewSrvPackageEvent.MarketAreaId > 0)
            {
                var marketArea = _marketAreaQueries.Find(addedNewSrvPackageEvent.MarketAreaId.Value);
                addedNewSrvPackageEvent.MarketAreaCode = marketArea.MarketCode;
            }

            if (addedNewSrvPackageEvent.ProjectId > 0)
            {
                var project = _projectQueries.Find(addedNewSrvPackageEvent.ProjectId.Value);
                addedNewSrvPackageEvent.ProjectCode = project.ProjectCode;
            }

            addedNewSrvPackageEvent.NewServicePackage = request.NewChannel.MapTo<OutContractServicePackageDTO>(_configAndMapperWapper.MapperConfig);

            addedNewSrvPackageEvent.CurrencyUnitCode = addedNewSrvPackageEvent.NewServicePackage.CurrencyUnitCode;
            addedNewSrvPackageEvent.CurrencyUnitId = addedNewSrvPackageEvent.NewServicePackage.CurrencyUnitId;

            await _integrationEventLogService.AddAndSaveEventAsync(addedNewSrvPackageEvent);
            return ActionResponse.Success;
        }
    }
}
