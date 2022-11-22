using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("PaymentVoucherPaymentDetails")]
    public class PaymentVoucherPaymentDetail : Entity
    {
        public int PaymentVoucherId { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal PaidAmount { get; set; }
        public int PaymentTurn { get; set; }

        public PaymentVoucherPaymentDetail()
        {
        }

        public PaymentVoucherPaymentDetail(CuPaymentVoucherPaymentDetailCommand command)
        {
            this.IdentityGuid = Guid.NewGuid().ToString();
            this.PaymentVoucherId = command.PaymentVoucherId;
            this.PaymentMethod = command.PaymentMethod;
            this.PaymentTurn = command.PaymentTurn;
            this.PaymentMethodName = command.PaymentMethodName;
            this.PaidAmount = command.PaidAmount ?? 0;
            this.CreatedBy = command.CreatedBy;
            this.UpdatedBy = command.UpdatedBy;
            this.CreatedDate = command.PaymentDate ?? DateTime.Now;
        }
    }
}
