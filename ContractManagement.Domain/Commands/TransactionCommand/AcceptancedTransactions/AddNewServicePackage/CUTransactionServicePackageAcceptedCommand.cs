using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.AddNewServicePackage
{
    public class CUTransactionServicePackageAcceptedCommand
    {
        public int Id { get; set; }
        public string CId { get; set; }
    }
}
