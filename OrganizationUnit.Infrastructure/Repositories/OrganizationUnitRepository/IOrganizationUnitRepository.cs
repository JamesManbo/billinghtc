using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.OrganizationUnitRepository
{
    public interface IOrganizationUnitRepository : ICrudRepository<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit, int>
    {
        bool CheckExistByName(string name, int id = 0);
        bool CheckExistByCode(string code, int id = 0);
        bool CheckExistByNumberPhone(string numberPhone, int id = 0);
        bool CheckExistByEmail(string email, int id = 0);
        IEnumerable<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit> GetOrganizationUnitIds(int? parentId);
    }
}
