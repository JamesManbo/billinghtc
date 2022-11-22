using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.EquipmentAggregate
{
    public enum UnitOfMeasurementType : int
    {
        CountingUnit = 0,
        Distance = 1,
        Timer = 2,
        Bandwidth = 3,
        Bytes = 4
    }
}
