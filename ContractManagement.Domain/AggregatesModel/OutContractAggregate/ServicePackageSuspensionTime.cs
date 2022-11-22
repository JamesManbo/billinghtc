using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("ServicePackageSuspensionTimes")]
    public class ServicePackageSuspensionTime: Entity
    {
        public int OutContractServicePackageId { get; set; }
        public DateTime SuspensionStartDate { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public int DaysSuspended { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmountBeforeTax { get; set; }
        public decimal RemainingAmountBeforeTax { get; set; }
        public string Description { get; set; }
        public virtual OutContractServicePackage Channel { get; set; }
    }
}
