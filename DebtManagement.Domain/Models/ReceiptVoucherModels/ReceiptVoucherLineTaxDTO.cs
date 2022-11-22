using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherLineTaxDTO
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int VoucherLineId { get; set; }
    }
    public class SharingTotalAmountByReceiptVoucher
    {
        public int OutContractId { get; set; }
        public decimal SharingPackageAmount { get; set; }
    }
}
