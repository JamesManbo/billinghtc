using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class UnitOfMeasurementDTO
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string StateLabel => !IsActive ? "Không hoạt động" : "Đang hoạt động";
        public DateTime UpdatedDate { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
