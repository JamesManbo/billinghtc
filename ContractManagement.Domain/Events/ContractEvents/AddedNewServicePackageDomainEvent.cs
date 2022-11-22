using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.Events
{
    public class AddedNewServicePackageDomainEvent : IRequest<ActionResponse>
    {
        public OutContract OutContract { get; set; }
        public OutContractServicePackage NewChannel { get; set; }
        public AddedNewServicePackageDomainEvent(OutContract outContract, OutContractServicePackage newChannel)
        {
            OutContract = outContract;
            NewChannel = newChannel;
        }
    }
}
