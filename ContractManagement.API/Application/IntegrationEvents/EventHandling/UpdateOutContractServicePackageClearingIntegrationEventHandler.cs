using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateOutContractServicePackageClearingIntegrationEventHandler : IIntegrationEventHandler<UpdateOutContractServicePackageClearingIntegrationEvent>
    {
        private readonly IOutContractServicePackageClearingRepository _outContractServicePackageClearingRepository;

        public UpdateOutContractServicePackageClearingIntegrationEventHandler(IOutContractServicePackageClearingRepository outContractServicePackageClearingRepository)
        {
            _outContractServicePackageClearingRepository = outContractServicePackageClearingRepository;
        }

        public async Task Handle(UpdateOutContractServicePackageClearingIntegrationEvent @event)
        {
            if(@event == null || !@event.OutContractServicePackageClearingIntegrationEvents.Any())
            {
                return;
            }

            foreach(var item in @event.OutContractServicePackageClearingIntegrationEvents)
            {
                _outContractServicePackageClearingRepository.Create(new OutContractServicePackageClearing() { 
                    Change = item.Change,
                    IsUsed = false,
                    OutContractServicePackageId = item.OutContractServicePackageId,
                    IsDaysPlus = item.IsDaysPlus,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "Hệ thống",
                    IsActive = true,
                    IsDeleted = false,
                    DisplayOrder = 0,
                });
            }

            await _outContractServicePackageClearingRepository.SaveChangeAsync();
        }
    }
}
