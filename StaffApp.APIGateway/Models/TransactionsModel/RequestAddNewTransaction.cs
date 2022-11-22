using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class RequestAddNewTransaction
    {
        public string TransactionJSON { get; set; }
        public int TypeId { get; set; }
    }
}
