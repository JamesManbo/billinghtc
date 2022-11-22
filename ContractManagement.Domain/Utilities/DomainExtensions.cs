using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Utilities
{
    public static class DomainExtensions
    {
        public static decimal RoundByCurrency(this decimal source, string currencyCode, int decimalDigits = 2)
        {
            if (source <= 0 || string.IsNullOrWhiteSpace(currencyCode))
            {
                return source;
            }

            switch (currencyCode.ToUpper())
            {
                case "VND":
                    return Math.Round(source);
                default:
                    return Math.Round(source, decimalDigits);
            }
        }
    }
}
