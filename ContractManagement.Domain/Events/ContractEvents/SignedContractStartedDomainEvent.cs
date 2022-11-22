using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.Events
{
    public class SignedContractStartedDomainEvent : IRequest<ActionResponse>
    {
        public OutContractDTO Contract { get; }
        public int[] ExcludeChannelIds { get; }

        public SignedContractStartedDomainEvent(OutContractDTO contract, int[] excludeChannelIds = null)
        {
            Contract = contract;
            ExcludeChannelIds = excludeChannelIds;
        }
    }
}
