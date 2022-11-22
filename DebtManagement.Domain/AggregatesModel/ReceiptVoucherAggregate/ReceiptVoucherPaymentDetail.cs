using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherPaymentDetails")]
    public class ReceiptVoucherPaymentDetail : Entity
    {
        public static ReceiptVoucherPaymentDetail DefaultTransferPayment(string createdBy = "Hệ thống")
        {
            return new ReceiptVoucherPaymentDetail()
            {
                IdentityGuid = Guid.NewGuid().ToString(),
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now,
                IsActive = true,
                PaidAmount = 0,
                PaymentMethod = VoucherPaymentMethod.Cash.Id,
                PaymentMethodName = VoucherPaymentMethod.Cash.Name,
                Status = PaymentStatus.Assigned,
                PaymentTurn = 0
            };
        }
        public static ReceiptVoucherPaymentDetail DefaultCashPayment(string createdBy = "Hệ thống")
        {
            return new ReceiptVoucherPaymentDetail()
            {
                IdentityGuid = Guid.NewGuid().ToString(),
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now,
                IsActive = true,
                PaidAmount = 0,
                PaymentMethod = VoucherPaymentMethod.Transfer.Id,
                PaymentMethodName = VoucherPaymentMethod.Transfer.Name,
                Status = PaymentStatus.Assigned,
                PaymentTurn = 0
            };
        }
        public static ReceiptVoucherPaymentDetail Default(int method)
        {
            return new ReceiptVoucherPaymentDetail()
            {
                IdentityGuid = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                IsActive = true,
                PaidAmount = 0,
                PaymentMethod = method,
                PaymentMethodName = Enumeration.FromValue<VoucherPaymentMethod>(method).ToString(),
                Status = PaymentStatus.Assigned,
                PaymentTurn = 0
            };
        }

        public int DebtHistoryId { get; set; }
        public int? ReceiptVoucherId { get; set; }
        public int PaymentMethod { get; set; } // 0: Tiền mặt, 1: Chuyển khoản, 2: Bù trừ
        public string PaymentMethodName { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentStatus Status { get; private set; }
        public string CashierUserId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int PaymentTurn { get; set; }

        public ReceiptVoucherPaymentDetail()
        {
            this.PaidAmount = 0;
            this.CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
            this.Status = PaymentStatus.Assigned;
            this.PaymentTurn = 0;
        }

        public ReceiptVoucherPaymentDetail(CuReceiptVoucherPaymentDetailCommand command)
        {
            this.IdentityGuid = Guid.NewGuid().ToString();
            this.PaymentMethod = command.PaymentMethod;
            this.PaymentMethodName = command.PaymentMethodName;
            this.PaidAmount = command.PaidAmount ?? 0;
            this.Status = PaymentStatus.Assigned;
            this.CreatedBy = command.CreatedBy;
            this.CreatedDate = command.PaymentDate ?? DateTime.Now;
            this.CurrencyUnitCode = command.CurrencyUnitCode;
            this.PaymentTurn = command.PaymentTurn;
        }

        public void SetAccountedStatus()
        {
            this.Status = PaymentStatus.Accounted;
        }
    }
}
