using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContactInfoDTO
    {
        public int Id { get; set; }
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
