using Dapper;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.ContractModels;
using DebtManagement.Domain.Models.ReportModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IOutContractService
    {
        void ActivateSuspensionHandled(string ids);
    }

    public class OutContractService : IOutContractService
    {
        //protected readonly ContractDbcontext
        private readonly ConnectionSettings _connectionSettings;
        private readonly ILogger<OutContractService> _logger;

        public OutContractService(IOptions<ConnectionSettings> options,
            ILogger<OutContractService> logger)
        {
            this._connectionSettings = options.Value;
            this._logger = logger;
        }

        public void ActivateSuspensionHandled(string ids)
        {
            try
            {
                WithConnection(conn =>
                    conn.Query("ActivateSuspensionTimes",
                    param: new { ids },
                    commandType: CommandType.StoredProcedure));
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "ERROR Handling delete data from table TemporaryPayingContracts");
            }
        }

        protected T WithConnection<T>(Func<IDbConnection, T> connection)
        {
            try
            {
                using var dbConnection = GetConnection(this._connectionSettings.ContractDbConnection);
                return connection(dbConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
                ex.Data["SqlGenericRepository.ConnectionString"] = this._connectionSettings.ContractDbConnection;
                throw;
            }
            catch (MySqlException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
                ex.Data["SqlGenericRepository.ConnectionString"] = this._connectionSettings.ContractDbConnection;
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
                ex.Data["SqlGenericRepository.ConnectionString"] = this._connectionSettings.ContractDbConnection;
                throw;
            }
        }

        private IDbConnection GetConnection(string connectionString)
        {
            var dbConnection = new MySqlConnection(connectionString);
            try
            {
                dbConnection.Open();
                return dbConnection;
            }
            catch (Exception e)
            {
                e.Data["SqlGenericRepository.Message-CreateDbConnection"] = "Not new SqlConnection";
                e.Data["SqlGenericRepository.ConnectionString"] = connectionString;
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
