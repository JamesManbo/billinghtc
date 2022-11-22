using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherTaxInsertBulkModel
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int DisplayOrder { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int? VoucherLineId { get; set; }
        public bool IsAutomaticGenerate { get; set; }
    }
}
