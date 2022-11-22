using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling
{
    public class PaidRealtimeRcptVchrIntegrationEventHandler : IIntegrationEventHandler<PaidRealtimeReceiptVoucherIntegrationEvent>
    {
        private readonly IContractSrvPckRepository _contractSrvPckRepository;

        public PaidRealtimeRcptVchrIntegrationEventHandler(IContractSrvPckRepository contractSrvPckRepository)
        {
            _contractSrvPckRepository = contractSrvPckRepository;
        }

        public async Task Handle(PaidRealtimeReceiptVoucherIntegrationEvent @event)
        {
            if (@event.PaidRealtimeServicePackages == null || @event.PaidRealtimeServicePackages.Count == 0)
            {
                return;
            }

            foreach (var paidSrvPackage in @event.PaidRealtimeServicePackages)
            {
                var outContractSrvPck = await _contractSrvPckRepository.GetByIdAsync(paidSrvPackage.OutContractServicePackageId);
                outContractSrvPck.SetNextBillingDate(paidSrvPackage.EndDate.AddDays(1));
                _contractSrvPckRepository.Update(outContractSrvPck);
            }

            await _contractSrvPckRepository.SaveChangeAsync();
        }
    }
}
