using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateServicePackageSuspensionTimesIntegrationEventHandler : IIntegrationEventHandler<UpdateServicePackageSuspensionTimesIntegrationEvent>
    {
        private readonly IServicePackageSuspensionTimeRepository _servicePackageSuspensionTimeRepository;
        public UpdateServicePackageSuspensionTimesIntegrationEventHandler(IServicePackageSuspensionTimeRepository servicePackageSuspensionTimeRepository)
        {
            _servicePackageSuspensionTimeRepository = servicePackageSuspensionTimeRepository;
        }

        public async Task Handle(UpdateServicePackageSuspensionTimesIntegrationEvent @event)
        {
            if (@event == null || !@event.ServicePackageSuspensionTimeEvents.Any())
            {
                return;
            }

            var spstIds = @event.ServicePackageSuspensionTimeEvents.Select(s => s.Id).ToArray();
            var spSuspensionTimes = await _servicePackageSuspensionTimeRepository.GetByIdsAsync(spstIds);
            for(int i = 0; i < @event.ServicePackageSuspensionTimeEvents.Count; i++)
            {
                var spSuspensionTime = spSuspensionTimes.Find(f => f.Id == @event.ServicePackageSuspensionTimeEvents[i].Id);
                if(spSuspensionTime != null)
                {
                    spSuspensionTime.RemainingAmount = @event.ServicePackageSuspensionTimeEvents[i].RemainingAmount;
                    _servicePackageSuspensionTimeRepository.Update(spSuspensionTime);
                }
            }
            await _servicePackageSuspensionTimeRepository.SaveChangeAsync();
        }
    }
}
