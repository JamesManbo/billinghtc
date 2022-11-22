using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository
{
    public class ConfigurationSettingUserRepository : CrudRepository<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount, int>, IConfigurationSettingUserRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public ConfigurationSettingUserRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        
    }
}
