using GenericRepository;
using Global.Models.Filter;
using OrganizationUnit.Domain.AggregateModels.OTPAggregate;
using OrganizationUnit.Domain.Models.Otp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IOtpQueries : IQueryRepository
    {
        OtpDto FindByUserName(string userName);
    }
    public class OtpQueries : QueryRepository<OtpEntity, int>, IOtpQueries
    {
        public OtpQueries(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public OtpDto FindByUserName(string userName)
        {
            var filter = new RequestFilterModel();

            var dapperExecution = BuildByTemplate<OtpDto>(filter);

            dapperExecution.SqlBuilder.InnerJoin(
                "Users t2 ON t2.MobilePhoneNo = t1.Phone AND t2.IsDeleted = FALSE AND t2.IsLock = FALSE");

            dapperExecution.SqlBuilder.Where("t2.UserName = @userName", new { userName = userName });
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
