using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using Dapper;
using GenericRepository;
using GenericRepository.DapperSqlBuilder;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IOutputChannelPointQueries : IQueryRepository
    {
        OutputChannelPointDTO Find(int id);
        IEnumerable<SelectionItem> GetSelectionList();
        IEnumerable<OutputChannelPointDTO> Autocomplete(OutputChannelFilterModel filterModel);
        IEnumerable<OutputChannelPointDTO> GetAll();
        IPagedList<OutputChannelPointDTO> GetList(OutputChannelFilterModel filterModel);
        int GetLatestId();
    }

    public class OutputChannelFilterModel : RequestFilterModel
    {
        public OutputChannelPointTypeEnum? PointType { get; set; }
    }

    public class OutputChannelPointSqlBuilder : SqlBuilder
    {
        public OutputChannelPointSqlBuilder(string tableName) : base(tableName)
        {
        }

        public OutputChannelPointSqlBuilder()
        {
        }

        public void SelectInstallationAddress()
        {
            Select("'' AS `InstallationAddressSpliter`");
            Select("t1.InstallationAddress_Street AS `Street`");
            Select("t1.InstallationAddress_CityId AS `CityId`");
            Select("t1.InstallationAddress_City AS `City`");
            Select("t1.InstallationAddress_DistrictId AS `DistrictId`");
            Select("t1.InstallationAddress_District AS `District`");
            Select("t1.InstallationAddress_Building AS `Building`");
            Select("t1.InstallationAddress_Floor AS `Floor`");
            Select("t1.InstallationAddress_RoomNumber AS `RoomNumber`");
        }

        public void FullTextSearch(string keywords)
        {
            Where("(t1.InstallationAddress_Street LIKE @keywords" +
                    " OR t1.InstallationAddress_City LIKE @keywords" +
                    " OR t1.InstallationAddress_District LIKE @keywords" +
                    " OR t1.InstallationAddress_Building LIKE @keywords)",
                    new
                    {
                        Keywords = $"%{keywords}%"
                    });
        }

        public void SelectEquipment(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.CurrencyUnitId");
            Select($"{alias}.CurrencyUnitCode");
            Select($"{alias}.OutputChannelPointId");
            Select($"{alias}.OutContractPackageId");
            Select($"{alias}.EquipmentName");
            Select($"{alias}.EquipmentPictureUrl");
            Select($"{alias}.EquipmentUom");
            Select($"{alias}.UnitPrice");
            Select($"{alias}.ExaminedUnit");
            Select($"{alias}.RealUnit");
            Select($"{alias}.ReclaimedUnit");
            Select($"{alias}.IsInSurveyPlan");
            Select($"{alias}.IsFree");
            Select($"{alias}.HasToReclaim");
            Select($"{alias}.SerialCode");
            Select($"{alias}.DeviceCode");
            Select($"{alias}.Manufacturer");
            Select($"{alias}.Specifications");
            Select($"{alias}.StatusId");
            Select($"{alias}.EquipmentId");
            Select($"{alias}.SubTotal");
            Select($"{alias}.GrandTotal");
            Select($"{alias}.ExaminedSubTotal");
            Select($"{alias}.ExaminedGrandTotal");
        }
    }

    public class OutputChannelPointQueries : QueryRepository<OutputChannelPoint, int>, IOutputChannelPointQueries
    {
        public OutputChannelPointQueries(ContractDbContext context) : base(context)
        {
        }

        public int GetLatestId()
        {
            return WithConnection(conn => conn.ExecuteScalar<int>(
                   $"SELECT Id FROM {this.TableName} ORDER BY Id DESC LIMIT 1"));
        }

        public IEnumerable<OutputChannelPointDTO> Autocomplete(OutputChannelFilterModel filterModel)
        {
            var cache = new Dictionary<int, OutputChannelPointDTO>();
            var dapperExecution = BuildByTemplate<OutputChannelPointDTO,
                OutputChannelPointSqlBuilder>();

            dapperExecution.SqlBuilder.SelectInstallationAddress();

            //dapperExecution.SqlBuilder.SelectEquipment("ce");

            //dapperExecution.SqlBuilder.LeftJoin("ContractEquipments ce ON ce.OutputChannelPointId = t1.Id");

            dapperExecution.SqlBuilder.GroupBy("t1.LocationId");

            if (!string.IsNullOrEmpty(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder.FullTextSearch(filterModel.Keywords);
            }

            if (filterModel.PointType.HasValue && filterModel.PointType > 0)
            {
                dapperExecution.SqlBuilder
                    .Where("t1.PointType = @pointType", new { PointType = filterModel.PointType });
            }

            dapperExecution.SqlBuilder.Skip(0).Take(10);

            return dapperExecution.ExecuteQuery<OutputChannelPointDTO, InstallationAddress>(
               (outputPoint, installationAddress) =>
               {
                   if (!cache.TryGetValue(outputPoint.Id, out var outputPointEntry))
                   {
                       outputPointEntry = outputPoint;
                   }
                   else
                   {
                       cache.Add(outputPoint.Id, outputPoint);
                   }

                   outputPointEntry.InstallationAddress = installationAddress;
                   //if(equipment != null && outputPointEntry.Equipments.All(e => e.Id != equipment.Id))
                   //{
                   //    outputPointEntry.Equipments.Add(equipment);
                   //}

                   return outputPointEntry;
               },
                "InstallationAddressSpliter")
                .Distinct();
        }

        public OutputChannelPointDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<OutputChannelPointDTO,
                OutputChannelPointSqlBuilder>();

            dapperExecution.SqlBuilder.SelectInstallationAddress();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<OutputChannelPointDTO> GetAll()
        {
            var dapperExecution = BuildByTemplate<OutputChannelPointDTO, OutputChannelPointSqlBuilder>();

            dapperExecution.SqlBuilder.SelectInstallationAddress();

            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<OutputChannelPointDTO> GetList(OutputChannelFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<OutputChannelPointDTO, OutputChannelPointSqlBuilder>(filterModel);

            dapperExecution.SqlBuilder.SelectInstallationAddress();

            if (!string.IsNullOrEmpty(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder.FullTextSearch(filterModel.Keywords);
            }

            if (filterModel.PointType.HasValue && filterModel.PointType > 0)
            {
                dapperExecution.SqlBuilder
                    .Where("t1.PointType = @pointType", new { PointType = filterModel.PointType });
            }

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();

            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.InstallationAddress_Street AS `Text`");

            return dapperExecution.ExecuteQuery();
        }
    }
}
