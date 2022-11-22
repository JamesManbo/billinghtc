using DebtManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class BillingPaymentCanceledIntegrationEvent : IntegrationEvent
    {
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public bool IsFirstVoucherOfContract { get; set; }
        public int OutContractId { get; set; }
        public List<ReceiptVoucherDetailDTO> VoucherDetails { get; set; }
        public BillingPaymentCanceledIntegrationEvent()
        {
            VoucherDetails = new List<ReceiptVoucherDetailDTO>();
        }
    }
}
