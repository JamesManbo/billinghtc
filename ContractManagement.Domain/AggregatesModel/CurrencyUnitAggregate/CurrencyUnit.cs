using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate
{
    [Table("CurrencyUnits")]
    public class CurrencyUnit : Entity
    {
        public static CurrencyUnit VND = new CurrencyUnit()
        {
            Id = 1,
            CurrencyUnitName = "Đồng",
            CurrencyUnitSymbol = "đ",
            CurrencyUnitCode = "VND",
            IssuingCountry = "Việt Nam"
        };

        public static CurrencyUnit USD = new CurrencyUnit()
        {
            Id = 2,

            CurrencyUnitName = "Dollar",
            CurrencyUnitSymbol = "$",
            CurrencyUnitCode = "USD",
            IssuingCountry = "United State"
        };
        public string CurrencyUnitName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string IssuingCountry { get; set; }
        public string CurrencyUnitSymbol { get; set; }
        public string Description { get; set; }
    }
}
