using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;

namespace ContractManagement.Domain.Commands.EquipmentTypeCommand
{
    public class CreateEquipmentTypeCommand : IRequest<ActionResponse<EquipmentTypeDTO>>
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public int DeviceSupplies { get; set; }
    }

    public class UpdateEquipmentTypeCommand : IRequest<ActionResponse<EquipmentTypeDTO>>
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public int DeviceSupplies { get; set; }
    }
}
