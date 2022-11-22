using ContractManagement.Domain.Models.ReceiptVouchers;
using Global.Models.StateChangedResponse;
using MediatR;
using System.Collections.Generic;

namespace ContractManagement.Domain.Commands.DebtCommand
{
    public class UpdateContractAfterBillingCommand : IRequest<ActionResponse>
    {
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public List<ReceiptVoucherDetailDTO> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucherDTO> Promotions { get; set; }
    }
}
