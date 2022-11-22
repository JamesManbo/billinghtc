using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DebtManagement.Domain.AggregatesModel.BaseVoucher
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

        public decimal Round(decimal number)
        {
            if (number <= 0) return 0;
            if (this.Id == VND.Id)
            {
                return Math.Round(number);
            }
            else
            {
                return Math.Round(number, 2);
            }
        }

        public static decimal RoundByCurrency(int currencyId, decimal number)
        {
            var currency = List().FirstOrDefault(c => c.Id == currencyId);
            if (currency == null) return number;

            return currency.Round(number);
        }

        public static List<CurrencyUnit> List()
        {
            return new List<CurrencyUnit>
            {
                VND,
                USD
            };
        }
    }
}
