using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.EquipmentPictureRepository
{
    public interface IEquipmentPictureRepository : ICrudRepository<EquipmentPicture, int>
    {
    }
}
