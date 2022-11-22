using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationUnit.Infrastructure.Repositories.OrganizationUnitRepository
{
    public class OrganizationUnitRepository : CrudRepository<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit, int>, IOrganizationUnitRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public OrganizationUnitRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public bool CheckExistByName(string name, int id)
        {
            return _organizationUnitDbContext.OrganizationUnits.Any(x => x.Name == name.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExistByCode(string code, int id)
        {
            return _organizationUnitDbContext.OrganizationUnits.Any(x => x.Code == code.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExistByNumberPhone(string numberPhone, int id)
        {
            return _organizationUnitDbContext.OrganizationUnits.Any(x => x.NumberPhone == numberPhone.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExistByEmail(string email, int id)
        {
            return _organizationUnitDbContext.OrganizationUnits.Any(x => x.Email == email.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public IEnumerable<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit> GetOrganizationUnitIds(int? parentId)
        {
            var lstChildOrganizations = _organizationUnitDbContext.OrganizationUnits.Where(x => x.ParentId == parentId && x.IsDeleted == false && x.IsActive).ToList();

            return lstChildOrganizations;
        }

        public override Task<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit> GetByIdAsync(object id)
        {
            return DbSet
                .Include(o => o.OrganizationUnitUsers)
                .FirstOrDefaultAsync(o => o.Id == (int)id);
        }
    }
}
