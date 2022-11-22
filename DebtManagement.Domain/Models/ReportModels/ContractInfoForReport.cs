using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ContractInfoForReport
    {
        public ContractInfoForReport(){
            servicePackages = new List<ServicePackage>();
            receiptVouchers = new List<ReceiptVoucherForReport>();
        }
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
        public decimal RemainingTotal { get; set; }

        public List<ServicePackage> servicePackages { get; set; }
        public List<ReceiptVoucherForReport> receiptVouchers { get; set; }
    }
}
