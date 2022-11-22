using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Utility;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IContractEquipmentQueries : IQueryRepository
    {
        IEnumerable<ContractEquipmentDTO> GetAllByOutContractId(string ids);
        IEnumerable<ContractEquipmentDTO> GetAllHasToReclaimByOutContractId(string ids);
    }

    public class ContractEquipmentQueries : QueryRepository<ContractEquipment, int>, IContractEquipmentQueries
    {
        public ContractEquipmentQueries(ContractDbContext context) : base(context)
        {
        }

        public IEnumerable<ContractEquipmentDTO> GetAllByOutContractId(string ids)
        {
            var cached = new Dictionary<int, ContractEquipmentDTO>();
            var dapperExecution = BuildByTemplate<ContractEquipmentDTO>();
            dapperExecution.SqlBuilder.Select("t2.`OutContractId` AS `OutContractId`");
            dapperExecution.SqlBuilder.Select("t2.`ServiceName` AS `ServiceName`");
            dapperExecution.SqlBuilder.Select("t2.`ChannelName` AS `ChannelName`");
            dapperExecution.SqlBuilder.Select("t2.`CId` AS `CId`");
            dapperExecution.SqlBuilder.Select("t2.`PackageName` AS `PackageName`");
            int statusId = OutContractServicePackageStatus.Terminate.Id;


            var idArr = ids.SplitToInt(',');
            dapperExecution.SqlBuilder.InnerJoin(
                "OutContractServicePackages AS t2 ON t2.Id = t1.OutContractPackageId AND t2.IsDeleted = FALSE AND t2.StatusId <> @statusId AND t2.OutContractId IN @idArr", new { statusId, idArr });

            var equipmentStatus = new int [] { EquipmentStatus.Examined.Id, EquipmentStatus.Deployed.Id };
            dapperExecution.SqlBuilder.Where("t1.StatusId IN @equipmentStatus", new { equipmentStatus });


            return dapperExecution
                .ExecuteQuery();
        }

        public IEnumerable<ContractEquipmentDTO> GetAllHasToReclaimByOutContractId(string ids)
        {
            var cached = new Dictionary<int, ContractEquipmentDTO>();
            var dapperExecution = BuildByTemplate<ContractEquipmentDTO>();
            dapperExecution.SqlBuilder.Select("t2.`OutContractId` AS `OutContractId`");
            dapperExecution.SqlBuilder.Select("t2.`ServiceName` AS `ServiceName`");
            dapperExecution.SqlBuilder.Select("t2.`ChannelName` AS `ChannelName`");
            dapperExecution.SqlBuilder.Select("t2.`CId` AS `CId`");
            dapperExecution.SqlBuilder.Select("t2.`PackageName` AS `PackageName`");

            int statusId = OutContractServicePackageStatus.Terminate.Id;

            var idArr = ids.SplitToInt(',');
            dapperExecution.SqlBuilder.InnerJoin(
                "OutContractServicePackages AS t2 ON t2.Id = t1.OutContractPackageId AND t2.IsDeleted = FALSE AND t2.StatusId <> @statusId AND t2.OutContractId IN @idArr", new { statusId, idArr });

            var equipmentStatus = new int[] { EquipmentStatus.Reclaimed.Id, EquipmentStatus.Cancelled.Id, EquipmentStatus.Terminated.Id, };
            dapperExecution.SqlBuilder.Where("t1.HasToReclaim = TRUE AND t1.StatusId NOT IN @equipmentStatus", new { equipmentStatus });


            return dapperExecution
                .ExecuteQuery();
        }
    }
}
