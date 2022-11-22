using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
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
