using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Models.DebtModels;
using GenericRepository;
using GenericRepository.DapperSqlBuilder;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Linq;
using static Global.Models.Filter.RequestFilterModel;

namespace DebtManagement.Infrastructure.Queries
{
    public class InDebtManagementFilterModel : RequestFilterModel
    {
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public string PartnerId { get; set; }
        public string PartnerTaxCode { get; set; }
    }

    public interface IInDebtManagementQueries : IQueryRepository
    {
        IPagedList<InDebtByPartnerDTO> GetDebtByPartner(InDebtManagementFilterModel requestFilterModel);
        IPagedList<InDebtByContractDTO> GetDebtByContracts(string partnerId, InDebtManagementFilterModel requestFilterModel);
    }
    public class InDebtManagementQueries : QueryRepository<PaymentVoucher, int>, IInDebtManagementQueries
    {
        public InDebtManagementQueries(DebtDbContext context) : base(context)
        {
        }

        public IPagedList<InDebtByContractDTO> GetDebtByContracts(string partnerId, InDebtManagementFilterModel requestFilterModel)
        {
            if (!requestFilterModel.StartPeriod.HasValue)
            {
                requestFilterModel.StartPeriod = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (!requestFilterModel.EndPeriod.HasValue)
            {
                requestFilterModel.EndPeriod = DateTime.Now;
            }

            var dapperExecution = BuildByTemplateWithoutSelect<InDebtByContractDTO, InDebtSqlBuilder>(requestFilterModel);
            dapperExecution.SqlBuilder.SetDebtPeriod(requestFilterModel.StartPeriod.Value, requestFilterModel.EndPeriod.Value);
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId AS CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("t1.InContractId AS InContractId");
            dapperExecution.SqlBuilder.Select("t1.ContractCode AS ContractCode");
            dapperExecution.SqlBuilder.Select("t1.ProjectName AS ProjectName");

            // Build debt select statement
            dapperExecution.SqlBuilder.SelectDebtStatement();
            dapperExecution.SqlBuilder.Where("t1.IssuedDate BETWEEN DATE(@startPeriod) AND DATE(@endPeriod)", new { startPeriod = requestFilterModel.StartPeriod.Value, endPeriod = requestFilterModel.EndPeriod.Value });
            dapperExecution.SqlBuilder.GroupBy("t1.InContractId");

            // Build debt amount where clauses
            dapperExecution.SqlBuilder.BuildDebtTotalFilterClause(requestFilterModel);

            if (!string.IsNullOrWhiteSpace(partnerId))
            {
                dapperExecution.SqlBuilder.Where("t1.TargetId = @partnerId", new { partnerId });
            }

            dapperExecution.SqlBuilder.WhereValidVoucher();
            return dapperExecution.ExecutePaginateQuery();
        }

        public IPagedList<InDebtByPartnerDTO> GetDebtByPartner(InDebtManagementFilterModel requestFilterModel)
        {
            if (!requestFilterModel.StartPeriod.HasValue)
            {
                requestFilterModel.StartPeriod = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (!requestFilterModel.EndPeriod.HasValue)
            {
                requestFilterModel.EndPeriod = DateTime.Now;
            }

            var dapperExecution = BuildByTemplateWithoutSelect<InDebtByPartnerDTO, InDebtSqlBuilder>(requestFilterModel);
            dapperExecution.SqlBuilder.SetDebtPeriod(requestFilterModel.StartPeriod.Value, requestFilterModel.EndPeriod.Value);
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId AS CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("t1.TargetId AS PartnerId");
            dapperExecution.SqlBuilder.Select("t2.TargetFullName AS PartnerName");
            dapperExecution.SqlBuilder.Select("t2.TargetPhone AS PartnerPhone");

            // Build debt select statement
            dapperExecution.SqlBuilder
                .SelectDebtStatement();

            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets t2 ON t2.Id = t1.TargetId");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails t3 ON t3.ReceiptVoucherId = t1.Id");
            dapperExecution.SqlBuilder.GroupBy("t1.TargetId");
          
            if (!string.IsNullOrWhiteSpace(requestFilterModel.PartnerId))
            {
                dapperExecution.SqlBuilder.Where("t2.Id LIKE @voucherTargetId",
                    new { voucherTargetId = requestFilterModel.PartnerId });
            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel.PartnerTaxCode))
            {
                dapperExecution.SqlBuilder
                    .Where("t2.TargetTaxIdNo LIKE @targetTaxNo", new { targetTaxNo = $"%{requestFilterModel.PartnerTaxCode}%" });
            }
            if (requestFilterModel.Any("serviceId"))
            {
                var serviceIds = requestFilterModel.Get<decimal?>("serviceId", RequestFilterOperator.Equal);
                dapperExecution.SqlBuilder.Where("t3.ServiceId = @serviceId",
                    new { serviceId = serviceIds });
            }
            if (requestFilterModel.Any("projectId"))
            {
                var projectIds = requestFilterModel.Get<decimal?>("projectId", RequestFilterOperator.Equal);
                dapperExecution.SqlBuilder.Where("t1.ProjectId = @projectId",
                    new { projectId = projectIds });
            }
            // Build debt amount where clauses
            dapperExecution.SqlBuilder.BuildDebtTotalFilterClause(requestFilterModel);

            dapperExecution.SqlBuilder.WhereValidVoucher();
            return dapperExecution.ExecutePaginateQuery();
        }
    }

