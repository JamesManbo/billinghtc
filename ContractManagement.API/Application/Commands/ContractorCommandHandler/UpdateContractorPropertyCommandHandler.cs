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
    public class UpdateContractorPropertyCommandHandler : IRequestHandler<UpdateContractorPropertyCommand>
    {
        private readonly ILogger<RemoveContractorPropertyCommandHandler> _logger;
        private readonly IContractorPropertiesRepository _contractorPropertyRepository;

        public UpdateContractorPropertyCommandHandler(ILogger<RemoveContractorPropertyCommandHandler> logger, 
            IContractorPropertiesRepository contractorPropertyRepository)
        {
            this._logger = logger;
            this._contractorPropertyRepository = contractorPropertyRepository;
        }

        public Task<Unit> Handle(UpdateContractorPropertyCommand request, CancellationToken cancellationToken)
        {
            if (request.ContractorCategoryId.HasValue)
            {
                _contractorPropertyRepository.UpdateAllContractorCategory(request.ContractorCategoryId.Value,
                    request.ContractorCategoryName);
            }

            if (request.ContractorClassId.HasValue)
            {
                _contractorPropertyRepository.UpdateAllContractorClass(request.ContractorClassId.Value,
                    request.ContractorClassName);
            }

            if (!string.IsNullOrEmpty(request.ContractorGroupId))
            {
                _contractorPropertyRepository.UpdateAllContractorGroup(request.ContractorGroupId,
                    request.OldContractorGroupName,
                    request.NewContractorGroupName);
            }

            if (!string.IsNullOrEmpty(request.ContractorIndustryId))
            {
                _contractorPropertyRepository.UpdateAllContractorIndustry(request.ContractorIndustryId,
                    request.OldContractorIndustryName,
                    request.NewContractorIndustryName);
            }

            if (request.ContractorStructureId.HasValue)
            {
                _contractorPropertyRepository.UpdateAllContractorStructure(request.ContractorStructureId.Value,
                    request.ContractorStructureName);
            }

            if (request.ContractorTypeId.HasValue)
            {
                _contractorPropertyRepository.UpdateAllContractorType(request.ContractorTypeId.Value,
                    request.ContractorTypeName);
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
