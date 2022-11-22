using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.ContractEvents
{
    public class TransactionDomainEvent : IRequest<ActionResponse>
    {
        public List<TransactionDTO> TransactionDTO { get; }
        public int[] ExcludeChannelIds { get; }

        public TransactionDomainEvent(List<TransactionDTO> transaction, int[] excludeChannelIds = null)
        {
            TransactionDTO = transaction;
            ExcludeChannelIds = excludeChannelIds;
        }
    }
}
