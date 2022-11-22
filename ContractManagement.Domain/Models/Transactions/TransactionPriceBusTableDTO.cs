using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.OutContracts
{
    public class TransactionPriceBusTableDTO : BaseDTO
    {
        public string CurrencyUnitCode { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public decimal? UsageBaseUomValueFrom { get; set; }
        public decimal? UsageValueFrom { get; set; }
        public int? UsageValueFromUomId { get; set; }
        public decimal? UsageBaseUomValueTo { get; set; }
        public decimal? UsageValueTo { get; set; }
        public int? UsageValueToUomId { get; set; }
        public decimal PriceValue { get; set; }
        public int PriceUnitUomId { get; set; }
        public bool? IsDomestic { get; set; }
        public TransactionPriceBusTableDTO()
        {
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }
    }
}
