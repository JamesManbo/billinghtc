using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class ChangeLocationServicePackagesAcceptanced: CUTransactionBase
    {
        public ChangeLocationServicePackagesAcceptanced()
        {
            ContractIds = new List<int>();
            TransactionServicePackagesId = new List<int>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }

        public List<int> TransactionServicePackagesId { get; set; }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
