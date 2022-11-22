using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.SharingRevenueModels
{
    public class SharingRevenueLineDetailDTO : BaseDTO
    {
        public string SharingLineUid { get; set; }
        public int? SharingLineId { get; set; }
        public string CurrencyUnitCode { get; set; }

        public int SharingType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public decimal SharingAmount { get; set; }
    }
}
