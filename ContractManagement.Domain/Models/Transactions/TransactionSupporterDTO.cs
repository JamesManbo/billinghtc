using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class TransactionSupporterDTO
    {
        public int PendingTask { get; set; }
        public int DoneTask { get; set; }
        public int CancelTask { get; set; }
        public int TotalTask { get; set; }
        public int TransactionType { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
