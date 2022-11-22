using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    [Table("ContractTotalByCurrencies")]
    public class ContractTotalByCurrency : Entity
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal ServicePackageAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal EquipmentAmount { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }

        public ContractTotalByCurrency()
        {
            PromotionTotalAmount = 0;
            ServicePackageAmount = 0;
            SubTotal = 0;
            GrandTotalBeforeTax = 0;
            GrandTotal = 0;
            OtherFee = 0;
            EquipmentAmount = 0;
            TotalTaxAmount = 0;
        }

        public void Calculate(IEnumerable<OutContractServicePackage> 
            channelsGroupedByCurrency)
        {
            this.TotalTaxAmount = channelsGroupedByCurrency.Sum(t => t.TaxAmount);
            this.PromotionTotalAmount = channelsGroupedByCurrency.Sum(e => e.PromotionAmount);
            this.ServicePackageAmount = channelsGroupedByCurrency.Sum(e => e.SubTotal);
            this.EquipmentAmount = channelsGroupedByCurrency.Sum(e => e.EquipmentAmount);
            this.InstallationFee = channelsGroupedByCurrency.Sum(e => e.InstallationFee);
            this.OtherFee = channelsGroupedByCurrency.Sum(e => e.OtherFee);

            this.SubTotalBeforeTax = channelsGroupedByCurrency.Sum(e => e.SubTotalBeforeTax);
            this.SubTotal = channelsGroupedByCurrency.Sum(e => e.SubTotal);

            this.GrandTotalBeforeTax = channelsGroupedByCurrency.Sum(e => e.GrandTotalBeforeTax);
            this.GrandTotal = channelsGroupedByCurrency.Sum(e => e.GrandTotal);
        }
    }
}
