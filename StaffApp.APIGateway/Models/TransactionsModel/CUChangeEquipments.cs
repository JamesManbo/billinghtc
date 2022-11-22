
using Global.Models.StateChangedResponse;
using MediatR;
using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class CUChangeEquipments : CUTransactionBase
    {
        public CUChangeEquipments()
        {
            ContractIds = new List<int>();
            TransactionEquipmentsId = new List<int>();
            AttachmentFiles = new List<CreateUpdateFile>();
        }

        public List<int> TransactionEquipmentsId { get; set; }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
