using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.RadiusAndBras
{
    public class BrasInfoDTO : BaseDTO
    {
        public string IP { get; set; }
        public string ProjectName { get; set; }
        public int ManualAPIPort { get; set; }
        public int? SSHPort { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? ProjectId { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
