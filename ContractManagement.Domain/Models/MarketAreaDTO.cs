using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public class MarketAreaDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int TreeLevel { get; set; }
        public string TreePath { get; set; }
        public int DisplayOrder { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string StateLabel => !IsActive ? "Đã khóa" : "Đang hoạt động";
    }
}
