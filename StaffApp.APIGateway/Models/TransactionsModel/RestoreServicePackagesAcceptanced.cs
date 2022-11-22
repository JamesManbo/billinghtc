using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class RestoreServicePackagesAcceptanced : CUTransactionBase
    {
        public RestoreServicePackagesAcceptanced()
        {
            ContractIds = new List<int>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }

        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
