using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public class EquipmentTypeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DeviceSupplies { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
    }

    public class EquipmentTypeGridDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasurementName { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public int DeviceSupplies { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}
