using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System.Linq;

namespace ContractManagement.Infrastructure.Repositories.EquipmentTypeRepository
{
    public class EquipmentTypeRepository : CrudRepository<EquipmentType, int>, IEquipmentTypeRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public EquipmentTypeRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public bool CheckExistEquipmentTypeName(string name, int id = 0)
        {
            var lstNameEquipment = _contractDbContext.EquipmentTypes.Where(x => x.Name == name.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstNameEquipment == 0) // không tồn tại tên thiết bị
            {
                return true;
            }
            else if (id > 0 && lstNameEquipment == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CheckExistEquipmentTypeCode(string code, int id = 0)
        {
            var lstCodeEquipment = _contractDbContext.EquipmentTypes.Where(x => x.Code == code.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstCodeEquipment == 0) // không tồn tại mã thiết bị
            {
                return true;
            }
            else if (id > 0 && lstCodeEquipment == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
