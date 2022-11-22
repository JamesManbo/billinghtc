using DebtManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ClearingCommand
{
    public class ApproveClearingVoucherCommand : IRequest<ActionResponse>
    {
        public int Id { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedByUserId { get; set; }
        public string CancelReason { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
    }
}
