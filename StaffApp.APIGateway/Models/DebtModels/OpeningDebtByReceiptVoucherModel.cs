using DebtManagement.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.DebtModels
{
    public class OpeningDebtByReceiptVoucherModel
    {
        public int Id { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IssuedMonth => IssuedDate.Month;
        public int Status { get; set; }
        public int ReceiptVoucherId { get; set; }
        public string ReceiptVoucherCode { get; set; }
        public string ReceiptVoucherContent { get; set; }
        public string SubstituteVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public Money OpeningTargetDebtTotal { get; set; }
        public Money OpeningCashierDebtTotal { get; set; }
        public int NumberOfPaymentDetails { get; set; }
        public List<ReceiptVoucherPaymentDetailDTO> PaymentDetails { get; set; }
    }
}
