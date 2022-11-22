using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.UnitOfMeasurementRepository
{
    public interface IUnitOfMeasurementRepository : ICrudRepository<UnitOfMeasurement, int>
    {
        bool CheckExitDescription(string description, int id);
        bool CheckExitLabel(string label, int id);
    }
}
