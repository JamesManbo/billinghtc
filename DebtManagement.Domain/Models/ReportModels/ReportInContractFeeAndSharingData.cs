using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ReportInContractFeeAndSharing
    {
        public ReportInContractFeeAndSharing()
        {

        }
        public string OutContractCode { get; set; }
        public string InContractCode { get; set; }
        public decimal SumGrandTotal { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
    }

    public class ReportInContractFeeAndSharingByCurrency
    {
        public ReportInContractFeeAndSharingByCurrency() {
            TypeId1 = 1;
            TypeName1 = "Thuê kênh";
            SumGrandTotal1 = 0;
            TypeId2 = 2;
            TypeName2 = "Phân chia hoa hồng";
            SumGrandTotal2 = 0;
            TypeId3 = 3;
            TypeName3 = "Phân chia doanh thu";
            SumGrandTotal3 = 0;
            TypeId4 = 4;
            TypeName4 = "Bảo trì, bảo dưỡng";
            SumGrandTotal4 = 0;
        }
        public string OutContractCode { get; set; }
        public string InContractCode { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int TypeId1 { get; set; }
        public string TypeName1 { get; set; }
        public decimal SumGrandTotal1 { get; set; }
        public int TypeId2 { get; set; }
        public string TypeName2 { get; set; }
        public decimal SumGrandTotal2 { get; set; }
        public int TypeId3 { get; set; }
        public string TypeName3 { get; set; }
        public decimal SumGrandTotal3 { get; set; }
        public int TypeId4 { get; set; }
        public string TypeName4 { get; set; }
        public decimal SumGrandTotal4 { get; set; }
        public decimal Total { get; set; }
    }
}
