using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.EquipmentAggregate
{
    [Table("EquipmentTypes")]
    public class EquipmentType : Entity
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int? UnitOfMeasurementId { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        //public bool HasToReClaim { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; } //Giá thiết bị
        public int DeviceSupplies { get; set; }
    }
}