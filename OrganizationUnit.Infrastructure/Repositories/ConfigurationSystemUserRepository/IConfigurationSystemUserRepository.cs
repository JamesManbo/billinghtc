using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.ConfigurationSystemUserRepository
{
    public interface IConfigurationSystemUserRepository : ICrudRepository<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationSystemParameter, int>
    {
        ConfigurationSystemParameter IsCheckAdminExportData();
    }
}
