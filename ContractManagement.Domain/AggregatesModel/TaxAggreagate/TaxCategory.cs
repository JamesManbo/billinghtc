using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TaxAggreagate
{
    [Table("TaxCategories")]
    public class TaxCategory : Entity
    {
        public static TaxCategory VAT => new TaxCategory(1, "Giá trị gia tăng", "VAT", 10,1,"admin", "");
        public static TaxCategory WHT => new TaxCategory(2, "Thuế nhà thầu nước ngoài", "WHT", 10, 1, "admin", "");
        public static IEnumerable<TaxCategory> Seeds() => new[] {VAT, WHT};

        public TaxCategory()
        {
        }

        public TaxCategory(int id, string taxName, string taxCode, int taxValue, int? userId,string userName, string explainTax)
        {
            this.Id = id;
            this.TaxName = taxName;
            this.TaxCode = taxCode;
            this.TaxValue = taxValue;
            this.UserId = userId;
            this.UserName = userName;
            this.ExplainTax = explainTax;
        }

        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string ExplainTax { get; set; }
    }
}