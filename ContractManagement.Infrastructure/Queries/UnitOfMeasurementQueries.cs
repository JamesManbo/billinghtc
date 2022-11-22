using CachingLayer.Interceptor;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IUnitOfMeasurementQueries : IQueryRepository
    {
        IPagedList<UnitOfMeasurementDTO> GetList(RequestFilterModel filterModel);
        [Cache]
        IEnumerable<SelectionItem> GetSelectionList(UnitOfMeasurementFilterModel filterModel);
        UnitOfMeasurementDTO Find(int id);
        IEnumerable<SelectionItem> GetSelectionListUOM(string label);
    }
    public class UnitOfMeasurementQueries : QueryRepository<UnitOfMeasurement, int>, IUnitOfMeasurementQueries
    {
        public UnitOfMeasurementQueries(ContractDbContext contractDbContext) : base(contractDbContext)
        {
        }

        public IPagedList<UnitOfMeasurementDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<UnitOfMeasurementDTO>(filterModel);
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(UnitOfMeasurementFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("CONCAT_WS('', t1.Description, '(', t1.Label, ')') AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Select("t1.Label AS Code");
            dapperExecution.SqlBuilder.Where("t1.Type = @type",new { type = filterModel.UnitOfMeasurementType });
            return dapperExecution.ExecuteQuery();
        }

        public UnitOfMeasurementDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<UnitOfMeasurementDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionListUOM(string label)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("CONCAT_WS('', t1.Description, '(', t1.Label, ')') AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`Label` AS `Code`");
            dapperExecution.SqlBuilder.Where($"t1.`Label` LIKE '%{label}%'");
            return dapperExecution.ExecuteQuery();
        }
    }
}
