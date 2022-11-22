using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using MediatR;

namespace ContractManagement.Domain.Events
{
    public class ContractStartedDomainEvent : INotification
    {
        public OutContract Contract { get; }

        public ContractStartedDomainEvent(OutContract contract)
        {
            Contract = contract;
        }
    }
}
