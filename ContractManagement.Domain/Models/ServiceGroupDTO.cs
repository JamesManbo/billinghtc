using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ServiceGroupDTO
    {
        public int Id { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string StateLabel => !IsActive ? "Đã khóa" : "Đang hoạt động";
    }
}
