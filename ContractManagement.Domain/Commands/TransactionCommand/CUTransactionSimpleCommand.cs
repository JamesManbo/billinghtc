using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUTransactionSimpleCommand
    {
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public int InContractId { get; set; }
        public int Type { get; set; }
        public int StatusId { get; set; }
    }
}
