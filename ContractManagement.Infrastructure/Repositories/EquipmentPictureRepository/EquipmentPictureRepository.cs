using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.EquipmentPictureRepository
{
    public class EquipmentPictureRepository : CrudRepository<EquipmentPicture, int>, IEquipmentPictureRepository
    {
        public EquipmentPictureRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
            context, configAndMapper)
        {
        }
    }
}
