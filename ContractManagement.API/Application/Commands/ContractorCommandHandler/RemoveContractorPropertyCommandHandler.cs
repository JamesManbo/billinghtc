using ContractManagement.Domain.Commands.ContractorCommand;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.DomainEventHandlers.ContractorEventHandlers
{
    public class RemoveContractorPropertyCommandHandler : IRequestHandler<RemoveContractorPropertyCommand>
    {
        private readonly ILogger<RemoveContractorPropertyCommandHandler> _logger;
        private readonly IContractorPropertiesRepository _contractorPropertyRepository;

        public RemoveContractorPropertyCommandHandler(ILogger<RemoveContractorPropertyCommandHandler> logger, 
            IContractorPropertiesRepository contractorPropertyRepository)
        {
            this._logger = logger;
            this._contractorPropertyRepository = contractorPropertyRepository;
        }

        public async Task<Unit> Handle(RemoveContractorPropertyCommand request, CancellationToken cancellationToken)
        {
            if (request.ContractorCategoryId.HasValue)
            {
                _contractorPropertyRepository.RemoveAllContractorCategory(request.ContractorCategoryId.Value);
            }

            if (request.ContractorClassId.HasValue)
            {
                _contractorPropertyRepository.RemoveAllContractorClass(request.ContractorClassId.Value);
            }

            if (request.ContractorGroupId.HasValue)
            {
                _contractorPropertyRepository.RemoveAllContractorGroup(request.ContractorGroupId.Value.ToString(), request.ContractorGroupName);
            }

            if (request.ContractorIndustryId.HasValue)
            {
                _contractorPropertyRepository.RemoveAllContractorIndustry(request.ContractorIndustryId.Value);
            }

            if (request.ContractorStructureId.HasValue)
            {
                _contractorPropertyRepository.RemoveAllContractorStructure(request.ContractorStructureId.Value);
            }

            if (request.ContractorTypeId.HasValue)
            {
                _contractorPropertyRepository.RemoveAllContractorType(request.ContractorTypeId.Value);
            }

            return await Task.FromResult(Unit.Value);
        }
    }
}
