using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class BusTablePricingCalculatorDTO : BaseDTO
    {
        public int? ChannelId { get; set; }
        public string ChannelCid { get; set; }
        public bool IsMainRcptVoucherLine { get; set; }
        public int ReceiptVoucherLineId { get; set; }
        public DateTime StartingBillingDate { get; set; }
        public DateTime Day { get; set; }
        public decimal UsageDataByBaseUnit { get; set; }
        public decimal UsageData { get; set; }
        public int UsageDataUnit { get; set; }
        public decimal PriceAmount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsDomestic { get; set; }
        public int PricingType { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}
