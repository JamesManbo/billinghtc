using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using OrganizationUnit.Domain.Models.FCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IFCMTokenQueries : IQueryRepository
    {
        FCMTokenDto FindByReceiverId(string id);
        FCMTokenDto FindByReceiverIdAndFcmToken(string receiverId, string fcmToken);
    }
    public class FCMTokenQueries: QueryRepository<FCMToken, int>, IFCMTokenQueries
    {
        public FCMTokenQueries(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public FCMTokenDto FindByReceiverId(string id)
        {
            var dapperExecution = BuildByTemplate<FCMTokenDto>();

            dapperExecution.SqlBuilder.Where("t1.ReceiverId = @id", new { id });

            return dapperExecution.ExecuteScalarQuery();
        }

        public FCMTokenDto FindByReceiverIdAndFcmToken(string receiverId, string fcmToken)
        {
            var dapperExecution = BuildByTemplate<FCMTokenDto>();
            dapperExecution.SqlBuilder.Where("t1.ReceiverId = @id", new { id = receiverId });
            dapperExecution.SqlBuilder.Where("t1.Token = @token", new { token = fcmToken });
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
