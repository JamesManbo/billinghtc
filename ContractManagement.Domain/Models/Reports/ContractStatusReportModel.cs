using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class ContractStatusReportModel
    {
        public string ContractStatusName { get; set; }
        public int ContractStatusId { get; set; }
        public int Amount { get; set; }
    }
}
