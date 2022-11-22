using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries.ServiceQueries
{
    public interface IServiceLevelAgreementQueries : IQueryRepository
    {
        public IEnumerable<ServiceLevelAgreementDTO> GetSLAByServiceId(int serviceId);
        public IEnumerable<ServiceLevelAgreementDTO> GetSLAByOutContractServicePackageId(int outContractServicePackageId);
      //  public IEnumerable<ServiceLevelAgreementDTO> GetALLSLAByOutContractServicePackageId(int outContractServicePackageId,int serviceId);
        public ServiceLevelAgreementDTO GetSLAById(int serviceId);
        
    }
    public class ServiceLevelAgreementQueries : QueryRepository<ServiceLevelAgreement,int> , IServiceLevelAgreementQueries
    {
        public ServiceLevelAgreementQueries(ContractDbContext context) : base(context)
        {
        }

        public IEnumerable<ServiceLevelAgreementDTO> GetSLAByServiceId(int serviceId)
        {
            var dapperExecution = BuildByTemplate<ServiceLevelAgreementDTO>();
            dapperExecution.SqlBuilder.Select("t1.IsDefault");
            dapperExecution.SqlBuilder.Where("t1.IsDefault = TRUE");
            dapperExecution.SqlBuilder.Where("IFNULL(t1.ServiceId, 0) != 0 AND t1.ServiceId = @serviceId", new { serviceId });
            return dapperExecution.ExecuteQuery();
        } 
        
        public ServiceLevelAgreementDTO GetSLAById(int Id)
        {
            var dapperExecution = BuildByTemplate<ServiceLevelAgreementDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @Id ",new { Id });
            return dapperExecution.ExecuteScalarQuery();
        }
        public IEnumerable<ServiceLevelAgreementDTO> GetSLAByOutContractServicePackageId(int outContractServicePackageId)
        {
            var dapperExecution = BuildByTemplate<ServiceLevelAgreementDTO>();
            dapperExecution.SqlBuilder.Where("t1.outContractServicePackageId = @outContractServicePackageId ", new { outContractServicePackageId });
            return dapperExecution.ExecuteQuery();
        }
        //public IEnumerable<ServiceLevelAgreementDTO> GetALLSLAByOutContractServicePackageId(int outContractServicePackageId, int serviceId)
        //{
        //    var dapperExecution = BuildByTemplate<ServiceLevelAgreementDTO>();
        //    dapperExecution.SqlBuilder.Where(" ( t1.outContractServicePackageId = @outContractServicePackageId " +
        //        " OR (t1.ServiceId = @serviceId AND t1.isDefault = 1) )", new { outContractServicePackageId, serviceId });            
        //    return dapperExecution.ExecuteQuery();
        //}
    }
}
