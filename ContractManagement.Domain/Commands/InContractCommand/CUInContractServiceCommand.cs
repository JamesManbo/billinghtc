using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.InContractCommand
{
    public class CUInContractServiceCommand
    {
        public int InContractId { get; set; }
        public int ServiceId { get; set; }
        public int ShareType { get; set; }
        public int PointType { get; set; }
        public string ServiceName { get; set; }
        public float SharedPackagePercent { get; set; }
        public float SharedInstallFeePercent { get; set; }
        public string ServicePackageName { get; set; }
        public decimal SharedFixedAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
