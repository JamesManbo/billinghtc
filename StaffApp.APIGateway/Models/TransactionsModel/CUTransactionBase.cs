using System;
using System.Collections.Generic;
using System.Text;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public abstract class CUTransactionBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public int StatusId { get; set; }
        public List<int> ContractIds { get; set; }

        public DateTime TransactionDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string OrganizationUnitId { get; set; }
        public string HandleUserId { get; set; }
        public string Note { get; set; }
        public string AcceptanceNotes { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AcceptanceStaff { get; set; }
    }
}
