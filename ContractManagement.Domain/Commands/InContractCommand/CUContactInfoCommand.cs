using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.InContractCommand
{
    public class CUContactInfoCommand
    {
        public int Id { get; set; }
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
