using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("ServicePackagePrice")]
    public class ServicePackagePrice : Entity
    {
        public ServicePackagePrice()
        {
            IsActive = true;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }
        public int? ChannelId { get; set; }
        public decimal PriceValue { get; set; }
        //public int? MarketAreaId { get; set; }
        //public int? ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public  string CurrencyUnitCode { get; set; }
    }
}
