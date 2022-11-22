using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories
{
    public interface ITransactionRepository : ICrudRepository<Transaction, int>
    {
        IEnumerable<Transaction> GetRootOnlyByIds(int[] ids);
        IEnumerable<Transaction> GetByIds(int[] ids);
        bool RestoreOrSuspendServices(int[] transactionIds, int transactionType, string acceptanceStaff);
        bool TerminateServices(int[] transactionIds, int transactionType, string acceptanceStaff);
        bool ChangeLocationServices(int[] transactionIds, int transactionType, string acceptanceStaff);
        bool TerminateContracts(int[] transactionIds, int transactionType, string acceptanceStaff, DateTime effectiveDate, bool isOutContract = true);
        bool ChangeEquipments(int[] transactionIds, int transactionType, string acceptanceStaff);
        bool UpgradeEquipments(int[] transactionIds, int transactionType, string acceptanceStaff);
        bool ReclaimEquipments(int[] transactionIds, int transactionType, string acceptanceStaff);
        bool IsEffectiveDateRestoreService(int outContractId, DateTime effectiveDate);
        bool IsEffectiveDateSuspendService(int outContractId, DateTime effectiveDate);
    }
    public class TransactionRepository : CrudRepository<Transaction, int>, ITransactionRepository
    {
        private readonly ContractDbContext _context;
        public TransactionRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public IEnumerable<Transaction> GetRootOnlyByIds(int[] ids)
        {
            return DbSet
                .Where(c => ids.Contains(c.Id));
        }

        public IEnumerable<Transaction> GetByIds(int[] ids)
        {
            return DbSet
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(s => s.StartPoint)
                .ThenInclude(s => s.Equipments)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(s => s.EndPoint)
                .ThenInclude(s => s.Equipments)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(e => e.TaxValues)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(e => e.ServiceLevelAgreements)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(e => e.PriceBusTables)
                .Include(c => c.AttachmentFiles)
                .Include(e => e.TransactionServicePackages)
                .ThenInclude(e => e.PaymentTarget)
                .Where(c => ids.Contains(c.Id) && !c.IsDeleted);
        }

        public bool IsEffectiveDateRestoreService(int outContractId, DateTime effectiveDate)
        {
            var maxDateRestoreService = DbSet.OrderByDescending(c => c.EffectiveDate)
                .FirstOrDefault(c => c.OutContractId == outContractId && !c.IsDeleted && c.Type == TransactionType.SuspendServicePackage.Id && c.StatusId == TransactionStatus.Acceptanced.Id);
            return maxDateRestoreService == null || !maxDateRestoreService.EffectiveDate.HasValue || effectiveDate.GreaterThanOrEqualDate(maxDateRestoreService.EffectiveDate.Value);
        }

        public bool IsEffectiveDateSuspendService(int outContractId, DateTime effectiveDate)
        {
            var maxDateSuspendService = DbSet.OrderByDescending(c => c.EffectiveDate)
                .FirstOrDefault(c => c.OutContractId == outContractId && !c.IsDeleted && c.Type == TransactionType.RestoreServicePackage.Id && c.StatusId == TransactionStatus.Acceptanced.Id);
            return maxDateSuspendService == null || !maxDateSuspendService.EffectiveDate.HasValue || effectiveDate.GreaterThanOrEqualDate(maxDateSuspendService.EffectiveDate.Value);
        }

        public bool RestoreOrSuspendServices(int[] transactionIds, int transactionType, string acceptanceStaff)
        {
            MySqlConnection connection = (MySqlConnection)_context.Database.GetDbConnection();
            MySqlCommand cmd = connection.CreateCommand();

            cmd.Connection = connection;
            if (_context.HasActiveTransaction)
            {
                cmd.Transaction = (MySqlTransaction)_context.GetCurrentTransaction().GetDbTransaction();
            }
            cmd.CommandText = "RestoreOrSuspendServices";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });

            var resultOutputParam = new MySqlParameter("@isSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            return isSuccess;
        }

        public bool TerminateServices(int[] transactionIds, int transactionType, string acceptanceStaff)
        {
            MySqlConnection connection = (MySqlConnection)_context.Database.GetDbConnection();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            if (_context.HasActiveTransaction)
            {
                cmd.Transaction = (MySqlTransaction)_context.GetCurrentTransaction().GetDbTransaction();
            }
            cmd.CommandText = "TerminateServices";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });

            var resultOutputParam = new MySqlParameter("@isSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            return isSuccess;
        }

        public bool ChangeLocationServices(int[] transactionIds, int transactionType, string acceptanceStaff)
        {
            MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "ChangeLocationServices";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });
            var resultOutputParam = new MySqlParameter("@IsSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            connection.Close();
            return isSuccess;
        }

        public bool TerminateContracts(int[] transactionIds, int transactionType, string acceptanceStaff, DateTime effectiveDate, bool isOutContract = true)
        {
            MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "TerminateContracts";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });
            cmd.Parameters.Add(new MySqlParameter("@effectiveDate", MySqlDbType.DateTime) { Value = effectiveDate });
            cmd.Parameters.Add(new MySqlParameter("@isOutContract", MySqlDbType.Bool) { Value = isOutContract });
            var resultOutputParam = new MySqlParameter("@isSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            connection.Close();
            return isSuccess;
        }

        public bool ChangeEquipments(int[] transactionIds, int transactionType, string acceptanceStaff)
        {
            MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "ChangeEquipments";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });
            var resultOutputParam = new MySqlParameter("@IsSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            connection.Close();
            return isSuccess;
        }

        public bool UpgradeEquipments(int[] transactionIds, int transactionType, string acceptanceStaff)
        {
            MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "UpgradeEquipments";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });
            var resultOutputParam = new MySqlParameter("@IsSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            connection.Close();
            return isSuccess;
        }

        public bool ReclaimEquipments(int[] transactionIds, int transactionType, string acceptanceStaff)
        {
            MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "ReclaimEquipments";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("@transactionIds", MySqlDbType.VarChar) { Value = string.Join(",", transactionIds) });
            cmd.Parameters.Add(new MySqlParameter("@transactionType", MySqlDbType.Int32) { Value = transactionType });
            cmd.Parameters.Add(new MySqlParameter("@acceptanceStaff", MySqlDbType.VarChar) { Value = acceptanceStaff });
            var resultOutputParam = new MySqlParameter("@IsSuccess", MySqlDbType.Bool) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(resultOutputParam);

            cmd.ExecuteNonQuery();

            bool isSuccess = !(resultOutputParam.Value is DBNull) && (bool)resultOutputParam.Value;
            connection.Close();
            return isSuccess;
        }


        public override Task<Transaction> GetByIdAsync(object id)
        {
            return DbSet
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(s => s.StartPoint)
                .ThenInclude(s => s.Equipments)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(s => s.EndPoint)
                .ThenInclude(s => s.Equipments)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(e => e.TaxValues)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(e => e.ServiceLevelAgreements)
                .Include(e => e.TransactionServicePackages)
                .ThenInclude(e => e.AppliedPromotions)
                .Include(e => e.TransactionServicePackages)
                .ThenInclude(e => e.PaymentTarget)
                .Include(c => c.TransactionServicePackages)
                .ThenInclude(e => e.PriceBusTables)
                .Include(c => c.AttachmentFiles)
                .Where(c => (int)id == c.Id && !c.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
