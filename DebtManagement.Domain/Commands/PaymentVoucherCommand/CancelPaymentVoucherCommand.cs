using Global.Models.StateChangedResponse;
using MediatR;

namespace DebtManagement.Domain.Commands.PaymentVoucherCommand
{
    public class CancelPaymentVoucherCommand :  IRequest<ActionResponse>
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string CancellationReason { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
