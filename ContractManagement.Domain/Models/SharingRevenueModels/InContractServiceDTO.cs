using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.SharingRevenueModels
{
    public class InContractServiceDTO : BaseDTO
    {
        public int InContractId { get; set; }
        public int ServiceId { get; set; }
        public int SharedType { get; set; }
        public int PointType { get; set; }
        public string PointTypeName { get; set; }
        public string ServiceName { get; set; }
        public float SharedPackagePercent { get; set; }
        public float SharedInstallFeePercent { get; set; }
    }
}
