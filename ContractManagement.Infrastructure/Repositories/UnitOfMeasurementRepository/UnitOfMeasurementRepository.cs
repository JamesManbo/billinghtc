using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.UnitOfMeasurementRepository
{
    public class UnitOfMeasurementRepository : CrudRepository<UnitOfMeasurement, int>, IUnitOfMeasurementRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public UnitOfMeasurementRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public bool CheckExitDescription(string description, int id)
        {
            if (string.IsNullOrEmpty(description))
                return false;

            return _contractDbContext
                .UnitOfMeasurements
                .Any(x => description.Trim().Equals(x.Description)
                    && (id == 0 || x.Id != id));
        }

        public bool CheckExitLabel(string label, int id)
        {
            if (string.IsNullOrEmpty(label)) return false;

            return _contractDbContext
                .UnitOfMeasurements
                .Any(x => label.Trim().Equals(x.Label)
                    && (id == 0 || x.Id != id));

        }

        public override ActionResponse DeleteAndSave(params object[] keys)
        {
            var actionResponse = new ActionResponse();
            var entity = DbSet.Find(keys);

            if (entity == null)
            {
                actionResponse.AddError(
                    $"Sorry, I could not find the records you wanted to delete.");
                return actionResponse;
            }

            if (entity.IsRequired)
            {
                actionResponse.AddError(
                    $"Xin lỗi, bản ghi này không được phép xóa.");
                return actionResponse;
            }

            entity.IsDeleted = true;
            Update(entity);

            Context.SaveChanges();

            return actionResponse;
        }

        public override ActionResponse DeleteWithActionAndSave(Func<DbContext, UnitOfMeasurement, IActionResponse> runBeforeDelete, params object[] keys)
        {
            var actionResponse = new ActionResponse();
            var entity = DbSet.Find(keys);
            if (entity == null)
            {
                actionResponse.AddError(
                    $"Sorry, I could not find the records you wanted to delete.");
                return actionResponse;
            }

            if (entity.IsRequired)
            {
                actionResponse.AddError(
                    $"Xin lỗi, bản ghi này không được phép xóa.");
                return actionResponse;
            }

            actionResponse.CombineResponse(runBeforeDelete(Context, entity));
            if (!actionResponse.IsSuccess) return actionResponse;

            entity.IsDeleted = true;
            Update(entity);

            Context.SaveChanges();

            return actionResponse;
        }

        public decimal ConvertToBase(decimal value, string fromUnitCode, UnitOfMeasurementType type)
        {
            var unitsOfType = DbSet.Where(c => c.Type == type && !c.IsDeleted);
            if (unitsOfType == null || unitsOfType.Count() == 0) return 0;

            var sourceUnit = unitsOfType.FirstOrDefault(m => m.Label.Equals(fromUnitCode, StringComparison.OrdinalIgnoreCase));

            return sourceUnit.ConversionRate * value;
        }

        public decimal ConvertTo(decimal value, string sourceUnitCode, string desUnitCode, UnitOfMeasurementType type)
        {
            var unitsOfType = DbSet.Where(c => c.Type == type && !c.IsDeleted);
            var sourceUnit = unitsOfType.FirstOrDefault(c => sourceUnitCode.Equals(c.Label, StringComparison.OrdinalIgnoreCase));
            var destinationUnit = unitsOfType.FirstOrDefault(c => desUnitCode.Equals(c.Label, StringComparison.OrdinalIgnoreCase));
            if (destinationUnit == null || sourceUnit == null) return 0;

            return value * sourceUnit.ConversionRate / destinationUnit.ConversionRate;
        }
    }
}
