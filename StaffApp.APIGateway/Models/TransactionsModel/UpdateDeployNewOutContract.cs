using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class UpdateDeployNewOutContract : CUTransactionBase
    {
        public UpdateDeployNewOutContract()
        {
            ContractIds = new List<int>();
            TransactionServicePackages = new List<CUTransactionServicePackageAccepted>();
            TransactionEquipments = new List<CUTransactionEquipment>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }

        public List<CUTransactionServicePackageAccepted> TransactionServicePackages { get; set; }
        public List<CUTransactionEquipment> TransactionEquipments { get; set; }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
