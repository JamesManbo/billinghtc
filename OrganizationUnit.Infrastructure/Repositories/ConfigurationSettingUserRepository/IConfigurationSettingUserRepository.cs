using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository
{
    public interface IConfigurationSettingUserRepository : ICrudRepository<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount, int>
    {
    }
}
