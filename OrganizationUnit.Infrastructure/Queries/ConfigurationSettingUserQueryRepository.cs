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
    public interface IConfigurationSettingUserQueryRepository : IQueryRepository
    {
        ConfigurationSettingUserDto FindById(int id);
    }
    public class ConfigurationSettingUserQueryRepository : QueryRepository<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount, int>, IConfigurationSettingUserQueryRepository
    {
        public ConfigurationSettingUserQueryRepository(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {

        }

        public ConfigurationSettingUserDto FindById(int id)
        {
            var dapperExecution = BuildByTemplate<ConfigurationSettingUserDto>();
            dapperExecution.SqlBuilder.Where("t1.UserId = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
