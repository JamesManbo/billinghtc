using Global.Models.StateChangedResponse;
using MediatR;
using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class CUReclaimEquipments : CUTransactionBase
    {
        public CUReclaimEquipments()
        {
            ContractIds = new List<int>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
