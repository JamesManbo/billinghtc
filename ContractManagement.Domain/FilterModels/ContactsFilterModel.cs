using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class ContactsFilterModel : RequestFilterModel
    {
        public string ProjectIds { get; set; }
        public int? ProjectId { get; set; }
        public int? ContractStatusId { get; set; }
        public int? ContractorId { get; set; }
        public string IdentityGuid { get; set; }
        public string ServiceIds { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
