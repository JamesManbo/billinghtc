using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using GenericRepository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherDebtHistories")]
    public class ReceiptVoucherDebtHistory : Entity
    {
        public DateTime IssuedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public int? ReceiptVoucherId { get; set; }
        public string ReceiptVoucherCode { get; set; }
        public string ReceiptVoucherContent { get; set; }
        public int? SubstituteVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public int NumberOfPaymentDetails { get; set; }
        public bool IsAutomaticGenerate { get; set; }

        private List<ReceiptVoucherPaymentDetail> _paymentDetails;
        [IgnoreDataMember]
        public IReadOnlyCollection<ReceiptVoucherPaymentDetail> PaymentDetails => _paymentDetails;

        // Tổng công nợ của nhân viên/đại lý thu hộ
        public decimal OpeningCashierDebtTotal { get; set; }
        // Tổng công nợ của khách hàng(đối tượng phải thu)
        public decimal OpeningTargetDebtTotal { get; set; }
        public decimal CashingPaidTotal { get; set; }
        public decimal TransferringPaidTotal { get; set; }
        public decimal CashingAccountedTotal { get; set; }
        public decimal TransferringAccountedTotal { get; set; }
        public bool IsOpeningDebtRecorded { get; set; }
        public string CurrencyUnitCode { get; set; }
        public ReceiptVoucherDebtHistory()
        {
            IdentityGuid = Guid.NewGuid().ToString();
            _paymentDetails = new List<ReceiptVoucherPaymentDetail>();
            OpeningCashierDebtTotal = 0;
            OpeningTargetDebtTotal = 0;
            CashingAccountedTotal = 0;
            TransferringAccountedTotal = 0;
            CashingPaidTotal = 0;
            TransferringPaidTotal = 0;
            CreatedDate = DateTime.UtcNow;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }

        private void ReUpdateStatus()
        {
            if (this._paymentDetails.Count == 0 || this._paymentDetails.All(p => p.PaidAmount <= 0))
            {
                this.Status = PaymentStatus.Assigned;
            }
        }

        public void AddPaymentDetail(CuReceiptVoucherPaymentDetailCommand command, bool force = false)
        {
            if ((command.PaidAmount.HasValue && command.PaidAmount.Value > 0) || force)
            {
                var newReceiptVoucherDetail = new ReceiptVoucherPaymentDetail(command)
                {
                    CreatedBy = this.CreatedBy,
                    CashierUserId = this.CashierUserId,
                    ReceiptVoucherId = this.ReceiptVoucherId,
                    CurrencyUnitCode = this.CurrencyUnitCode
                };

                this._paymentDetails.Add(newReceiptVoucherDetail);
                this.CalculateTotal();
            }
        }

        public void AddOrUpdatePaymentDetail(CuReceiptVoucherPaymentDetailCommand command, bool force = false)
        {
            var receiptVchrPaymentDetail = _paymentDetails.FirstOrDefault(p => command.Id > 0 && p.Id == command.Id);
            if (receiptVchrPaymentDetail == null)
            {
                command.CurrencyUnitCode = this.CurrencyUnitCode;
                this.AddPaymentDetail(command, force);
            }
            else
            {
                receiptVchrPaymentDetail.PaidAmount = command.PaidAmount ?? 0;
                receiptVchrPaymentDetail.UpdatedBy = command.UpdatedBy;
                receiptVchrPaymentDetail.UpdatedDate = DateTime.Now;
                receiptVchrPaymentDetail.CurrencyUnitCode = this.CurrencyUnitCode;
            }

            this.CalculateTotal();
        }

        public void UpdatePaymentDetail(ReceiptVoucherPaymentDetail entity)
        {
            var receiptVchrPaymentDetail = _paymentDetails.FirstOrDefault(p => p.Id == entity.Id);
            if (entity.PaidAmount > 0)
            {
                receiptVchrPaymentDetail.PaidAmount = entity.PaidAmount;
                receiptVchrPaymentDetail.UpdatedBy = entity.UpdatedBy;
                receiptVchrPaymentDetail.UpdatedDate = DateTime.Now;
            }
            else
            {
                _paymentDetails.Remove(receiptVchrPaymentDetail);
            }
            this.CalculateTotal();
        }

        public void SetAccountedStatus()
        {
            this.Status = PaymentStatus.Accounted;
            foreach (var paymentDetail in _paymentDetails)
            {
                if (paymentDetail.IsDeleted || paymentDetail.PaidAmount <= 0)
                {
                    _paymentDetails.Remove(paymentDetail);
                }
                else
                {
                    paymentDetail.SetAccountedStatus();
                }
            }
            this.CalculateTotal();
        }

        public void ClearPaymentDetails()
        {
            this._paymentDetails?.Clear();
            this.CalculateTotal();
        }

        public void CalculateTotal()
        {
            this.ReUpdateStatus();
            this.NumberOfPaymentDetails = this.PaymentDetails.Count;

            this.TransferringPaidTotal = this.PaymentDetails
                    .Where(p => p.PaymentMethod == VoucherPaymentMethod.Transfer.Id)
                    .Sum(e => e.PaidAmount);

            this.CashingPaidTotal = this.PaymentDetails
                    .Where(p => p.PaymentMethod == VoucherPaymentMethod.Cash.Id)
                    .Sum(e => e.PaidAmount);

            this.TransferringAccountedTotal = this.PaymentDetails
                    .Where(p => p.PaymentMethod == VoucherPaymentMethod.Transfer.Id &&
                        p.Status == PaymentStatus.Accounted)
                    .Sum(e => e.PaidAmount);

            this.CashingAccountedTotal = this.PaymentDetails
                    .Where(p => p.PaymentMethod == VoucherPaymentMethod.Cash.Id &&
                        p.Status == PaymentStatus.Accounted)
                    .Sum(e => e.PaidAmount);
        }
    }

    public enum PaymentStatus
    {
        Assigned = 0,
        CollectionOnBeHalf = 1,
        Accounted = 8
    }
}
