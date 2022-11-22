using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
	public class ReceiptVoucherSharingRevenueDTO
	{
		public int ReceiptVoucherId { get; set; }
		public string CurrencyUnitId { get; set; }
		public string CurrencyUnitCode { get; set; }
		public string VoucherCode { get; set; }
		public int OutContractId { get; set; }
		public string MarketAreaName { get; set; }
		public string ProjectName { get; set; }
		public string ContractCode { get; set; }
		public string IssuedDate { get; set; }
        public decimal CashTotal { get; set; }
        public int OutContractServicePackageId { get; set; }
		public int ServiceId { get; set; }
		public string ServiceName { get; set; }
		public decimal SPointInstallationFee { get; set; }
		public int SPointSharedInstallFeePercent { get; set; }
		public decimal SPointSharingInstallationAmount { get; set; }

		public decimal EPointInstallationFee { get; set; }
		public int EPointSharedInstallFeePercent { get; set; }
		public decimal EPointSharingInstallationAmount { get; set; }

		public int SPointSharedPackagePercent { get; set; }
		public decimal SPointMonthlyCost { get; set; }
		public decimal SPointSharingPackageAmount { get; set; }

		public int EPointSharedPackagePercent { get; set; }
		public decimal EPointMonthlyCost { get; set; }
		public decimal EPointSharingPackageAmount { get; set; }

		public decimal TotalSharingAmount { get; set; }

	}
	public class ReceiptVoucherSharingRevenue
    {
		public ReceiptVoucherSharingRevenue()
		{
			this.VoucherCode = "";
			this.OutContractId = 0;
			this.MarketAreaName = "";
			this.ContractCode = "";
			this.IssuedDate = "";
			this.ServiceSharingRevenue = new List<ServiceSharingRevenue>();
		}
        public int ReceiptVoucherId { get; set; }
        public string CurrencyUnitId { get; set; }
		public string CurrencyUnitCode { get; set; }
		public string VoucherCode { get; set; }
		public int OutContractId { get; set; }
		public string MarketAreaName { get; set; }
		public string ProjectName { get; set; }
		public string ContractCode { get; set; }
		public string IssuedDate { get; set; }
		public List<ServiceSharingRevenue> ServiceSharingRevenue { get; set; }

	}
	public class ServiceSharingRevenue
    {
		public int OutContractServicePackageId { get; set; }
		public int ServiceId { get; set; }
		public string ServiceName { get; set; }
		public decimal SPointInstallationFee { get; set; }
		public int SPointSharedInstallFeePercent { get; set; }
		public decimal SPointSharingInstallationAmount { get; set; }

		public decimal EPointInstallationFee { get; set; }
		public int EPointSharedInstallFeePercent { get; set; }
		public decimal EPointSharingInstallationAmount { get; set; }

		public int SPointSharedPackagePercent { get; set; }
		public decimal SPointMonthlyCost { get; set; }
		public decimal SPointSharingPackageAmount { get; set; }

		public int EPointSharedPackagePercent { get; set; }
		public decimal EPointMonthlyCost { get; set; }
		public decimal EPointSharingPackageAmount { get; set; }

		public decimal TotalSharingAmount { get; set; }
	}
}
