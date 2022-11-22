using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.PaymentVoucherModels
{
    public class TotalRevenueEnterpiseDTO
    {
        //  Khách hàng
        public string ContractorFullName { get; set; }
        //  Số hợp đồng
        public string ContractCode { get; set; }
        //  Danh mục khách hàng
        public string ContractorCategoryName { get; set; }
        //  Dự án
        public string ProjectName { get; set; }
        //  Vùng miền
        public string MarketAreaName { get; set; }
        //  Kỳ thanh toán
        public int TimeLinePaymentPeriod { get; set; }
        //  Lĩnh vực
        public string ContractorIndustryNames { get; set; }
        //  Loại dịch vụ
        public string ServiceName { get; set; }
        //  TLK cuối tháng trước(sau VAT)
        public decimal OpeningDebtAmount { get; set; }
        //  Tiền phát sinh trong tháng
        public decimal GrandTotal { get; set; }
        //  Số H.Đơn
        public string InvoiceCode { get; set; }
        //  Ngày xuất H.Đơn
        public DateTime? InvoiceDate { get; set; }
        //  Ngày nhận H.Đơn
        public DateTime? InvoiceReceivedDate { get; set; }
        //  TLK tháng(sau VAT)
        public decimal GrandTotalIncludeDebt { get; set; }
        //  Thanh toán thực tế
        public decimal PaidTotal { get; set; }
        //  Ngày thanh toán
        public DateTime? PaymentDate { get; set; }
        //  Ngày bù trừ
        public DateTime? ClearingDate { get; set; }
        //  GT bù trừ
        public decimal ClearingTotal { get; set; }
        //  Tổng lũy kế tháng hiện tại -- check
        public decimal RemainingTotal { get; set; }
        //  Trạng thái
        public string Status { get; set; }
        //  Nội dung thanh toán
        public string Content { get; set; }
        //  Nhân viên KD phục trách
        public string SignedUserName { get; set; }
        //  Khối KD phụ trách
        public string OrganizationUnitName { get; set; }
        //  CS phụ trách
        public string CustomerCareStaffUser { get; set; }

        //Khách hàng	Số hợp đồng	Danh mục khách hàng	Dự án	Vùng miền	Kỳ thanh toán	Lĩnh vực	Loại dịch vụ	TLK cuối tháng trước(sau VAT)	Tiền phát sinh trong tháng	Số H.Đơn	Ngày xuất H.Đơn	Ngày nhận H.Đơn	TLK tháng(sau VAT)	Thanh toán thực tế 	Ngày thanh toán	Ngày bù trừ	GT bù trừ	Tổng lũy kế tháng hiện tại	Trạng thái	Nội dung thanh toán	Nhân viên KD phục trách	Khối KD phụ trách	CS phụ trách
    
    }
    public class TotalRevenuePersonalDTO
    {
        //Miền	
        public string MarketAreaName { get; set; }
        //Bộ phận kíHĐ	
        //Ban/ trung tâm ký hợp đồng	
        public string OrganizationUnitName { get; set; }
        //CS phụ trách	
        public string CustomerCareStaffUser { get; set; }
        //Kinh doanh phụ trách	
        public string SignedUserName { get; set; }
        //Phạm vi kênh	
        //Nhóm khách hàng
        //Kiểu KH	
        public string ContractorGroupName { get; set; }
        //Hạng khách hàng	
        public string ContractorClassName { get; set; }
        //Phân khúc khách hàng	
        public string ContractorIndustryNames { get; set; }
        //Tên khách hàng	
        public string ContractorFullName { get; set; }
        //mã HĐ	
        //Số HĐ	
        public string ContractCode { get; set; }
        //Ngày ký	
        public DateTime? TimeLineSigned { get; set; }
        //Ngày nghiệm thu
        public DateTime TimeLineEffective { get; set; }
        //Ngày tính cước
        public DateTime? TimeLineStartBillingDate { get; set; }
        //Phụ lục	
        public string TransactionCode { get; set; }
        //Ngày ký phụ lục	
        public DateTime TransactionDate { get; set; }
        //Tình trạng HĐ	
        public string ContractSatus { get; set; }
        //Chi tiết triển khai CCDV	
        public string TransactionStatus { get; set; }
        //Tình trạng CCDV	
        //Ngày thanh lý/giảm giá	
        public string TransactionEffectedDate { get; set; }
        // TransactionDate when TransactionType = thanh ly
        //Lý do thanh lý/giảm giá/tạm ngưng	
        public string ReasonCancelAcceptance { get; set; }
        //Thanh lý trước hạn
        public bool IsEndBeforeExpriedDate { get; set; }
        //Doanh thu mất đi do thanh lý (theo term HĐ)	
        public decimal AmountLost { get; set; }
        //Thời hạn HĐ	
        public DateTime TimeLineExpiration { get; set; }
        //CID / User	
        public string CID { get; set; }
        //Điểm đầu	
        public string StartPoint { get; set; }
        //Điểm cuối	
        public string EndPoint { get; set; }
        //Băng thông trong nước (M)	
        public float DomesticBandwidth { get; set; }
        public float DomesticBandwidthUom { get; set; }
        //Băng thông quốc tế (M)	
        public float InternationalBandwidth { get; set; }
        public float InternationalBandwidthUom { get; set; }
        //Nhóm dịch vụ	
        public string ServiceGroupName { get; set; }
        //Loại dịch vụ	
        public string ServiceName { get; set; }
        //Chi phí khác	
        public decimal OtherFee { get; set; }
        //Cước cài đặt 	
        public decimal InstallationFee { get; set; }
        //Cước hàng tháng 	
        public decimal PackagePrice { get; set; }
        //Tổng GTHĐ (term 12 tháng) VND(Trước VAT) "
        public decimal TermTotal { get; set; }
        //Doanh thu nền (là những hợp đồng còn hoạt động từ năm trước= cước tháng * 12 ) 	
        public decimal LastYearSales { get; set; }
        //Doanh thu tháng 1	
        public decimal JanuaryTotal { get; set; }
        //Doanh thu phát triển mới tháng 1	
        public decimal JanuaryNewTotal { get; set; }
        //Doanh thu tháng 2	
        public decimal FebruaryTotal { get; set; }
        //Doanh thu phát triển mới tháng 2	
        public decimal FebruaryNewTotal { get; set; }
        //Doanh thu tháng 3	
        public decimal MarTotal { get; set; }
        //Doanh thu phát triển mới tháng 3	
        public decimal MarNewTotal { get; set; }
        //Doanh thu tháng 4
        public decimal AprTotal { get; set; }
        //Doanh thu phát triển mới tháng 4
        public decimal AprNewTotal { get; set; }
        //Doanh thu tháng 5
        public decimal MayTotal { get; set; }
        //Doanh thu phát triển mới tháng 5
        public decimal MayNewTotal { get; set; }
        //Doanh thu tháng 6
        public decimal JunTotal { get; set; }
        //Doanh thu phát triển mới tháng 6	
        public decimal JunNewTotal { get; set; }

        //Doanh thu tháng 7
        public decimal JulTotal { get; set; }
        //Doanh thu phát triển mới tháng 7
        public decimal JulNewTotal { get; set; }
        //Doanh thu tháng 8
        public decimal AugTotal { get; set; }
        //Doanh thu phát triển mới tháng 8
        public decimal AugNewTotal { get; set; }
        //Doanh thu tháng 9
        public decimal SepTotal { get; set; }
        //Doanh thu phát triển mới tháng 9
        public decimal SepNewTotal { get; set; }
        //Doanh thu tháng 10
        public decimal OctTotal { get; set; }
        //Doanh thu phát triển mới tháng 10
        public decimal OctNewTotal { get; set; }
        //Doanh thu tháng 11
        public decimal NovTotal { get; set; }
        //Doanh thu phát triển mới tháng 11
        public decimal NovNewTotal { get; set; }
        //Doanh thu tháng 12
        public decimal DecTotal { get; set; }
        //Doanh thu phát triển mới tháng 12
        public decimal DecNewTotal { get; set; }

        //Tổng doanh thu 12 tháng(chưa VAT)
        //	Doanh thu giảm nền ( chưa VAT)	
        public decimal LastYearSaleLost { get; set; }
        //	Doanh thu phát triển mới ( chưa VAT)	
        public decimal NewRevenueTotal { get; set; }
        //	Tổng doanh thu 12 tháng (VAT)	
        public decimal CurrentYearSales { get; set; }
        //	Tổng đã thu tiền ( VAT)	
        public decimal PaidTotal { get; set; }
        //	Tổng đã bù trừ ( VAT)
        public decimal ClearingTotal { get; set; }
        //	Tồn phải thu ( VAT)
        public decimal DebtTotal { get; set; }

    }

    public class FTTHProjectRevenueDTO
    {
        //Tuần cập nhật
        public int UpdateWeek { get; set; }
        //Miền
        public string MarketAreaName { get; set; }
        //Tỉnh/Thành
        public string CityName { get; set; }
        //Năm hợp đồng
        public int ContractYear { get; set; }
        //Tên dự án
        public string ProjectName { get; set; }
        //Tình trạng hoạt động
        public string StatusName { get; set; }
        //Số điện thoại
        public string PhoneNumber { get; set; }
        //Địa chỉ
        public string Address { get; set; }
        //Mã KH/CID
        public string CustomerCode { get; set; }
        //Số HĐ
        public string ContractCode { get; set; }
        //CS phụ trách
        public string CustomerCareStaffUser { get; set; }
        //Gói cước
        public string ServicePackageName { get; set; }
        //Trả trước
        public decimal Prepay { get; set; }
        //Số tháng được tặng trong 1 năm
        public int MonthsGiven { get; set; }
        //Số tháng dùng
        public int MonthsUse { get; set; }
        //Cước hợp đồng hàng tháng
        public decimal MonthlyContractFee { get; set; }
        //Cước tháng Internet
        public decimal InternetMonthlyFee { get; set; }
        //Cước tháng truyền hình
        public int TVMonthlyfee { get; set; }
        //Tỷ lệ chia sẻ cho đối tác phần Internet
        public float ShareRateForInternet { get; set; }
        //Tỷ lệ chia sẻ cho đối tác phần truyền hình
        public float ShareRateForTV { get; set; }
        //Cước HTC được nhận hàng thángtrấn
        public decimal HTCChargesReceivedMonthly { get; set; }
        //Cước HTC được nhận được TB sau khi trừ đi khuyến mãi
        public decimal HTCChargesReceivedAfterPromotion { get; set; }
        //Cước đã thu lần 1
        public decimal ChargesCollected1st { get; set; }
        //Chủ thể xuất hóa đơn
        public string InvoiceIssuer { get; set; }
        //Ngày ký HĐ/lắp đặt
        public DateTime ContractSigningDate { get; set; }
        //Ngày nghiệm thu
        public DateTime DateOfAcceptance { get; set; }
        //Ngày hết hạn theo term 12 tháng
        public DateTime ExpirationDate { get; set; }
        //Ngày thanh lý
        public DateTime LiquidationDate { get; set; }
        //Thanh lý(trước hạn hay sau hạn)
        public bool IsAfterTerm { get; set; }
        //Doanh thu mất đi do thanh lý(theo term HĐ)
        public decimal LossOfRevenueDueToLiquidation { get; set; }
        //Chi tiết lý do thanh lý
        public string DetailReasonForLiquidation { get; set; }
        //Phân loại thanh lý
        public int LiquidationType { get; set; }
        //Nền 2021
        public decimal PreYearEconomy { get; set; }
        //Giảm nền 2021
        public decimal EconomyReductionLastYear { get; set; }
        //Doanh thu phát triển mới
        public decimal NewGrowthRevenue { get; set; }
        //Tổng doanh thu 2021
        public decimal TotalRevenue { get; set; }
        //Tháng 1
        public decimal JanRevenue { get; set; }
        //Tháng 2
        public decimal FebRevenue { get; set; }
        //Tháng 3
        public decimal MarRevenue { get; set; }
        //Tháng 4
        public decimal AprRevenue { get; set; }
        //Tháng 5
        public decimal MayRevenue { get; set; }
        //Tháng 6
        public decimal JunRevenue { get; set; }
        //Tháng 7
        public decimal JulRevenue { get; set; }
        //Tháng 8
        public decimal AugRevenue { get; set; }
        //Tháng 9
        public decimal SepRevenue { get; set; }
        //Tháng 10
        public decimal OctRevenue { get; set; }
        //Tháng 11
        public decimal NovRevenue { get; set; }
        //Tháng 12
        public decimal DecRevenue { get; set; }
        //Ghi chú
        public string Note { get; set; }
    }
}
