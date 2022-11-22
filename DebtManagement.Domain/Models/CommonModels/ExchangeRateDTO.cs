using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.CommonModels
{
    public class ExchangeRateDTO
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Buy { get; set; }
        public string Transfer { get; set; }
        public string Sell { get; set; }
        public double BuyValue { get; set; }
        public double TransferValue { get; set; }
        public double SellValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
