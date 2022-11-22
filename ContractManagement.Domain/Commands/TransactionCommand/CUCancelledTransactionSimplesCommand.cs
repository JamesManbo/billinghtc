using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUCancelledTransactionSimplesCommand : IRequest<ActionResponse>
    {
        public int[] TransactionIds { get; set; }
        public string AcceptanceStaff { get; set; }
        public int StatusId { get; set; }
        public string ReasonContent { get; set; }
    }
}
