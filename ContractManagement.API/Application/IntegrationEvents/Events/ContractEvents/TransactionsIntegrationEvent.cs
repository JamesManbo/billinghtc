using ContractManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class TransactionIntegrationEvent : IntegrationEvent
    {
        public List<TransactionDTO> TransactionDTO { get; set; }
        public TransactionIntegrationEvent(List<TransactionDTO> transactionDTO)
        {
            TransactionDTO = transactionDTO;
        }
    }
}
