using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class AcceptanceDTO: TransactionDTO
    {
        public ContractorDTO Contractor { get; set; }
        public DateTime CreatedDate { get; set; }
         
    }
}
