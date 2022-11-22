using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class BusinessBlockDTO
    {
        public int Id { get; set; }
        public string BusinessBlockName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

    }
}
