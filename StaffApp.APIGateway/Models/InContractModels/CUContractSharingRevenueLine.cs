using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.InContractModels
{
    public class CUContractSharingRevenueLine
    {
        public string CsrUid { get; set; }
        public int Id { get; set; }
        public int? CsrId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public int? InServiceChannelId { get; set; }
        public int? OutServiceChannelId { get; set; }
        public float SharedInstallFeePercent { get; set; }
        public float SharedPackagePercent { get; set; }
        public decimal SharedFixedAmount { get; set; }
        public int PointType { get; set; }
        public int? OutContractPackageId { get; set; }
        public int SharingType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
