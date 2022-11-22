using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class ReportContractorInProjectDTO
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalCustomerQuit { get; set; }
        public string PackageName { get; set; }

        public int Total { get; set; }
    }
}
