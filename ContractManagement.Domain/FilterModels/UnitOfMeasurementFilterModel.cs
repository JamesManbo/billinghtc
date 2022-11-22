using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class UnitOfMeasurementFilterModel : RequestFilterModel
    {
        public UnitOfMeasurementType UnitOfMeasurementType { get; set; }
    }
}
