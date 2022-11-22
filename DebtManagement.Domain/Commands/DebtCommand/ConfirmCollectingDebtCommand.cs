using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.DebtCommand
{
    public class ConfirmCollectingDebtCommand : IRequest<ActionResponse>
    {
        public int[] ReceiptVoucherIds { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public string ApprovedUserId { get; set; }
    }
}
