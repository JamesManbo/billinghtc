using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.DebtCommand
{
    public class CuOpeningDebtPaymentCommand
    {
        public CuOpeningDebtPaymentCommand()
        {
            PaymentDetails = new List<CuReceiptVoucherPaymentDetailCommand>();
        }

        public int Id { get; set; }
        public DateTime IssuedDate { get; set; }
        public int ReceiptVoucherId { get; set; }
        public string ReceiptVoucherCode { get; set; }
        public string ReceiptVoucherContent { get; set; }
        public int? SubstituteVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public decimal DebtTotal { get; set; }
        public decimal OpeningCashierDebtTotal { get; set; }
        public decimal OpeningTargetDebtTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IssuedMonth => this.IssuedDate.Month;
        public List<CuReceiptVoucherPaymentDetailCommand> PaymentDetails;
    }
}
