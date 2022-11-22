using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TerminateServicePackagesAcceptanced : CUTransactionBase
    {
        public TerminateServicePackagesAcceptanced()
        {
            ContractIds = new List<int>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }

        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
