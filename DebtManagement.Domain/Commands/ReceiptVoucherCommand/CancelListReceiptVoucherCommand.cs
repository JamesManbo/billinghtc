using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class CancelListReceiptVoucherCommand : IRequest<ActionResponse>
    {
        public string Ids { get; set; }
        public string CancellationReason { get; set; }
        public string UpdatedBy { get; set; }
    }
}
