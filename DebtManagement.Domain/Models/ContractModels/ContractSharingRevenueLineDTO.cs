using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ContractSharingRevenueLineDTO
    {
        public int Id { get; set; } 
        public string CurrencyUnitCode { get; set; }
        public int SharingType { get; set; }
        public int? InServiceChannelId { get; set; }
        public string InServiceChannelUid { get; set; }
        public int? OutServiceChannelId { get; set; }
        public string OutletChannelDescription { get; set; }
        public string InletChannelDescription { get; set; }
        public int? InContractId { get; set; }
        public int OutContractId { get; set; }
        public float InSharedInstallFeePercent { get; set; }
        public float InSharedPackagePercent { get; set; }
        public decimal InSharedFixedAmount { get; set; }

        public float OutSharedInstallFeePercent { get; set; }
        public float OutSharedPackagePercent { get; set; }
        public decimal OutSharedFixedAmount { get; set; }
        // Tổng giá trị phân chia thực tế
        public decimal SharedTotalAmount { get; set; }

        public string OutContractCode { get; set; }
        public string InContractCode { get; set; }
        public string CsMaintenanceUid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Thời hạn phân chia
        /// </summary>
        public int? SharingDuration { get; set; }
        public DateTime? SharingFrom { get; set; }
        public DateTime? SharingTo { get; set; }
        public bool HasStartPointSharing { get; set; }

    }
}
