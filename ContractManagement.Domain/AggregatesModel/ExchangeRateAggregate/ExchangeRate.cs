using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate
{
    [Table("ExchangeRates")]
    public class ExchangeRate : Entity
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Buy { get; set; }
        public double BuyValue { get; set; }
        public string Transfer { get; set; }
        public double TransferValue { get; set; }
        public string Sell { get; set; }
        public double SellValue { get; set; }
    }
}
