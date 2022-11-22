using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ClearingCommand;
using DebtManagement.Domain.Seed;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace DebtManagement.Domain.AggregatesModel.ClearingAggregate
{
    [Table("Clearings")]
    public class Clearing : Entity
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CodeClearing { get; set; }
        public string CalculatorUserId { get; set; }
        public int PaymentConfirmerUserId { get; set; }
        public DateTime ClearingDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public PaymentMethod Payment { get; set; }
        public int TargetId { get; set; }
        public string CreatedUserId { get; set; }
        public string OrganizationUnitId { get; set; }
        public string ApprovalUserId { get; set; }
        public string CancelReason { get; set; }
        public int MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal TotalPayment { get; set; }
        public virtual ICollection<ReceiptVoucher> ReceiptVouchers { get; set; }
        public virtual ICollection<PaymentVoucher> PaymentVouchers { get; set; }
        public double ExchangeRate { get; set; }
        public DateTime ExchangeRateApplyDate { get; set; }

        public Clearing()
        {
            IdentityGuid = Guid.NewGuid().ToString();
            ReceiptVouchers = new HashSet<ReceiptVoucher>();
            PaymentVouchers = new HashSet<PaymentVoucher>();
        }

        public Clearing(CUClearingCommand cUClearingCommand)
        {
            IdentityGuid = Guid.NewGuid().ToString();
            CodeClearing = cUClearingCommand.CodeClearing;
            CalculatorUserId = cUClearingCommand.CalculatorUserId;
            PaymentConfirmerUserId = cUClearingCommand.PaymentConfirmerUserId;
            ClearingDate = cUClearingCommand.ClearingDate;
            FromDate = cUClearingCommand.FromDate;
            ToDate = cUClearingCommand.ToDate;
            StatusId = cUClearingCommand.StatusId;
            Description = cUClearingCommand.Description;
            Payment = cUClearingCommand.Payment;
            TargetId = cUClearingCommand.TargetId;
            CreatedUserId = cUClearingCommand.CreatedUserId;
            OrganizationUnitId = cUClearingCommand.OrganizationUnitId;
            MarketAreaId = cUClearingCommand.MarketAreaId;
            MarketAreaName = cUClearingCommand.MarketAreaName;
            ClearingTotal = cUClearingCommand.ClearingTotal;
            TotalReceipt = cUClearingCommand.TotalReceipt;
            TotalPayment = cUClearingCommand.TotalPayment;
            CurrencyUnitId = cUClearingCommand.CurrencyUnitId;
            CurrencyUnitCode = cUClearingCommand.CurrencyUnitCode;
            ReceiptVouchers = new HashSet<ReceiptVoucher>();
            PaymentVouchers = new HashSet<PaymentVoucher>();
            CancelReason = cUClearingCommand.CancelReason;
        }

        public void Update(CUClearingCommand cUClearingCommand)
        {
            CodeClearing = cUClearingCommand.CodeClearing;
            CalculatorUserId = cUClearingCommand.CalculatorUserId;
            PaymentConfirmerUserId = cUClearingCommand.PaymentConfirmerUserId;
            ClearingDate = cUClearingCommand.ClearingDate;
            FromDate = cUClearingCommand.FromDate;
            ToDate = cUClearingCommand.ToDate;
            StatusId = cUClearingCommand.StatusId;
            Description = cUClearingCommand.Description;
            Payment = cUClearingCommand.Payment;
            TargetId = cUClearingCommand.TargetId;
            CreatedUserId = cUClearingCommand.CreatedUserId;
            OrganizationUnitId = cUClearingCommand.OrganizationUnitId;
            MarketAreaId = cUClearingCommand.MarketAreaId;
            MarketAreaName = cUClearingCommand.MarketAreaName;
            ClearingTotal = cUClearingCommand.ClearingTotal;
            ApprovalUserId = cUClearingCommand.ApprovalUserId;
            UpdatedBy = cUClearingCommand.UpdatedBy;
            CurrencyUnitId = cUClearingCommand.CurrencyUnitId;
            CurrencyUnitCode = cUClearingCommand.CurrencyUnitCode;
            CancelReason = cUClearingCommand.CancelReason;
        }

        public void SetStatusId(int stautus)
        {
            StatusId = stautus;
        }

        public void SetContractor(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
