using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.Models;
using Dapper;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContractManagement.Utility;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using AutoMapper;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IEquipmentTypeQueries : IQueryRepository
    {
        IEnumerable<ContractEquipmentDTO> AutocompleteInstance(RequestFilterModel filterModel);
        IEnumerable<EquipmentTypeDTO> Autocomplete(RequestFilterModel requestFilterModel);
        IPagedList<EquipmentTypeDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        IEnumerable<EquipmentTypeDTO> GetAll(RequestFilterModel filterModel = null);
        EquipmentTypeDTO Find(int id);
    }

    public class EquipmentTypeQueries : QueryRepository<EquipmentType, int>, IEquipmentTypeQueries
    {
        private readonly IMapper _mapper;
        public EquipmentTypeQueries(ContractDbContext contractDbContext,
            IMapper mapper) : base(contractDbContext)
        {
            this._mapper = mapper;
        }
        public IEnumerable<EquipmentTypeDTO> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<EquipmentTypeDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t2.Description as UnitOfMeasurement");
            dapperExecution.SqlBuilder.LeftJoin("UnitOfMeasurement t2 ON t1.UnitOfMeasurementId = t2.Id");
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.Code LIKE @name", new { name = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.Name LIKE @name", new { name = $"%{requestFilterModel.Keywords}%" });
            }
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<EquipmentTypeDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<EquipmentTypeDTO>(filterModel);
            dapperExecution.SqlBuilder.LeftJoin("UnitOfMeasurement t2 ON t1.UnitOfMeasurementId = t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Description as UnitOfMeasurement");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.Code LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.Name LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.Manufacturer LIKE @name", new { name = $"%{filterModel.Keywords}%" });
            }
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.Name AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<EquipmentTypeDTO> GetAll(RequestFilterModel filterModel)
        {
            if (filterModel != null)
            {
                filterModel.Paging = false;
            }
            var dapperExecution = BuildByTemplate<EquipmentTypeDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("CONCAT(t2.Label, '(', t2.Description, ')') as UnitOfMeasurementName");

            dapperExecution.SqlBuilder.LeftJoin("UnitOfMeasurement t2 ON t1.UnitOfMeasurementId = t2.Id");
            if (filterModel != null)
            {
                if (filterModel.PropertyFilterModels
                .Any(p => p.Field.Equals("Ids", StringComparison.OrdinalIgnoreCase)))
                {
                    var ids =
                        filterModel.PropertyFilterModels.First(
                                p => p.Field.Equals("Ids", StringComparison.OrdinalIgnoreCase))
                            .FilterValue.ToString().SplitToInt(',');
                    dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
                }
            }

            return dapperExecution.ExecuteQuery();
        }

        public EquipmentTypeDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<EquipmentTypeDTO>();
            dapperExecution.SqlBuilder.Select("t2.`Description` AS `UnitOfMeasurement`");
            dapperExecution.SqlBuilder.LeftJoin("UnitOfMeasurement t2 ON t1.UnitOfMeasurementId = t2.Id");

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<ContractEquipmentDTO> AutocompleteInstance(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<EquipmentTypeDTO>(filterModel);

            dapperExecution.SqlBuilder.Select("uom.`Description` AS `UnitOfMeasurement`");

            dapperExecution.SqlBuilder.InnerJoin("UnitOfMeasurement AS uom ON uom.Id = t1.UnitOfMeasurementId");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.Code LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.Name LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.Manufacturer LIKE @name", new { name = $"%{filterModel.Keywords}%" });
            }

            var equipmentTypes = dapperExecution.ExecuteQuery();
            if (equipmentTypes != null && equipmentTypes.Any())
            {
                return equipmentTypes.Select(c =>
                {
                    var contractEquipment = new ContractEquipment()
                    {
                        EquipmentId = c.Id,
                        EquipmentName = c.Name,
                        DeviceCode = c.Code,
                        HasToReclaim = false,
                        IsFree = false,
                        EquipmentUom = c.UnitOfMeasurement,
                        Manufacturer = c.Manufacturer,
                        Specifications = c.Specifications,
                        UnitPrice = c.Price,
                    };
                    contractEquipment.SetExaminedUnits(1);
                    contractEquipment.SetStatusId(EquipmentStatus.Examined.Id);
                    contractEquipment.CalculateTotal();

                    return this._mapper.Map<ContractEquipmentDTO>(contractEquipment);
                });
            }

            return default;
        }
    }
}
