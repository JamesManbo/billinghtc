using DebtManagement.Domain.Commands.Commons;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
   public class BadReceiptVoucherCommand : IRequest<ActionResponse>
    {
        public BadReceiptVoucherCommand()
        {
            AttachmentFiles = new List<AttachmentFileCommand>();
        }
        public string BadDebtApprovalContent { get; set; }
        public string UpdatedBy { get; set; }
        public List<int> Ids { get; set; }
        public List<AttachmentFileCommand> AttachmentFiles { get; set; }
    }
}
