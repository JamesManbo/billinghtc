using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.BaseVoucher
{
    public abstract class VoucherAbstraction : Entity, IAggregateRoot
    {
        public int? MarketAreaId { get; set; }
        [StringLength(256)] public string MarketAreaName { get; set; }
        [StringLength(256)] public string ContractCode { get; set; }
        public int? ProjectId { get; set; }
        [StringLength(256)] public string ProjectName { get; set; }
        public int TypeId { get; set; }
        [StringLength(256)] public string VoucherCode { get; set; }
        public DateTime IssuedDate { get; set; }
        /// <summary>
        /// <see cref="ReceiptVoucherStatus"/>
        /// </summary>
        public int StatusId { get; protected set; }
        public int? TargetId { get; set; }
        [StringLength(6000)] public string Content { get; set; }
        [StringLength(1000)] public string Description { get; set; }
        [StringLength(256)] public string ReductionReason { get; set; }
        [StringLength(1000)] public string CancellationReason { get; set; }
        [StringLength(68)] public string CreatedUserId { get; set; }
        [StringLength(512)] public string CreatedUserName { get; set; }
        [StringLength(512)] public string CreatedUserFullName { get; set; }
        [StringLength(68)] public string OrganizationUnitId { get; set; }
        [StringLength(512)] public string OrganizationUnitName { get; set; }
        [StringLength(68)] public string InvoiceCode { get; set; }
        [StringLength(68)] public string AccountingCode { get; set; }
        [StringLength(68)] public string Source { get; set; }
        [StringLength(68)] public string ApprovedUserId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        [StringLength(256)]
        public string CurrencyUnitCode { get; set; }
        public double ExchangeRate { get; set; }
        public DateTime ExchangeRateApplyDate { get; set; }
        public int PaymentPeriod { get; set; }
        /// <summary>
        /// Số tiền giảm trừ
        /// </summary>
        public decimal ReductionFreeTotal { get; set; }
        /// <summary>
        /// Tổng tiền hòa mạng/lắp đặt
        /// </summary>
        public decimal InstallationFee { get; set; }
        /// <summary>
        /// Tổng tiền thiết bị
        /// </summary>
        public decimal EquipmentTotalAmount { get; set; }

        /// <summary>
        /// Số tiền khuyến mại
        /// </summary>
        public decimal PromotionTotalAmount { get; set; }
        /// <summary>
        /// Tổng phụ trước thuế
        /// </summary>
        public decimal SubTotalBeforeTax { get; set; }
        /// <summary>
        /// Tổng phụ sau thuế
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// Số tiền thực hiện bù trừ công nợ
        /// </summary>
        public decimal ClearingTotal { get; set; }
        /// <summary>
        /// Số tiền thu được không bao gồm bù trừ công nợ
        /// </summary>
        public decimal CashTotal { get; set; }
        /// <summary>
        /// Số tiền chi phí khác
        /// </summary>
        public decimal OtherFee { get; set; }
        /// <summary>
        /// Tổng chính trước thuế
        /// </summary>
        public decimal GrandTotalBeforeTax { get; set; }

        /// <summary>
        /// Tổng chính sau thuế
        /// </summary>
        public decimal GrandTotal { get; set; }

        /// <summary>
        /// Tổng tiền nợ đầu kỳ
        /// </summary>
        public decimal OpeningDebtAmount { get; set; }
        /// <summary>
        /// Tổng nợ đầu kỳ đã thu
        /// </summary>
        public decimal OpeningDebtPaidAmount { get; set; }
        /// <summary>
        /// Tổng chính bao gồm nợ đầu kỳ
        /// </summary>
        public decimal GrandTotalIncludeDebt { get; set; }
        /// <summary>
        /// Số tiền thu được: bao gồm tất cả các hình thức thanh toán và bù trừ công nợ
        /// </summary>
        public decimal PaidTotal { get; set; }
        /// <summary>
        /// Số tiền còn thiếu
        /// </summary>
        public decimal RemainingTotal { get; set; }
        /// <summary>
        /// Tổng tiền thuế các loại
        /// </summary>
        public decimal TaxAmount { get; set; }
        public PaymentMethod Payment { get; set; }
        public Discount Discount { get; protected set; }
        public bool IsEnterprise { get; set; }
        public bool IsLock { get; set; }
        public bool IsAutomaticGenerate { get; set; }
        public int? ClearingId { get; set; }
        /// <summary>
        /// Thời giạn thanh toán(ngày)
        /// </summary>
        public int NumberBillingLimitDays { get; set; }

        /// <summary>
        /// Số ngày quá hạn thanh toán
        /// </summary>
        public int NumberDaysOverdue { get; set; }
        public abstract void CalculateTotal(bool overrideTotal = false);
    }
}