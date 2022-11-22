using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class UpgradeBandwidthAcceptanced : CUTransactionBase
    {
        public UpgradeBandwidthAcceptanced()
        {
            ContractIds = new List<int>();
            TransactionEquipments = new List<CUTransactionEquipment>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }

        public List<CUTransactionEquipment> TransactionEquipments { get; set; }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
