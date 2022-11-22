
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IPromotionForContractsQueries : IQueryRepository
    {
        PromotionForContract GetCurPromotionOfProduct(int outContractServicePackageId, int promotionDetailId);
        List<PromotionForContract> GetPromotionByOutServicePackageId(int outServicePackageId);
        List<PromotionForContract> GetPromotionNotApplied(int outServicePackageId);
        PromotionForContract GetPromotionForContractById(int id);
    }
    public class PromotionForContractsQueries : QueryRepository<PromotionForContract, int>, IPromotionForContractsQueries
    {
        public PromotionForContractsQueries(ContractDbContext context) : base(context)
        {

        }
        public PromotionForContract GetCurPromotionOfProduct(int outContractServicePackageId, int promotionDetailId)
        {
            var dapperExecution = BuildByTemplate<PromotionForContract>();
            dapperExecution.SqlBuilder.Where("t1.OutContractServicePackageId = @outContractServicePackageId", new { outContractServicePackageId });
            dapperExecution.SqlBuilder.Where("t1.PromotionDetailId = @PromotionDetailId", new { promotionDetailId });            
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE AND t1.IsDeleted = FALSE");
            return dapperExecution.ExecuteQuery().FirstOrDefault();
            

        }

        public List<PromotionForContract> GetPromotionByOutServicePackageId(int outServicePackageId)
        {
            var dapperExecution = BuildByTemplate<PromotionForContract>();
            dapperExecution.SqlBuilder.Where("t1.OutContractServicePackageId = @outServicePackageId", new { outServicePackageId });
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE AND t1.IsDeleted = FALSE");
            return dapperExecution.ExecuteQuery().ToList();
        }
        public List<PromotionForContract> GetPromotionNotApplied(int outServicePackageId)
        {
            var dapperExecution = BuildByTemplate<PromotionForContract>();
            dapperExecution.SqlBuilder.Where("t1.OutContractServicePackageId = @outServicePackageId", new { outServicePackageId });
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE AND t1.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("t1.IsApplied = FALSE");
            dapperExecution.SqlBuilder.Where("t1.PromotionType IN (3,4) ");
            return dapperExecution.ExecuteQuery().ToList();
        }
        public PromotionForContract GetPromotionForContractById(int id)
        {
            var dapperExecution = BuildByTemplate<PromotionForContract>();
            dapperExecution.SqlBuilder.Where("t1.id = @id", new { id });           
            return dapperExecution.ExecuteQuery().FirstOrDefault();
        }
    }
}
