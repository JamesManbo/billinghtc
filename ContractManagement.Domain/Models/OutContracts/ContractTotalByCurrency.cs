using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractTotalByCurrencyDTO
    {
        public int Id { get; set; }
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
    }
}
