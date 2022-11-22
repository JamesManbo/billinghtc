using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel.App
{
    public class CUTransactionEquipmentApp
    {
        public int? UnitOfMeasurementId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public bool HasToReClaim { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; } //Giá thiết bị
    }
}
