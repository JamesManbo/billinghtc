using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class OutContractInfoForReport
    {
        public OutContractInfoForReport()
        {
            ServicePackages = new List<ServicePackageForMasterReport>();
            InContractInfos = new List<InContractInfoForReport>();
        }
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public int WeekReport { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string ContractCode { get; set; }
        public string MarketAreaName { get; set; }
        public string ProjectName { get; set; }
        public string SignedUserName { get; set; }

        public string CustomerCategory { get; set; }
        public string GroupName { get; set; }
        public string CustomerStruct { get; set; }
        public string CustomerType { get; set; }
        public string CustomerClass { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractorFullName { get; set; }

        public DateTime TimeLineSigned { get; set; }
        public DateTime TimeLineExpiration { get; set; }

        public decimal InstallationFee { get; set; }
        public decimal InstallationFeeUSD { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal PackagePriceUSD { get; set; }

        public int PaymentTerm { get; set; }
        public string ContractType { get; set; }
        public string CId { get; set; }

        public string TypeRevenue { get; set; }
        public string TypeServiceName { get; set; }


        public string InContractCode { get; set; }
        public string Contractor { get; set; }

        public string Description { get; set; }
        public string CustomerCare { get; set; }
        public string CustomerBusiness { get; set; }
        public string OrganizationUnitName { get; set; }

        public decimal LastYearAmount { get; set; }
        public decimal LostRevenue { get; set; }
        public decimal ValueMonth1 { get; set; }
        public decimal ValueMonth2 { get; set; }
        public decimal ValueMonth3 { get; set; }
        public decimal ValueMonth4 { get; set; }
        public decimal ValueMonth5 { get; set; }
        public decimal ValueMonth6 { get; set; }
        public decimal ValueMonth7 { get; set; }
        public decimal ValueMonth8 { get; set; }
        public decimal ValueMonth9 { get; set; }
        public decimal ValueMonth10 { get; set; }
        public decimal ValueMonth11 { get; set; }
        public decimal ValueMonth12 { get; set; }
        public decimal ValueYearNow { get; set; }
        public int Total { get; set; }

        public List<ServicePackageForMasterReport> ServicePackages { get; set; }
        public List<InContractInfoForReport> InContractInfos { get; set; }
    }
}
