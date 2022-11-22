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

namespace DebtManagement.BackgroundTasks.Services.OutContracts
{
    public interface IOutContractService
    {
        List<OutContractDTO> GetNeedToPaymentContracts();
        IEnumerable<ChannelSuspensionTimeDTO> GetChannelSuspensionTimes();
        bool UpdateNextBillingDateOfPayingContracts();
        void DeleteFromTemporaryPayingContracts();
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

        public IEnumerable<ChannelSuspensionTimeDTO> GetChannelSuspensionTimes()
        {
            return WithConnection(conn =>
                   conn.Query<ChannelSuspensionTimeDTO>(
                       "GetAllChannelSuspensionTimes",
                       new
                       {
                           cId = string.Empty
                       },
                       commandType: CommandType.StoredProcedure));
        }

        public List<OutContractDTO> GetNeedToPaymentContracts()
        {
            var cache = new Dictionary<int, OutContractDTO>();
            var result = WithConnection(conn =>
                conn.Query(
                    "GetNeedToPaymentContractServices",
                    new[]
                    {
                        typeof(OutContractDTO), // 0
                        typeof(PaymentMethod), // 1
                        typeof(ContractTimeLine), // 2
                        typeof(OutContractServicePackageDTO), // 3
                        typeof(BillingTimeLine), // 4
                        typeof(ContractorDTO), // 5
                        typeof(OutContractServicePackageTaxDTO), // 6
                        typeof(OutputChannelPointDTO), // 7
                        typeof(InstallationAddress), // 8
                        typeof(OutputChannelPointDTO), // 9
                        typeof(InstallationAddress), // 10
                        typeof(PromotionForContractDTO) // 11
                    },
                    results =>
                    {
                        var outContract = (OutContractDTO)results[0];
                        var paymentMethod = (PaymentMethod)results[1];
                        var contractTimeLine = (ContractTimeLine)results[2];
                        var srvChannel = (OutContractServicePackageDTO)results[3];
                        var srvPckTimeLine = (BillingTimeLine)results[4];
                        var paymentTarget = (ContractorDTO)results[5];
                        var ocspt = results[6] as OutContractServicePackageTaxDTO;

                        var endPoint = results[7] as OutputChannelPointDTO;
                        var endPointInstallAddress = results[8] as InstallationAddress;

                        var startPoint = results[9] as OutputChannelPointDTO;
                        var startPointInstallAddress = results[10] as InstallationAddress;
                        var promotionForContract = results[11] as PromotionForContractDTO;

                        if (!cache.TryGetValue(outContract.Id, out var cachedContractDto))
                        {
                            cachedContractDto = outContract;
                            cache.Add(outContract.Id, cachedContractDto);
                        }

                        cachedContractDto.Payment = paymentMethod;
                        cachedContractDto.TimeLine = contractTimeLine;

                        var existedChannel = cachedContractDto.ServicePackages
                            .FirstOrDefault(s => s.Id == srvChannel.Id);

                        if (existedChannel == null)
                        {
                            srvChannel.TimeLine = srvPckTimeLine;
                            srvChannel.PaymentTarget = paymentTarget;
                            cachedContractDto.ServicePackages.Add(srvChannel);
                        }
                        else
                        {
                            srvChannel = existedChannel;
                        }

                        if (srvChannel.StartPointChannelId.HasValue)
                        {
                            startPoint.InstallationAddress = startPointInstallAddress;
                            srvChannel.StartPoint = startPoint;
                        }

                        endPoint.InstallationAddress = endPointInstallAddress;
                        srvChannel.EndPoint = endPoint;

                        if (ocspt != null
                            && ocspt.TaxCategoryId != 0
                            && srvChannel.OutContractServicePackageTaxes
                                .All(s => s.TaxCategoryId != ocspt.TaxCategoryId))
                        {
                            srvChannel.OutContractServicePackageTaxes.Add(ocspt);
                        }

                        if (promotionForContract != null &&
                            srvChannel.PromotionForContracts.All(s => s.PromotionDetailId != promotionForContract.PromotionDetailId))
                        {
                            srvChannel.PromotionForContracts.Add(promotionForContract);
                        }

                        return cachedContractDto;
                    },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Form,Signed,Id,PaymentPeriod,Id,OutContractServicePackageId,Id,EndPointInstallationSpliter,Id,StartPointInstallationSpliter,PromotionId"));//

            return result.Distinct().ToList();
        }

        public bool UpdateNextBillingDateOfPayingContracts()
        {
            var effectedRecords = WithConnection(conn =>
                conn.Execute("UpdateNextBillingDateOfPayingContracts", commandType: CommandType.StoredProcedure));
            return effectedRecords > 0;
        }

        public void DeleteFromTemporaryPayingContracts()
        {
            try
            {
                WithConnection(conn =>
                    conn.Execute("DELETE FROM TemporaryPayingContracts", commandType: CommandType.Text));
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
