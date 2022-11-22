using ApplicationUserIdentity.API.Models.Otp;
using GenericRepository;
using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IOtpQueries : IQueryRepository
    {
        OtpDto FindByUserName(string userName);
    }
    public class OtpQueries : QueryRepository<OtpEntity, int>, IOtpQueries
    {
        public OtpQueries(ApplicationUserDbContext context) : base(context)
        {
        }

        public OtpDto FindByUserName(string userName)
        {
            var filter = new RequestFilterModel();
            var dapperExecution = BuildByTemplate<OtpDto>(filter);
            dapperExecution.SqlBuilder.InnerJoin(
                "applicationusers t2 ON t2.MobilePhoneNo = t1.Phone AND t2.IsDeleted = FALSE AND t2.IsLocked = FALSE AND t2.IsActive = TRUE");

            dapperExecution.SqlBuilder.Where("t2.UserName = @userName", new { userName = userName });
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
