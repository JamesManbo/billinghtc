using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IConfigurationSystemUserQueryRepository : IQueryRepository
    {
        ConfigurationSystemParameterDto Find();
    }
    public class ConfigurationSystemUserQueryRepository : QueryRepository<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationSystemParameter, int>, IConfigurationSystemUserQueryRepository
    {
        public ConfigurationSystemUserQueryRepository(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {

        }
        
        public ConfigurationSystemParameterDto Find()
        {
            var dapperExecution = BuildByTemplate<ConfigurationSystemParameterDto>();
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
