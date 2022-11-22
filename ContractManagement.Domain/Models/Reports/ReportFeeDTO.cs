using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class ReportFeeDTO
    {
        ////public string IsEnterprise { get; set; }
        ////public string InContractCode { get; set; }
        //public string ServiceName { get; set; }
        //public string InputChannelName { get; set; }
        //public decimal OutContractValue { get; set; }
        //public decimal TotalFee { get; set; }
        ////public string OutContractCode { get; set; }
        ////public string OutContractorName { get; set; }
        //public string AgentCode { get; set; }
        ////public string AgentContractName { get; set; }
        ////public string OrganizationUnitName { get; set; }
        ////public string SignedUserName { get; set; }
        //public string CurrencyUnitCode { get; set; }

        //Khu vực
        public string MarketAreaName { get; set; }
        //Khách hàng trong nước và quốc tế
        public string CustomerCategoryName { get; set; }
        //Tên khách hàng
        public string CustomerFullName { get; set; }
        //Số HĐ đầu vào
        public string InContractCode { get; set; }
        //Khách hàng đầu ra 
        public string OutContractorName { get; set; }
        //Số hợp đồng đầu ra
        public string OutContractCode { get; set; }
        //Tình trạng hợp đồng(theo HĐ đầu ra)
        public string OutContractStatus { get; set; }
        //Ngày ký 
        public DateTime TimeLineSigned { get; set; }
        //Ngày nghiệm thu
        public DateTime TimeLineEffective { get; set; }
        //Ngày thanh lý
        public DateTime TimeLineLiquidation { get; set; }
        //Thời hạn hợp đồng
        public int TimeLineRenewPeriod { get; set; }
        //Kỳ thanh toán
        public int TimeLinePaymentPeriod { get; set; }
        //Hình thức thanh toán
        public string PaymentForm { get; set; }
        //Chi tiết dịch vụ(băng thông, sợi, đường truyền)
        public string ServiceDetails { get; set; }
        //Mã CID(Thuê kênh đầu vào)
        public string CId { get; set; }
        //Điểm đầu 
        public string StartPoint { get; set; }
        //Điểm cuối
        public string EndPoint { get; set; }
        //Cước lắp đặt
        public decimal InstallationFee { get; set; }
        //Tháng 1(VND)
        public decimal JanInVND { get; set; }
        //Tháng 1(USD)
        public decimal JanInUSD { get; set; }
        //Tháng 2(VND)
        public decimal FebInVND { get; set; }
        //Tháng 2(USD)
        public decimal FebInUSD { get; set; }
        //Tháng 3(VND)
        public decimal MarInVND { get; set; }
        //Tháng 3(USD)
        public decimal MarInUSD { get; set; }
        //Tháng 4(VND)
        public decimal AprInVND { get; set; }
        //Tháng 4(USD)
        public decimal AprInUSD { get; set; }
        //Tháng 5(VND)
        public decimal MayInVND { get; set; }
        //Tháng 5(USD)
        public decimal MayInUSD { get; set; }
        //Tháng 6(VND)
        public decimal JunInVND { get; set; }
        //Tháng 6(USD)
        public decimal JunInUSD { get; set; }
        //Tháng 7(VND)
        public decimal JulInVND { get; set; }
        //Tháng 7(USD)
        public decimal JulInUSD { get; set; }
        //Tháng 8(VND)
        public decimal AugInVND { get; set; }
        //Tháng 8(USD)
        public decimal AugInUSD { get; set; }
        //Tháng 9(VND)
        public decimal SepInVND { get; set; }
        //Tháng 9(USD)
        public decimal SepInUSD { get; set; }
        //Tháng 10(VND)
        public decimal OctInVND { get; set; }
        //Tháng 10(USD)
        public decimal OctInUSD { get; set; }
        //Tháng 11(VND)
        public decimal NovInVND { get; set; }
        //Tháng 11(USD)
        public decimal NovInUSD { get; set; }
        //Tháng 12(VND)
        public decimal DecInVND { get; set; }
        //Tháng 12(USD)
        public decimal DecInUSD { get; set; }
        //Cả năm
        public decimal AllMonthsVND { get; set; }
        public decimal AllMonthsUSD { get; set; }


        //Loại hình chi phí
        public string SharingTypeName { get; set; }
        //Đối tác
        public string AgentContractName { get; set; }
        //KCN/Dự án
        public string ProjectName { get; set; }
        //Đối tượng HĐ đầu ra(match vs thông tin HĐ đầu ra: Cá nhân, Doanh nghiệp, CQNN)
        public string IsEnterprise { get; set; }
        
        //Giá trị cước tháng của HĐ đầu ra 
        public decimal FeeOfMonthByOutContract { get; set; }
        //Dịch vụ phân chia
        public string SharingServiceName { get; set; }
        //Tỷ lệ %(cước cài đặt)
        public decimal InSharedInstallFeePercent { get; set; }
        public decimal OutSharedInstallFeePercent { get; set; }
        //Tỷ lệ %(cước hàng tháng)
        public decimal InSharedPackagePercent { get; set; }
        public decimal OutSharedPackagePercent { get; set; }
        //Chi phí sau khi PCDT/HH cho cước cài đặt
        public decimal CostOfInstallFee { get; set; }
        //Chi phí sau khi PCDT/HH theo cước hàng tháng
        public decimal CostOfFeeInMonth { get; set; }
        //Nhân viên KD phụ trách
        public string SignedUserName { get; set; }
        //Khối KD
        public string OrganizationUnitName { get; set; }
        //Nhân viên CS phụ trách HĐ
        public string CustomerCareStaffUserName { get; set; }

        public bool HasStartAndEndPoint { get; set; }

        public TotalDataReportFeeDTO TotalDataReportFee { get; set; }
    }


    public class ReportTotalFeeByCurrency
    {
        public string InContractCode { get; set; }
        public string OutContractCode { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int ShareChannelRentalId { get; set; }
        public string ShareChannelRental { get; set; }
        public decimal ShareChannelRentalAmount { get; set; }
        public int ShareCommissionId { get; set; }
        public string ShareCommission { get; set; }
        public decimal ShareCommissionAmount { get; set; }
        public int ShareRevenueId { get; set; }
        public string ShareRevenue { get; set; }
        public decimal ShareRevenueAmount { get; set; }
        public int ShareMaintenanceId { get; set; }
        public string ShareMaintenance { get; set; }
        public decimal ShareMaintenanceAmount { get; set; }
        public decimal ShareInfrastructureAmount { get; set; }
        public decimal ShareSupplyAmount { get; set; }
        public decimal ShareEquipmentAmount { get; set; }
        public decimal ShareCostOfAcceptanceAmount { get; set; }
        public decimal ShareConstructionAmount { get; set; }
        public decimal TotalShareAmount { get; set; }

        public string ContractorName { get; set; }
        public string ServiceName { get; set; }
        public DateTime TimeLineExpiration { get; set; }
        public decimal ContractRevenue { get; set; }
    }

    public class TotalDataIncontractFeeSharingReportDto
    {
        public decimal ShareChannelRentalTotal { get; set; }
        public decimal ShareCommissionTotal { get; set; }
        public decimal ShareRevenueTotal { get; set; }
        public decimal ShareMaintenanceTotal { get; set; }
        public decimal ShareInfrastructureTotal { get; set; }
        public decimal ShareConstructionTotal { get; set; }
        public decimal ShareEquipmentTotal { get; set; }
        public decimal ShareSupplyTotal { get; set; }
        public decimal ShareCostOfAcceptanceTotal { get; set; }
        public decimal ShareOtherTotal { get; set; }
        public decimal ShareAllTotal { get; set; }
        public decimal ContractRevenueTotal { get; set; }
        
    }

    public class TotalDataReportFeeDTO
    {
        public decimal SumInstallationFee { get; set; }
        public decimal SumFeeOfMonthByOutContract { get; set; }
        public decimal SumCostOfInstallFee { get; set; }
        public decimal SumCostOfFeeInMonth { get; set; }
        public string[] SumEachMonthVND { get; set; }
        public decimal SumAllMonthsVND { get; set; }
        public string[] SumEachMonthUSD { get; set; }
        public decimal SumAllMonthsUSD { get; set; }

        public TotalDataReportFeeDTO()
        {
            SumInstallationFee = 0;
            SumEachMonthVND = null;
            SumAllMonthsVND = 0;
            SumEachMonthUSD = null;
            SumAllMonthsUSD = 0;
        }
    }
}