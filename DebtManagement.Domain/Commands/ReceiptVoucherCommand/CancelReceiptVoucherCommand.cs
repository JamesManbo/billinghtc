using Global.Models.StateChangedResponse;
using MediatR;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class CancelReceiptVoucherCommand :  IRequest<ActionResponse>
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string CancellationReason { get; set; }
        public string UpdatedBy { get; set; }
    }
}
