using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.ConfigurationSystemUserRepository
{
    public class ConfigurationSystemUserRepository : CrudRepository<ConfigurationSystemParameter, int>, IConfigurationSystemUserRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public ConfigurationSystemUserRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper)
            : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public ConfigurationSystemParameter IsCheckAdminExportData()
        {
            var checkAdminExportData = _organizationUnitDbContext
                .ConfigurationSystemParameters
                .FirstOrDefault();
            return checkAdminExportData;
        }
    }
}
