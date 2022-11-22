using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class CUBusTablePricingCalculatorCommand : IRequest<ActionResponse>
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string DomesticBandwidth { get; set; }
        public string InternationalBandwidth { get; set; }

        public int ChannelId { get; set; }
        public string ChannelCid { get; set; }
        public bool IsMainRcptVoucherLine { get; set; }
        public int ReceiptVoucherLineId { get; set; }
        public int? Month { get; set; }
        public DateTime Day { get; set; }
        public DateTime StartingBillingDate { get; set; }
        public decimal UsageDataByBaseUnit { get; set; }
        public decimal UsageData { get; set; }
        public int UsageDataUnit { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsDomestic { get; set; }
        public int PricingType { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}
