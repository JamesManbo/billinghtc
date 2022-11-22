using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class TaxCategoryDTO
    {
        public int Id { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string ExplainTax { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
        public string StateLabel => !IsActive ? "Không hoạt động" : "Đang hoạt động";
    }
}
