using ApplicationUserIdentity.API.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IFCMTokenQueries : IQueryRepository
    {
        FCMTokenDto FindByReceiverIdAndFcmToken(string receiverId, string fcmToken);
        List<FCMTokenDto> GetListTokenByUids(string uids);
    }
    public class FCMTokenQueries : QueryRepository<FCMToken, int>, IFCMTokenQueries
    {
        public FCMTokenQueries(ApplicationUserDbContext context) : base(context)
        {

        }
        public FCMTokenDto FindByReceiverIdAndFcmToken(string receiverId, string fcmToken)
        {
            var dapperExecution = BuildByTemplate<FCMTokenDto>();
            dapperExecution.SqlBuilder.Where("t1.ReceiverId = @id", new { id = receiverId });
            dapperExecution.SqlBuilder.Where("t1.Token = @token", new { token = fcmToken });
            return dapperExecution.ExecuteScalarQuery();
        }

        public List<FCMTokenDto> GetListTokenByUids(string uids)
        {
            var dapperExecution = BuildByTemplate<FCMTokenDto>();
            dapperExecution.SqlBuilder.Select("t2.`FullName` AS `Receiver`");
            dapperExecution.SqlBuilder.InnerJoin(
                "ApplicationUsers t2 ON t2.IdentityGuid = t1.ReceiverId AND t2.IsDeleted = FALSE AND FIND_IN_SET(t2.IdentityGuid, @uids) > 0", new { uids });
     

            return dapperExecution.ExecuteQuery().ToList();
        }
    }
}
