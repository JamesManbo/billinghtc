using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ChannelPriceBusTableDTO
    {
        public int Id { get; set; }
        public string CurrencyUnitCode { get; set; }
        public decimal? UsageValueFrom { get; set; }
        public int? UsageValueFromUomId { get; set; }
        public decimal? UsageValueTo { get; set; }
        public int? UsageValueToUomId { get; set; }
        public decimal PriceValue { get; set; }
        public int PriceUnitUomId { get; set; }
        public bool? IsDomestic { get; set; }
        public int? ChannelId { get; set; }
        public int ReceiptVoucherLineId { get; set; }
        public decimal BasedPriceValue { get; set; }
        public decimal UsageBaseUomValueTo { get; set; }
        public decimal UsageBaseUomValueFrom { get; set; }
    }
}
