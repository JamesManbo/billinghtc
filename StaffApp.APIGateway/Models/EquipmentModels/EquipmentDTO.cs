using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.EquipmentModels
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public MoneyDTO Price { get; set; }
    }
}
