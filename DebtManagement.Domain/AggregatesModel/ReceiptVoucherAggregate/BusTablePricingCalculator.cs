using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("BusTablePricingCalculators")]
    public class BusTablePricingCalculator : Entity
    {
        public string CurrencyUnitCode { get; set; }
        public int? ChannelId { get; set; }
        public string ChannelCid { get; set; }
        public int ReceiptVoucherLineId { get; set; }
        public bool IsMainRcptVoucherLine { get; set; }
        public DateTime StartingBillingDate { get; set; }
        public DateTime Day { get; set; }
        public decimal UsageDataByBaseUnit { get; set; }
        public decimal UsageData { get; set; }
        public int UsageDataUnit { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsDomestic { get; set; }
        public int PricingType { get; set; }

        public BusTablePricingCalculator()
        {
        }

        public BusTablePricingCalculator(CUBusTablePricingCalculatorCommand command)
        {
            this.Binding(command);
        }

        public void Binding(CUBusTablePricingCalculatorCommand command)
        {
            Id = command.Id;
            ChannelId = command.ChannelId;
            ChannelCid = command.ChannelCid;
            ReceiptVoucherLineId = command.ReceiptVoucherLineId;
            //IsMainRcptVoucherLine = command.IsMainRcptVoucherLine;
            Day = command.Day;
            UsageDataByBaseUnit = command.UsageDataByBaseUnit;
            UsageData = command.UsageData;
            UsageDataUnit = command.UsageDataUnit;
            Price = command.Price;
            TotalAmount = command.TotalAmount;
            IsDomestic = command.IsDomestic;
            PricingType = command.PricingType;
            StartingBillingDate = command.StartingBillingDate;
            CurrencyUnitCode = command.CurrencyUnitCode;
        }
    }
}
