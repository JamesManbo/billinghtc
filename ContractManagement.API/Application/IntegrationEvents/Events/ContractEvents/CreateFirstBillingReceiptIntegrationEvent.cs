using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events
{
    public class CreateFirstBillingReceiptIntegrationEvent : IntegrationEvent
    {
        public CreateFirstBillingReceiptIntegrationEvent(OutContractDTO outContract)
        {
            OutContract = outContract;
        }

        public string MarketAreaCode { get; set; }
        public string ProjectCode { get; set; }
        public OutContractDTO OutContract { get; set; }
    }
}
