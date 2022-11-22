using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ChannelPriceBusTables")]
    public class ChannelPriceBusTable: Entity
    {
        public int? ChannelId { get; set; }
        public int ReceiptVoucherLineId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public decimal? UsageValueFrom { get; set; }
        public decimal? UsageBaseUomValueFrom { get; set; }
        public int? UsageValueFromUomId { get; set; }
        public decimal? UsageValueTo { get; set; }
        public decimal? UsageBaseUomValueTo { get; set; }
        public int? UsageValueToUomId { get; set; }
        public decimal BasedPriceValue { get; set; }
        public decimal PriceValue { get; set; }
        public int PriceUnitUomId { get; set; }
        public bool? IsDomestic { get; set; }

        public ChannelPriceBusTable()
        {
        }
        public ChannelPriceBusTable(CUChannelPriceBusTableCommand createCommand)
        {
            this.Binding(createCommand);
        }

        public void Binding(CUChannelPriceBusTableCommand command)
        {
            this.Id = command.Id;
            this.CurrencyUnitCode = command.CurrencyUnitCode;
            this.UsageValueFrom = command.UsageValueFrom;
            this.UsageValueFromUomId = command.UsageValueFromUomId;
            this.UsageValueTo = command.UsageValueTo;
            this.UsageValueToUomId = command.UsageValueToUomId;
            this.PriceValue = command.PriceValue;
            this.PriceUnitUomId = command.PriceUnitUomId;
            this.IsDomestic = command.IsDomestic;
            this.ChannelId = command.ChannelId;
            this.ReceiptVoucherLineId = command.ReceiptVoucherLineId;
            this.BasedPriceValue = command.BasedPriceValue;
            this.UsageBaseUomValueFrom = command.UsageBaseUomValueFrom;
            this.UsageBaseUomValueTo = command.UsageBaseUomValueTo;
        }
    }
}
