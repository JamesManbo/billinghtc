using DebtManagement.Domain.Models.ContractModels;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events
{
    public class TransactionIntegrationEvent : IntegrationEvent
    {
        public List<TransactionDTO> TransactionDTO { get; set; }           
    }
}
