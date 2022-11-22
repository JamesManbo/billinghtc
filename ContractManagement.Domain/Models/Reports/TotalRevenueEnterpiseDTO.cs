using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
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
        public int StatusId { get; set; }
        public int? TransactionType { get; set; }
        public string TransactionTypeName
        {
            get
            {
                if (!this.TransactionType.HasValue || this.TransactionType <= 0)
                {
                    return string.Empty;
                }

                return AggregatesModel.TransactionAggregate.TransactionType.GetTypeName(this.TransactionType.Value);
            }
        }
        public string StatusName => OutContractServicePackageStatus.From(StatusId)?.ToString();
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
        public int Id { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int ReportYear { get; set; }
        //Miền
        public string MarketAreaName { get; set; }
        //Bộ phận kíHĐ
        public string ContractDepartment { get; set; }
        //Ban/ trung tâm ký hợp đồng
        public string OrganizationUnitName { get; set; }
        //CS phụ trách
        public string CustomerCareStaffUser { get; set; }
        public string CustomerCareStaffUserId { get; set; }
        //Kinh doanh phụ trách
        public string SignedUserName { get; set; }
        //Phạm vi kênh
        public string ChannelRange { get; set; }
        //Nhóm khách hàng
        public string ContractorGroupName { get; set; }
        //Kiểu KH
        public string ContractorTypeName { get; set; }
        //Hạng khách hàng
        public string ContractorClassName { get; set; }
        //Phân khúc khách hàng
        public string ContractorIndustryNames { get; set; }
        //Tên khách hàng	
        public string ContractorFullName { get; set; }
        //mã HĐ
        public string OutContractCode { get; set; }
        //Số HĐ
        public string ContractCode { get; set; }
        //Ngày ký
        public DateTime? TimeLineSigned { get; set; }
        //Ngày nghiệm thu
        public DateTime? TimeLineEffective { get; set; }
        //Ngày tính cước
        public DateTime? TimeLineStartBillingDate { get; set; }
        public int? TransactionType { get; set; }
        public string TransactionTypeName
        {
            get
            {
                if (!this.TransactionType.HasValue || this.TransactionType <= 0)
                {
                    return string.Empty;
                }

                return AggregatesModel.TransactionAggregate.TransactionType.GetTypeName(this.TransactionType.Value);
            }
        }
        //Phụ lục
        public int? TransactionId { get; set; }
        public string TransactionCode { get; set; }
        //Ngày ký phụ lục
        public DateTime? TransactionDate { get; set; }
        //Tình trạng HĐ
        public int? ContractStatus { get; set; }

        //Tình trạng CCDV
        public string ContractStatusName
        {
            get
            {
                if (this.ContractStatus.HasValue)
                {
                    return AggregatesModel.BaseContract.ContractStatus.From(this.ContractStatus.Value)
                        ?.ToString();
                }
                return "";
            }
        }
        //Chi tiết triển khai CCDV
        public string ImplementationDetails { get; set; }
        public string TransactionStatus { get; set; }
        //Ngày thanh lý/giảm giá
        public DateTime? TransactionEffectedDate { get; set; }
        public int TerminateReasonType { get; set; }
        // TransactionDate when TransactionType = thanh ly
        //Lý do thanh lý/giảm giá/tạm ngưng
        public string ReasonCancelAcceptance
        {
            get
            {
                if (this.TerminateReasonType > 0)
                {
                    return TransactionReason.
                        TerminateReasons.FirstOrDefault(
                            s => s.Id == this.TerminateReasonType
                        )?.Name;
                }
                return string.Empty;
            }
        }
        //Thanh lý trước hạn
        public string IsEndBeforeExpriedDate { get; set; }
        //Doanh thu mất đi do thanh lý (theo term HĐ)	
        public decimal AmountLost { get; set; }
        //Thời hạn HĐ	
        public string TimeLineExpiration { get; set; }
        //CID / User	
        public string CID { get; set; }
        public int? StatusId { get; set; }
        public string ServiceDeliveryStatus
        {
            get
            {
                if (this.StatusId.HasValue)
                {
                    return OutContractServicePackageStatus.From(this.StatusId.Value)?.ToString();
                }

                return "";
            }
        }
        //Điểm đầu	
        public InstallationAddress StartPoint { get; set; }
        //Điểm cuối	
        public InstallationAddress EndPoint { get; set; }
        //Băng thông trong nước (M)	
        public string DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        //Băng thông quốc tế (M)	
        public string InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
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
        public decimal JanuaryNewTotal
        {
            get
            {
                if ((this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 1 && this.TimeLineEffective?.Year == this.ReportYear)
                    || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 1 && this.TimeLineSigned?.Year == this.ReportYear))
                        return this.JanuaryTotal;
                
                return 0;
            }
        }
        //Doanh thu tháng 2	
        public decimal FebruaryTotal { get; set; }
        //Doanh thu phát triển mới tháng 2	
        public decimal FebruaryNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 2 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 2 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.FebruaryTotal : 0;
        //Doanh thu tháng 3	
        public decimal MarTotal { get; set; }
        //Doanh thu phát triển mới tháng 3	
        public decimal MarNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 3 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 3 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.MarTotal : 0;
        //Doanh thu tháng 4
        public decimal AprTotal { get; set; }
        //Doanh thu phát triển mới tháng 4
        public decimal AprNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 4 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 4 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.AprTotal : 0;
        //Doanh thu tháng 5
        public decimal MayTotal { get; set; }
        //Doanh thu phát triển mới tháng 5
        public decimal MayNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 5 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 5 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.MayTotal : 0;
        //Doanh thu tháng 6
        public decimal JunTotal { get; set; }
        //Doanh thu phát triển mới tháng 6	
        public decimal JunNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 6 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 6 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.JunTotal : 0;

        //Doanh thu tháng 7
        public decimal JulTotal { get; set; }
        //Doanh thu phát triển mới tháng 7
        public decimal JulNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 7 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 7 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.JulTotal : 0;
        //Doanh thu tháng 8
        public decimal AugTotal { get; set; }
        //Doanh thu phát triển mới tháng 8
        public decimal AugNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 8 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 8 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.AugTotal : 0;
        //Doanh thu tháng 9
        public decimal SepTotal { get; set; }
        //Doanh thu phát triển mới tháng 9
        public decimal SepNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 9 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 9 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.SepTotal : 0;
        //Doanh thu tháng 10
        public decimal OctTotal { get; set; }
        //Doanh thu phát triển mới tháng 10
        public decimal OctNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 10 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 10 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.OctTotal : 0;
        //Doanh thu tháng 11
        public decimal NovTotal { get; set; }
        //Doanh thu phát triển mới tháng 11
        public decimal NovNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 11 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 11 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.NovTotal : 0;
        //Doanh thu tháng 12
        public decimal DecTotal { get; set; }
        //Doanh thu phát triển mới tháng 12
        public decimal DecNewTotal
            => (this.TransactionId.HasValue && this.TimeLineEffective?.Month <= 12 && this.TimeLineEffective?.Year == this.ReportYear)
            || (!this.TransactionId.HasValue && this.TimeLineSigned?.Month <= 12 && this.TimeLineSigned?.Year == this.ReportYear)
            ? this.DecTotal : 0;

        //Tổng doanh thu 12 tháng(chưa VAT)
        public decimal AllMonthsTotalNotVAT { get; set; }
        //	Doanh thu giảm nền ( chưa VAT)	
        public decimal LastYearSaleLost
        {
            get
            {
                return LastYearSales - AllMonthsTotalNotVAT > 0 ? LastYearSales - AllMonthsTotalNotVAT : 0;
            }
        }
        //	Doanh thu phát triển mới ( chưa VAT)	
        public decimal NewRevenueTotal
        {
            get
            {
                return JanuaryNewTotal + FebruaryNewTotal + MarNewTotal + AprNewTotal + MayNewTotal + JunNewTotal + JulNewTotal + AugNewTotal + SepNewTotal
                        + OctNewTotal + NovNewTotal + DecNewTotal;
            }
        }
        //	Tổng doanh thu 12 tháng (VAT)	
        public decimal CurrentYearSales { get; set; }
        //	Tổng đã thu tiền ( VAT)	
        public decimal PaidTotal { get; set; }
        //	Tổng đã bù trừ ( VAT)
        public decimal ClearingTotal { get; set; }
        //	Tồn phải thu ( VAT)
        public decimal DebtTotal { get; set; }
        public TotalDataRevenuePersonalDTO TotalDataRevenuePersonalDTO { get; set; }
        public List<MonthValue> MonthValues
        {
            get
            {
                return new List<MonthValue>()
                {
                    new MonthValue()
                    {
                        Month = 1,
                        Name = "Jan",
                        Revenue = 0,
                        NewRevenue = 0,
                    },
                     new MonthValue()
                    {
                        Month = 2,
                        Name = "Feb",
                        Revenue = 0,
                        NewRevenue = 0,
                    },
                      new MonthValue()
                    {
                        Month = 3,
                        Name = "Mar",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 4,
                        Name = "April",
                        Revenue = 0,
                        NewRevenue = 0,
                    }
                    , new MonthValue()
                    {
                        Month = 5,
                        Name = "May",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 6,
                        Name = "Jun",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 7,
                        Name = "Jul",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 8,
                        Name = "Aug",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 9,
                        Name = "Sep",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 10,
                        Name = "Oct",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 11,
                        Name = "Nov",
                        Revenue = 0,
                        NewRevenue = 0,
                    }, new MonthValue()
                    {
                        Month = 12,
                        Name = "Dec",
                        Revenue = 0,
                        NewRevenue = 0,
                    },
                };
            }
        }

    }
    public class TotalDataRevenuePersonalDTO
    {
        public int TotalRecords { get; set; }
        public decimal SumAmountLost { get; set; }
        public decimal SumOtherFee { get; set; }
        public decimal SumInstallationFee { get; set; }
        public decimal SumPackagePrice { get; set; }
        public decimal SumTermTotal { get; set; }
        public decimal SumLastYearSales { get; set; }
        public string SumJoinedEachMonthRevenue { get; set; }
        public string[] SumEachMonthTotal { get; set; }
        public string SumJoinedEachMonthNewRevenue { get; set; }
        public string[] SumEachMonthNewTotal { get; set; }
        public decimal SumAllMonthsTotalNotVAT { get; set; }
        public decimal SumLastYearSaleLost => this.SumLastYearSales > this.SumAllMonthsTotalNotVAT
            ? this.SumLastYearSales - this.SumAllMonthsTotalNotVAT
            : 0;
        public decimal SumNewRevenueTotal { get; set; }
        public decimal SumCurrentYearSales { get; set; }
        public decimal SumPaidTotal { get; set; }
        public decimal SumClearingTotal { get; set; }
        public decimal SumDebtTotal { get; set; }

        public TotalDataRevenuePersonalDTO()
        {
            SumAmountLost = 0;
            SumOtherFee = 0;
            SumInstallationFee = 0;
            SumPackagePrice = 0;
            SumTermTotal = 0;
            SumLastYearSales = 0;
            SumEachMonthTotal = null;
            SumEachMonthNewTotal = null;
            SumAllMonthsTotalNotVAT = 0;
            SumNewRevenueTotal = 0;
            SumCurrentYearSales = 0;
            SumPaidTotal = 0;
            SumClearingTotal = 0;
            SumDebtTotal = 0;
        }
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
        //Loại khách hàng
        public string ContractorTypeName { get; set; }
        //Tên khách hàng
        public string ContractorFullName { get; set; }
        public int StatusId { get; set; }
        public string StatusName => OutContractServicePackageStatus.From(StatusId)?.ToString();
        public int ContractStatusId { get; set; }
        public string CId { get; set; }
        //Số điện thoại
        public string PhoneNumber { get; set; }
        //Địa chỉ
        public string Address { get; set; }
        //Mã KH/CID
        public string CustomerCode { get; set; }
        //Số HĐ
        public string ContractCode { get; set; }
        //Kinh doanh phụ trách
        public string SignedUserName { get; set; }
        //CS phụ trách
        public string CustomerCareStaffUser { get; set; }
        //Gói cước
        public string ServicePackageName { get; set; }
        //Trả trước
        public string Prepay { get; set; }
        //Số tháng được tặng trong 1 năm
        public string MonthsGiven { get; set; }
        //Số tháng dùng
        public string MonthsUse { get; set; }
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
        //Cước HTC được nhận hàng tháng
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
        public DateTime? DateOfAcceptance { get; set; }
        //Ngày bắt đầu tính cước
        public DateTime? DateOfBilling { get; set; }
        //Ngày hết hạn theo term 12 tháng
        public DateTime ExpirationDate { get; set; }
        //Ngày thanh lý
        public DateTime? LiquidationDate { get; set; }
        //Thanh lý(trước hạn hay sau hạn)
        public string IsAfterTerm { get; set; }
        //Doanh thu mất đi do thanh lý(theo term HĐ)
        public decimal LossOfRevenueDueToLiquidation { get; set; }
        public string DetailReasonForLiquidation { get; set; }
        //Phân loại thanh lý
        public int LiquidationType { get; set; }
        public string LiquidationReason
        {
            get
            {
                if (this.LiquidationType > 0)
                {
                    return TransactionReason.
                        TerminateReasons.FirstOrDefault(
                            s => s.Id == this.LiquidationType
                        )?.Name;
                }
                return string.Empty;
            }
        }
        //Nền
        public decimal PreYearEconomy { get; set; }
        //Doanh thu phát triển mới
        public decimal NewGrowthRevenue { get; set; }
        //Tổng doanh thu
        public decimal TotalRevenue { get; set; }
        //Giảm nền
        public decimal EconomyReductionLastYear => PreYearEconomy > 0 ? (PreYearEconomy - TotalRevenue) : 0;
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

        public TotalDataFTTHProjectRevenueDTO TotalDataFTTHProjectRevenueDTO { get; set; }
    }

    public class TotalDataFTTHProjectRevenueDTO
    {
        public int TotalRecords { get; set; }
        public decimal SumMonthlyContractFee { get; set; }
        public decimal SumInternetMonthlyFee { get; set; }
        public decimal SumTVMonthlyfee { get; set; }
        public decimal SumHTCChargesReceivedMonthly { get; set; }
        public decimal SumHTCChargesReceivedAfterPromotion { get; set; }
        public decimal SumChargesCollected1st { get; set; }
        public decimal SumLossOfRevenueDueToLiquidation { get; set; }
        public decimal SumPreYearEconomy { get; set; }
        public decimal SumEconomyReductionLastYear { get; set; }
        public decimal SumNewGrowthRevenue { get; set; }
        public decimal SumTotalRevenue { get; set; }
        public decimal NewGrowthRevenue { get; set; }
        public string SumJoinedEachMonthRevenue { get; set; }
        public string[] SumEachMonthRevenue { get; set; }

        public TotalDataFTTHProjectRevenueDTO()
        {
            SumMonthlyContractFee = 0;
            SumInternetMonthlyFee = 0;
            SumTVMonthlyfee = 0;
            SumHTCChargesReceivedMonthly = 0;
            SumHTCChargesReceivedAfterPromotion = 0;
            SumChargesCollected1st = 0;
            SumLossOfRevenueDueToLiquidation = 0;
            SumPreYearEconomy = 0;
            SumEconomyReductionLastYear = 0;
            SumNewGrowthRevenue = 0;
            SumTotalRevenue = 0;
            TotalRecords = 0;
        }
    }

    public class DebtDTO
    {
        public int OutContractServicePackageId { get; set; }
        public decimal PaidTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal DebtTotal { get; set; }

    }

    public class OutContractReportBasicDTO
    {
        public OutContractReportBasicDTO()
        {
            TransactionBasicDTOs = new List<TransactionBasicDTO>();
        }
        public int Id { get; set; }
        public List<TransactionBasicDTO> TransactionBasicDTOs { get; set; }
    }
    public class TransactionServicePackageBasicDTO
    {
        public int Id { get; set; }
        public bool IsOld { get; set; }
        public DateTime? TimeLineEffective { get; set; }
        public DateTime? TimeLineTerminateDate { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal PackagePrice { get; set; }
    }
    public class TransactionBasicDTO
    {
        public TransactionBasicDTO()
        {
            TransactionServicePackages = new List<TransactionServicePackageBasicDTO>();
        }
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<TransactionServicePackageBasicDTO> TransactionServicePackages { get; set; }

    }
    public class MonthValue
    {

        public int Month { get; set; }
        public string Name { get; set; }
        public decimal Revenue { get; set; }
        public decimal NewRevenue { get; set; }
    }
}
