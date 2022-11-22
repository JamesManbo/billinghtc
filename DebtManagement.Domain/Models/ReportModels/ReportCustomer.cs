using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ReportCustomerDto
    {
        public int ServicePackageId;

        public int ContractId { get; set; }
        public string MarketAreaName { get; set; }
        public string ProjectName { get; set; }
        public string CustomerClass { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceAdd { get; set; }
        public string TaxIdNo { get; set; }
       
        public string CustomerCode { get; set; }
        public string ContractCode { get; set; }
        public string ContractAdd { get; set; }
        public string EquipmentName { get; set; }
        public int EquipmentQuantity { get; set; }
        public string EquipmentStatus { get; set; }
        public string EquipmentSerial { get; set; }
        public bool IsReturn { get; set; }
        public string ResonNotReturn { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string PaymentAdd { get; set; }
        public DateTime TimeLineSigned { get; set; }
        public DateTime TimeLineExpiration { get; set; }
        public string Content { get; set; }
        public string PreviousMonthDebit { get; set; }
        public string PaymentInMonth { get; set; }
        public DateTime PaymentDate { get; set; }
        public string paymentMethod { get; set; }
        public string Liquidation { get; set; }
        public string isActive { get; set; }
        public string InternationalBandwidth { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InstallationAddressStartPoint { get; set; }
        public string InstallationAddressEndPoint { get; set; }
        public string InstallationAddressStreet { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public decimal PackagePrice { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public string BandwithLabel { get; set; }
        public decimal InstallationFee { get; set; }
        public DateTime? TimeLineEffective { get; set; }
        public int TimeLinePaymentPeriod { get; set; }
        public DateTime? TimeLineSuspensionStartDate { get; set; }
        public DateTime? TimeLineSuspensionEndDate { get; set; }
        public DateTime? TimeLineLatestBilling { get; set; }
        public DateTime? TimeLineRenewPeriod { get; set; }
        public int TimeLinePrepayPeriod { get; set; }
        public DateTime? TimeLineNextBilling { get; set; }
        public int TotalMonthUse { get; set; }
        public int PromotionDateQuantity { get; set; }

        public DateTime IssuedDate { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidTotal { get; set; }
        public string ReceiptVoucherId { get; set; }
        public decimal RemainingTotal { get; set; }
    }
}
