

using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using System.Linq;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IPromotionDetailQueries : IQueryRepository
    {
        PromotionDetail GetPromotionDetails(int id);
        PromotionDetailDTO GetPromotionDetailByOCIdAndSPId(int outContractServicePackageId);
            //, int serviceId, int servicePackageId, int projectId);
            // public void UpdatePromotionProduct(int oldServicePackageId, int newServicePackageId);
    }
    public class PromotionDetailQueries : QueryRepository<PromotionDetail, int>, IPromotionDetailQueries
    {
        public PromotionDetailQueries(ContractDbContext context) : base(context)
        {
            
        }
        public PromotionDetail GetPromotionDetails(int id)
            //, int serviceId, int servicePackageId, int projectId)
        {
            var dapperExecution = BuildByTemplate<PromotionDetail>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
           // dapperExecution.SqlBuilder.Where("(t1.ProjectId = @projectId OR @projectId = 0)", new { projectId });
           // dapperExecution.SqlBuilder.Where("(t1.serviceId = @serviceId OR @serviceId = 0)", new { serviceId });
           // dapperExecution.SqlBuilder.Where("(t1.servicePackageId = @servicePackageId OR @servicePackageId = 0)", new { servicePackageId });
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            var lstPriceRaw = dapperExecution.ExecuteQuery();
            return lstPriceRaw.FirstOrDefault();
        }

        public PromotionDetailDTO GetPromotionDetailByOCIdAndSPId(int outContractServicePackageId)
        {
            var dapperExecution = BuildByTemplate<PromotionDetailDTO>();
            dapperExecution.SqlBuilder.InnerJoin("PromotionForContracts as t2 ON t2.PromotionDetailId = t1.Id AND t2.OutContractServicePackageId = @OutContractServicePackageId", new { outContractServicePackageId });
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE AND t1.IsDeleted = FALSE");
            return dapperExecution.ExecuteQuery().FirstOrDefault();
        }
    }
}
