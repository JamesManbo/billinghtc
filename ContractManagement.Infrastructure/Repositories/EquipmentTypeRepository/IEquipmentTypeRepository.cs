using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using GenericRepository;

namespace ContractManagement.Infrastructure.Repositories.EquipmentTypeRepository
{
    public interface IEquipmentTypeRepository : ICrudRepository<EquipmentType, int>
    {
        bool CheckExistEquipmentTypeName(string name, int id = 0);
        bool CheckExistEquipmentTypeCode(string code, int id = 0);
    }
}
