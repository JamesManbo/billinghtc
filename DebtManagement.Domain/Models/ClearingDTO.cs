using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ClearingDTO: BaseDTO
    {
        public ClearingDTO()
        {
            ReceiptVouchers = new List<ReceiptVoucherDTO>();
            PaymentVouchers = new List<PaymentVoucherDTO>();
            AttachmentFiles = new List<AttachmentFileDTO>();
        }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CodeClearing { get; set; }
        public string CalculatorUserId { get; set; }
        public int PaymentConfirmerUserId { get; set; }
        public DateTime ClearingDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName => StatusId > 0 ? ClearingStatus.From(StatusId).Name : "";
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string TargetId { get; set; }
        public string TargetFullName { get; set; }
        public string TargetPhone { get; set; }
        public string CreatedUserId { get; set; }
        public string OrganizationUnitId { get; set; }
        public string ApprovalUserId { get; set; }
        public int MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal TotalPayment { get; set; }
        public VoucherTargetDTO Target { get; set; }
        public List<ReceiptVoucherDTO> ReceiptVouchers { get; set; }
        public List<PaymentVoucherDTO> PaymentVouchers { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public string CancelReason { get; set; }
    }
}
