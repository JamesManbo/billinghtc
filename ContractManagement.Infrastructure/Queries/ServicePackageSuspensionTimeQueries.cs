using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models.OutContracts;
using Dapper;
using GenericRepository;
using System.Collections.Generic;
using System.Data;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IServicePackageSuspensionTimeQueries : IQueryRepository
    {
        IEnumerable<ServicePackageSuspensionTimeDTO> GetListRestoredByOCSPIds(int[] oCSPIds);
        IEnumerable<ServicePackageSuspensionTimeDTO> GetChannelSuspensionTimes(string cId);
    }

    public class ServicePackageSuspensionTimeQueries : QueryRepository<ServicePackageSuspensionTime, int>, IServicePackageSuspensionTimeQueries
    {
        public ServicePackageSuspensionTimeQueries(ContractDbContext context) : base(context)
        {
        }

        public IEnumerable<ServicePackageSuspensionTimeDTO> GetListRestoredByOCSPIds(int[] oCSPIds)
        {
            var dapperExecution = BuildByTemplate<ServicePackageSuspensionTimeDTO>();
            dapperExecution.SqlBuilder.Where("t1.IsActive = FALSE AND t1.SuspensionEndDate IS NOT NULL AND t1.RemainingAmount > 0 AND t1.OutContractServicePackageId IN @oCSPIds"
                , new { oCSPIds });
            return dapperExecution.ExecuteQuery();
        }


        public IEnumerable<ServicePackageSuspensionTimeDTO> GetChannelSuspensionTimes(string cId)
        {
            return WithConnection(conn =>
                   conn.Query<ServicePackageSuspensionTimeDTO>(
                       "GetAllChannelSuspensionTimes",
                       param: new
                       {
                           cId
                       },
                       commandType: CommandType.StoredProcedure));
        }
    }
}
