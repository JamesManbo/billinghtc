using ContractManagement.Domain.Commands.Commons;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUApprovedTransactionSimplesCommand : IRequest<ActionResponse>
    {
        public List<CUTransactionSimpleCommand> TransactionSimpleCommands { get; set; }
        public string AcceptanceStaff { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? StartBillingDate { get; set; }
        public bool IsOutContract { get; set; } = true;
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
