using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class TransactionRequestFilterModel : RequestFilterModel
    {
        public string StatusIds { get; set; }
        public string AcceptanceTypes { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ProjectIds { get; set; }
        public string SupporterId { get; set; }
    }
}
