using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Seed;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    public abstract class DeploymentChannelPackagePriceBusTable : Entity
    {
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

        public DeploymentChannelPackagePriceBusTable()
        {
            PriceUnitUomId = UnitOfMeasurement.Mbps.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }

        public void CalculateBaseValue()
        {
            var priceUnit = UnitOfMeasurement.Find(c => c.Id == this.PriceUnitUomId);
            this.BasedPriceValue = this.PriceValue / (decimal)priceUnit.ConversionRate;

            var usageFromUnit = UnitOfMeasurement.Find(c => c.Id == this.UsageValueFromUomId);
            this.UsageBaseUomValueFrom = usageFromUnit.ConvertToBase(this.UsageValueFrom);

            var usageToUnit = UnitOfMeasurement.Find(c => c.Id == this.UsageValueToUomId);
            this.UsageBaseUomValueTo = usageToUnit.ConvertToBase(this.UsageValueTo);
        }
    }
}