    public class InDebtSqlBuilder : SqlBuilder
    {
        public static string OpeningDebtSelectClause;
        public static string IncurredDebtSelectClause;
        public static string TotalDebtSelectClause;
        public static string ReductionAmountSelectClause;
        public static string PaidAmountSelectClause;
        public static string ClosingDebtSelectClause;

        public static DateTime StartingDebtPeriod;
        public static DateTime EndingDebtPeriod;
        public InDebtSqlBuilder() { }
        public InDebtSqlBuilder(string tableName) : base(tableName) { }

        public void SetDebtPeriod(DateTime startingDebtPeriod, DateTime endingDebtPeriod)
        {
            StartingDebtPeriod = startingDebtPeriod;
            EndingDebtPeriod = endingDebtPeriod;
        }

        public InDebtSqlBuilder BuildDebtTotalFilterClause(InDebtManagementFilterModel requestFilterModel)
        {
            if (requestFilterModel.Any("OpeningDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("OpeningDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("OpeningDebt", RequestFilterOperator.LessThanOrEqual);
                WhereOpeningDebtClause(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("IncurredDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("IncurredDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("IncurredDebt", RequestFilterOperator.LessThanOrEqual);
                WhereIncurredDebtClause(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("TotalDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("TotalDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("TotalDebt", RequestFilterOperator.LessThanOrEqual);
                WhereTotalDebtClause(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("ReductionAmount"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("ReductionAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("ReductionAmount", RequestFilterOperator.LessThanOrEqual);
                WhereRedutionAmountClause(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("PaidAmount"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("PaidAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("PaidAmount", RequestFilterOperator.LessThanOrEqual);
                WherePaidAmountClause(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("ClosingDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("ClosingDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("ClosingDebt", RequestFilterOperator.LessThanOrEqual);
                WhereClosingDebtClause(startingOpeningDebt, endingOpeningDebt);
            }

            return this;
        }

        public InDebtSqlBuilder WhereValidVoucher()
        {
            var invalidStates = PaymentVoucherStatus.InvalidStates().Select(s => s.Id).ToArray();
            Where("t1.IsActive = TRUE");
            Where("t1.StatusId NOT IN @invalidStates", new { invalidStates });
            Where("DATE(t1.IssuedDate) <= CURDATE()");
            return this;
        }

        public InDebtSqlBuilder WhereOpeningDebtClause(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue)
            {
                Having($"{OpeningDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod });
            }

            if (endingDebt.HasValue)
            {
                Having($"{OpeningDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod });
            }

            return this;
        }

        public InDebtSqlBuilder WhereIncurredDebtClause(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue)
            {
                Having($"{IncurredDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue)
            {
                Having($"{IncurredDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public InDebtSqlBuilder WhereTotalDebtClause(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue)
            {
                Having($"{TotalDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue)
            {
                Having($"{TotalDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public InDebtSqlBuilder WhereRedutionAmountClause(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue)
            {
                Having($"{ReductionAmountSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue)
            {
                Having($"{ReductionAmountSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public InDebtSqlBuilder WherePaidAmountClause(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue)
            {
                Having($"{PaidAmountSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue)
            {
                Having($"{PaidAmountSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public InDebtSqlBuilder WhereClosingDebtClause(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue)
            {
                Having($"{ClosingDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue)
            {
                Having($"{ClosingDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            return this;
        }

        public InDebtSqlBuilder SelectDebtStatement()
        {
            // Truy vấn nợ đầu kỳ
            OpeningDebtSelectClause = "SUM(CASE"
                + " WHEN DATE(t1.IssuedDate) < DATE(@startPeriod)"
                + " THEN t1.RemainingTotal"
                + " ELSE 0"
                + " END)";
            Select($"{OpeningDebtSelectClause} AS OpeningDebt", new { startPeriod = StartingDebtPeriod });

            // Truy vấn nợ phát sinh
            IncurredDebtSelectClause = "SUM(CASE"
                + " WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) && DATE(t1.IssuedDate) <= DATE(@endPeriod)"
                + " THEN t1.GrandTotal"
                + " ELSE 0"
                + " END)";
            Select($"{IncurredDebtSelectClause} AS IncurredDebt", new { startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });

            // Truy vấn công nợ phải trả
            TotalDebtSelectClause = $"({OpeningDebtSelectClause} + {IncurredDebtSelectClause})";
            Select($"{TotalDebtSelectClause} AS TotalDebt",
                new { startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });

            // Truy vấn giảm trừ + khuyến mại 
            ReductionAmountSelectClause = "SUM(CASE"
                + " WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) && DATE(t1.IssuedDate) <= DATE(@endPeriod)"
                + " THEN t1.ReductionFreeTotal + t1.PromotionTotalAmount"
                + " ELSE 0"
                + " END)";
            Select($"{ReductionAmountSelectClause} AS ReductionAmount", new { startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });

            // Truy vấn công nợ đã thanh toán
            PaidAmountSelectClause = "SUM(CASE"
                + " WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) && DATE(t1.IssuedDate) <= DATE(@endPeriod)"
                + " THEN t1.PaidTotal"
                + " ELSE 0"
                + " END)";
            Select($"{PaidAmountSelectClause} AS PaidAmount", new { startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });

            // Truy vấn nợ cuối kỳ
            ClosingDebtSelectClause = $"({OpeningDebtSelectClause} + {IncurredDebtSelectClause} - {PaidAmountSelectClause})";
            Select($"{ClosingDebtSelectClause} AS ClosingDebt",
                new { startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });

            return this;
        }
    }
}
