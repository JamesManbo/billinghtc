using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.SharingRevenueModels
{
    public class ContractSharingRevenueLineInsertBulkModel
    {
        public int Id { get; set; }
        public string CsrUid { get; set; }
        public int? CsrId { get; set; }
        public int SharingType { get; set; }
        public int? OutContractPackageId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServicePackageId { get; set; }
        public int? InServiceChannelId { get; set; }
        public int? OutServiceChannelId { get; set; }
        public float SharedInstallFeePercent { get; set; }
        public float SharedPackagePercent { get; set; }
        public decimal SharedFixedAmount { get; set; }
        public int PointType { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int DisplayOrder { get; set; }

        public int Month { get; set; }
        public string OutContractCode { get; set; }
        public string CsMaintenanceUid { get; set; }
        public int Year { get; set; }
    }
}
