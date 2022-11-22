using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.TransactionModels
{
    public class RequestAddNewTransaction
    {
        public string TransactionJSON { get; set; }
        public int TypeId { get; set; }
    }
}
