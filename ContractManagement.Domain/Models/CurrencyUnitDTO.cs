using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class CurrencyUnitDTO
    {
        public int Id { get; set; }
        public string CurrencyUnitName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string IssuingCountry { get; set; }
        public string CurrencyUnitSymbol { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PriceByCurrencyUnitDTO 
    {
        public int Id { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CurrencyUnitSymbol { get; set; }
        public decimal PriceValue { get; set; }

        public PriceByCurrencyUnitDTO()
        {
        }

        public PriceByCurrencyUnitDTO(int id, string currencyUnitName, string currencyUnitCode, string currencyUnitSymbol,  decimal priceValue )
        {
            this.Id = id;
            this.CurrencyUnitId = id;
            this.CurrencyUnitName = currencyUnitName;
            this.CurrencyUnitCode = currencyUnitCode;
            this.CurrencyUnitSymbol = currencyUnitSymbol;
            this.PriceValue = priceValue;
        }
    }
}
