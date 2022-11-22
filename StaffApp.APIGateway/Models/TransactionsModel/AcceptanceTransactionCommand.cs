using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class AcceptanceTransactionCommand
    {
        public int TransactionId { get; set; }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
        public List<TransactionEquipmentDTO> Equipments { get; set; }
        public string Note { get; set; }
        public string AcceptanceStaff { get; set; }
        public string AcceptanceStaffUid { get; set; }
    }
}
