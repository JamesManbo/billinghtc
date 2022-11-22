using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class InContractInfoForReport
    {
        public int OutContractId { get; set; }
        public int InContractId { get; set; }
        public string InContractCode { get; set; }
        public string InContractorFullName { get; set; }
        public string HasSharing { get; set; }
        public int SharingType { get; set; }

        public int SharedPackagePercent { get; set; }
        public int PartnerPercent { get; set; }
    }
}
