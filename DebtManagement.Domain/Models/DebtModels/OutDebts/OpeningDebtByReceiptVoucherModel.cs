using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebtManagement.Domain.Models.DebtModels.OutDebts
{
    public class OpeningDebtByReceiptVoucherModel
    {
        public OpeningDebtByReceiptVoucherModel()
        {
            _paymentDetails = new List<ReceiptVoucherPaymentDetailDTO>();
        }

        public int Id { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IssuedMonth => IssuedDate.Month;
        public PaymentStatus Status { get; set; }
        public int ReceiptVoucherId { get; set; }
        public string ReceiptVoucherCode { get; set; }
        public string ReceiptVoucherContent { get; set; }
        public int? SubstituteVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public decimal OpeningTargetDebtTotal { get; set; }
        public decimal OpeningCashierDebtTotal { get; set; }
        public int NumberOfPaymentDetails { get; set; }
        public string CurrencyUnitCode { get; set; }
        private List<ReceiptVoucherPaymentDetailDTO> _paymentDetails;
        public IReadOnlyCollection<ReceiptVoucherPaymentDetailDTO> PaymentDetails => _paymentDetails.OrderBy(d => d.PaymentTurn).ToList();
        public void AddPaymentDetail(ReceiptVoucherPaymentDetailDTO paymentDetail)
        {
            var duplicateChecking = _paymentDetails.Any(p =>
                p.Id == 0 ? p.PaymentMethod == paymentDetail.PaymentMethod : p.Id == paymentDetail.Id
            );

            if (!duplicateChecking)
            {
                _paymentDetails.Add(paymentDetail);
            }
        }
    }
}
