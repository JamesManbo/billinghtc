using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.PaymentVoucherModels;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class PaymentVoucherDTO : VoucherBaseDTO
    {
        public PaymentVoucherDTO()
        {
            PaymentVoucherDetails = new List<PaymentVoucherDetailDTO>();
            PaymentDetails = new List<PaymentVoucherPaymentDetailDTO>();
            PaymentVoucherTaxes = new List<TaxCategoryDTO>();
            ReciptVouchers = new List<ReceiptVoucherInPaymentVoucherDTO>();
        }
        public VoucherTargetDTO Target { get; set; }
        public int InContractId { get; set; }

        public List<PaymentVoucherDetailDTO> PaymentVoucherDetails { get; set; }
        public List<TaxCategoryDTO> PaymentVoucherTaxes { get; set; }
        public List<PaymentVoucherPaymentDetailDTO> PaymentDetails { get; set; }
        public string StatusName => StatusId > 0 ? Enumeration.FromValue<ReceiptVoucherStatus>(StatusId).ToString(): "";

        public List<ReceiptVoucherInPaymentVoucherDTO> ReciptVouchers { get; set; }
    }
}
