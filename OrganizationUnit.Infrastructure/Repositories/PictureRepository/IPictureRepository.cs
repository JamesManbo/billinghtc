using System;
using System.Collections.Generic;
using System.Text;
using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.PictureAggregate;

namespace OrganizationUnit.Infrastructure.Repositories.PictureRepository
{
    public interface IPictureRepository : ICrudRepository<OrganizationUnit.Domain.AggregateModels.PictureAggregate.Picture, int>
    {
    }
}
