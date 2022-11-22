using System;
using System.Collections.Generic;
using System.Text;
using GenericRepository;
using ContractManagement.Domain.AggregateModels.PictureAggregate;

namespace ContractManagement.Infrastructure.Repositories.PictureRepository
{
    public interface IPictureRepository : ICrudRepository<Picture, int>
    {
    }
}
