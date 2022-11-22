using DebtManagement.Domain.Models.ContractModels;
using EventBus.Events;

namespace DebtManagement.API.Application.IntegrationEvents.Events
{
    public class CreateFirstBillingReceiptIntegrationEvent : IntegrationEvent
    {
        public string MarketAreaCode { get; set; }
        public string ProjectCode { get; set; }
        public OutContractDTO OutContract { get; set; }
    }
}
