﻿using ContractManagement.Domain.Events.ContractEvents;
using ContractManagement.Domain.Events.DebtEvents;
using ContractManagement.Domain.Models.ReceiptVouchers;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.DebtCommand
{
    public class UpdateContractAfterPaymentCommand : IRequest<ActionResponse>
    {
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public int OutContractId { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public List<ReceiptVoucherDetailDTO> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucherDTO> Promotions { get; set; }
    }
}
