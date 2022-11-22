using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class OutContractTransactionDTO
    {
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public int TransactionId { get; set; }
    }
}
