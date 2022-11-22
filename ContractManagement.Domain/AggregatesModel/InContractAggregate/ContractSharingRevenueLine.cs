using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("ContractSharingRevenueLines")]
    public class ContractSharingRevenueLine : Entity
    {
        [StringLength(68)]
        public string Uid { get; set; }
        public string CurrencyUnitCode { get; set; }

        public int? InServiceChannelId { get; set; }
        [StringLength(68)]
        public string InServiceChannelUid { get; set; }
        public int? OutServiceChannelId { get; set; }
        [StringLength(68)]
        public string OutServiceChannelUid { get; set; }
        public int? InContractId { get; set; }
        public int OutContractId { get; set; }

        public float InSharedInstallFeePercent { get; set; }
        public float InSharedPackagePercent { get; set; }
        public decimal InSharedFixedAmount { get; set; }

        public float OutSharedInstallFeePercent { get; set; }
        public float OutSharedPackagePercent { get; set; }
        public decimal OutSharedFixedAmount { get; set; }

        public decimal SharedTotalAmount { get; set; }

        public string OutContractCode { get; set; }
        public string InContractCode { get; set; }
        public string CsMaintenanceUid { get; set; }
        public int SharingType { get; set; }
        public bool HasStartPointSharing { get; set; }
        /// <summary>
        /// Thời hạn phân chia
        /// </summary>
        public int? SharingDuration { get; set; }
        public DateTime? SharingFrom { get; set; }
        public DateTime? SharingTo { get; set; }

        public ContractSharingRevenueLine()
        {
        }

        public ContractSharingRevenueLine(CUContractSharingRevenueLineCommand contractSharingRevenueLine)
        {
            Id = contractSharingRevenueLine.Id;
            Uid = contractSharingRevenueLine.Uid;
            CurrencyUnitCode = contractSharingRevenueLine.CurrencyUnitCode;

            InServiceChannelId = contractSharingRevenueLine.InServiceChannelId;
            InServiceChannelUid = contractSharingRevenueLine.InServiceChannelUid;
            OutServiceChannelId = contractSharingRevenueLine.OutServiceChannelId;
            OutServiceChannelUid = contractSharingRevenueLine.OutServiceChannelUid;
            InContractId = contractSharingRevenueLine.InContractId;
            OutContractId = contractSharingRevenueLine.OutContractId;

            InSharedInstallFeePercent = contractSharingRevenueLine.InSharedInstallFeePercent;
            InSharedPackagePercent = contractSharingRevenueLine.InSharedPackagePercent;
            InSharedFixedAmount = contractSharingRevenueLine.InSharedFixedAmount;
            
            OutSharedInstallFeePercent = contractSharingRevenueLine.OutSharedInstallFeePercent;
            OutSharedPackagePercent = contractSharingRevenueLine.OutSharedPackagePercent;
            OutSharedFixedAmount = contractSharingRevenueLine.OutSharedFixedAmount;

            SharingType = contractSharingRevenueLine.SharingType;
            IsActive = true;
            CreatedDate = contractSharingRevenueLine.CreatedDate;
            CreatedBy = contractSharingRevenueLine.CreatedBy;

            CsMaintenanceUid = contractSharingRevenueLine.CsMaintenanceUid;
            OutContractCode = contractSharingRevenueLine.OutContractCode;
            InContractCode = contractSharingRevenueLine.InContractCode;
            HasStartPointSharing = contractSharingRevenueLine.HasStartPointSharing;

            SharedTotalAmount = contractSharingRevenueLine.SharedTotalAmount;
            SharingDuration = contractSharingRevenueLine.SharingDuration;
            SharingFrom = contractSharingRevenueLine.SharingFrom;
            SharingTo = contractSharingRevenueLine.SharingTo;
        }
    }
}
