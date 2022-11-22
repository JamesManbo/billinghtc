using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ExportInvoiceFilesModels
{
    public class ExportInvoiceFileModel
    {
        public ExportInvoiceFileModel()
        {
            ReceiptVoucherLines = new List<ReceiptVoucherLines>();
        }
        public string CustomerCode { get; set; }
        public string CategoryName { get; set; }
        public string FullName { get; set; }
        public string ContractCode { get; set; }
        public string MarketAreaName { get; set; }
        public string TimeLinePaymentPeriod { get; set; }
        public string ServiceGroupName { get; set; }
        public string VoucherCode { get; set; }
        public string InvoiceCode { get; set; }
        public string Content { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceReceivedDate { get; set; }
        public string PaidTotal { get; set; }
        public string PaymentDate { get; set; }
        public string LuyKeThang { get; set; }
        public string Note { get; set; }
        public string StatusName { get; set; }

        //
        public decimal GrandTotal { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal RemainingTotal { get; set; }

        public int NumberBillingLimitDays { get; set; }

        public string DateBillingLimit { get; set; }
        public string Today { get; set; }
        public int NumberDaysOverdue { get; set; }
        public int OverdueReceivables { get; set; }
        public int NotExpired { get; set; }
        public int OverdueLess30 { get; set; }
        public int OverdueFrom30To60 { get; set; }
        public int OverdueFrom60To90 { get; set; }
        public int OverdueFrom90To365 { get; set; }
        public int OverdueOver365 { get; set; }
        public string Reason { get; set; }
        public decimal TargetDebtRemaningTotal { get; set; }
        public string Description { get; set; }
        public List<ReceiptVoucherLines> ReceiptVoucherLines { get; set; }
        public int Id { get; set; }
        public int StatusId { get; set; }
        public bool IsEnterprise { get; set; }
    }
    public class CountReceiptVoucher
    {
        public int TotalPersonal { get; set; }
        public int TotalPersonalUnCollected { get; set; }
        public int TotalPersonalCollected { get; set; }
        public int TotalEnterpriseUnCollected { get; set; }
        public int TotalEnterpriseCollected { get; set; }
        public int TotalEnterpriseExportInVoice { get; set; }
        public int TotalEnterpriseUnExportInvoice { get; set; }
        public int TotalEnterprise { get; set; }
    }

    public class ReceiptVoucherLines
    {
        public string ServiceGroupName { get; set; }
    }
}
