using Dapper;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.DebtModels;
using DebtManagement.Domain.Models.DebtModels.OutDebts;
using GenericRepository;
using GenericRepository.DapperSqlBuilder;
using Global.Models.Auth;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Global.Models.Filter.RequestFilterModel;

namespace DebtManagement.Infrastructure.Queries
{
    public class DebtManagementFilterModel : RequestFilterModel
    {
        public int ServiceId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? StartPeriod { get; set; }
        public DateTime? EndPeriod { get; set; }
        public bool? IsEnterprise { get; set; }
        public int CustomerId { get; set; }
        public string CustomerTaxCode { get; set; }
        public string CustomerBRNo { get; set; }
        public string CashierUserId { get; set; }

    }

    public class OutDebtSqlBuilder : SqlBuilder
    {
        public string OpeningDebtSelectClause;
        public string IncurredDebtSelectClause;
        public string TotalDebtSelectClause;
        public string ReductionAmountSelectClause;
        public string ClearingAmountSelectClause;
        public string PaidAmountSelectClause;
        public string ClosingDebtSelectClause;

        public string NumberOfDebtHistoriesSelectClause
            = "IF(t1.NumberOfDebtHistories > 0, t1.NumberOfDebtHistories, 1)";

        public DateTime StartingDebtPeriod;
        public DateTime EndingDebtPeriod;
        public OutDebtSqlBuilder()
        {
        }

        public OutDebtSqlBuilder(string tableName) : base(tableName)
        {
        }

        public OutDebtSqlBuilder SetDebtPeriod(DateTime? startingDebtPeriod, DateTime? endingDebtPeriod)
        {
            if (!startingDebtPeriod.HasValue)
            {
                startingDebtPeriod = DateTime.UtcNow.AddHours(7);
            }
            else
            {
                startingDebtPeriod = startingDebtPeriod.Value.AddHours(7);
            }

            if (!endingDebtPeriod.HasValue)
            {
                endingDebtPeriod = DateTime.UtcNow.AddHours(7);
            }
            else
            {
                //endingDebtPeriod = endingDebtPeriod.Value.AddHours(7);
                endingDebtPeriod = new DateTime(endingDebtPeriod.Value.Year, endingDebtPeriod.Value.Month, endingDebtPeriod.Value.Day);
            }

            StartingDebtPeriod = startingDebtPeriod.Value;
            EndingDebtPeriod = endingDebtPeriod.Value;
            return this;
        }
        public OutDebtSqlBuilder BuildCashierDebtTotalFilterClause(DebtManagementFilterModel requestFilterModel)
        {
            if (requestFilterModel.Any("OpeningDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("OpeningDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("OpeningDebt", RequestFilterOperator.LessThanOrEqual);
                if (startingOpeningDebt.HasValue)
                {
                    Having($"{CashierOpeningDebtSelectStatement} >= @startingOpeningDebt", new { startingOpeningDebt, startPeriod = StartingDebtPeriod });
                }

                if (endingOpeningDebt.HasValue)
                {
                    Having($"{CashierOpeningDebtSelectStatement} <= @endingOpeningDebt", new { endingOpeningDebt, startPeriod = StartingDebtPeriod });
                }
            }

            if (requestFilterModel.Any("IncurredDebt"))
            {
                var startingIncurredDebt = requestFilterModel.Get<decimal?>("IncurredDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingIncurredDebt = requestFilterModel.Get<decimal?>("IncurredDebt", RequestFilterOperator.LessThanOrEqual);
                if (startingIncurredDebt.HasValue)
                {
                    Having($"{CashierIncurredDebtSelectStatement} >= @startingIncurredDebt", new { startingIncurredDebt, startPeriod = StartingDebtPeriod });
                }

                if (endingIncurredDebt.HasValue)
                {
                    Having($"{CashierIncurredDebtSelectStatement} <= @endingIncurredDebt", new { endingIncurredDebt, startPeriod = StartingDebtPeriod });
                }
            }

            if (requestFilterModel.Any("TotalDebt"))
            {
                var startingTotalDebt = requestFilterModel.Get<decimal?>("TotalDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingTotalDebt = requestFilterModel.Get<decimal?>("TotalDebt", RequestFilterOperator.LessThanOrEqual);
                if (startingTotalDebt.HasValue)
                {
                    Having($"{CashierTotalDebtSelectStatement} >= @startingTotalDebt", new { startingTotalDebt, startPeriod = StartingDebtPeriod });
                }

                if (endingTotalDebt.HasValue)
                {
                    Having($"{CashierTotalDebtSelectStatement} <= @endingTotalDebt", new { endingTotalDebt, startPeriod = StartingDebtPeriod });
                }
            }

            if (requestFilterModel.Any("ClearingAmount"))
            {
                var startingClearingAmount = requestFilterModel.Get<decimal?>("ClearingAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingClearingAmount = requestFilterModel.Get<decimal?>("ClearingAmount", RequestFilterOperator.LessThanOrEqual);
                if (startingClearingAmount.HasValue)
                {
                    Having($"{CashierClearingTotalSelectStatement} >= @startingClearingAmount", new { startingClearingAmount, startPeriod = StartingDebtPeriod });
                }

                if (endingClearingAmount.HasValue)
                {
                    Having($"{CashierClearingTotalSelectStatement} <= @endingClearingAmount", new { endingClearingAmount, startPeriod = StartingDebtPeriod });
                }
            }

            if (requestFilterModel.Any("PaidAmount"))
            {
                var startingPaidAmount = requestFilterModel.Get<decimal?>("PaidAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingPaidAmount = requestFilterModel.Get<decimal?>("PaidAmount", RequestFilterOperator.LessThanOrEqual);
                if (startingPaidAmount.HasValue)
                {
                    Having($"{CashierPaidAmountSelectClause} >= @startingPaidAmount", new { startingPaidAmount, startPeriod = StartingDebtPeriod });
                }

                if (endingPaidAmount.HasValue)
                {
                    Having($"{CashierPaidAmountSelectClause} <= @endingPaidAmount", new { endingPaidAmount, startPeriod = StartingDebtPeriod });
                }
            }

            if (requestFilterModel.Any("ClosingDebt"))
            {
                var startingClosingDebt = requestFilterModel.Get<decimal?>("ClosingDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingClosingDebt = requestFilterModel.Get<decimal?>("ClosingDebt", RequestFilterOperator.LessThanOrEqual);
                if (startingClosingDebt.HasValue)
                {
                    Having($"{CashierClosingDebtSelectStatement} >= @startingClosingDebt", new { startingClosingDebt, startPeriod = StartingDebtPeriod });
                }

                if (endingClosingDebt.HasValue)
                {
                    Having($"{CashierClosingDebtSelectStatement} <= @endingClosingDebt", new { endingClosingDebt, startPeriod = StartingDebtPeriod });
                }
            }

            return this;
        }
        public OutDebtSqlBuilder BuildTargetDebtTotalFilterClause(DebtManagementFilterModel requestFilterModel)
        {
            if (requestFilterModel.Any("OpeningDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("OpeningDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("OpeningDebt", RequestFilterOperator.LessThanOrEqual);
                WhereOpeningDebtRange(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("IncurredDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("IncurredDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("IncurredDebt", RequestFilterOperator.LessThanOrEqual);
                WhereIncurredDebtRange(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("TotalDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("TotalDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("TotalDebt", RequestFilterOperator.LessThanOrEqual);
                WhereTotalDebtRange(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("ReductionAmount"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("ReductionAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("ReductionAmount", RequestFilterOperator.LessThanOrEqual);
                WhereRedutionAmountRange(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("ClearingAmount"))
            {
                var startingClearingAmount = requestFilterModel.Get<decimal?>("ClearingAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingClearingAmount = requestFilterModel.Get<decimal?>("ClearingAmount", RequestFilterOperator.LessThanOrEqual);
                WhereClearingAmountRange(startingClearingAmount, endingClearingAmount);
            }

            if (requestFilterModel.Any("PaidAmount"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("PaidAmount", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("PaidAmount", RequestFilterOperator.LessThanOrEqual);
                WherePaidAmountRange(startingOpeningDebt, endingOpeningDebt);
            }

            if (requestFilterModel.Any("ClosingDebt"))
            {
                var startingOpeningDebt = requestFilterModel.Get<decimal?>("ClosingDebt", RequestFilterOperator.GreaterThanOrEqual);
                var endingOpeningDebt = requestFilterModel.Get<decimal?>("ClosingDebt", RequestFilterOperator.LessThanOrEqual);
                WhereClosingDebtRange(startingOpeningDebt, endingOpeningDebt);
            }

            return this;
        }

        public OutDebtSqlBuilder WhereValidVoucher(UserIdentity currentUser)
        {
            var invalidStates = ReceiptVoucherStatus
                .InvalidStates().Select(s => s.Id).ToArray();
            Where("t1.IsActive = TRUE");
            Where("t1.IsDeleted = FALSE");
            Where("t1.StatusId NOT IN @invalidStates", new { invalidStates });

            if (!currentUser.Roles.Contains("ADMIN"))
            {
                Where("t1.InvalidIssuedDate <> TRUE && t1.IssuedDate IS NOT NULL");
                Where("t1.IssuedDate <= @currentDate", new { currentDate = DateTime.Now });
            }

            return this;
        }

        public OutDebtSqlBuilder InsightWhereValidVoucher(UserIdentity currentUser)
        {
            var insightInvalidStates = ReceiptVoucherStatus
                .InvalidStates().Select(s => s.Id).ToArray();
            InsightWhere("t1.IsActive = TRUE");
            InsightWhere("t1.IsDeleted = FALSE");
            InsightWhere("t1.StatusId NOT IN @insightInvalidStates", new { insightInvalidStates });

            //if (!currentUser.Roles.Contains("ADMIN"))
            //{
            //    InsightWhere("t1.InvalidIssuedDate <> TRUE && t1.IssuedDate IS NOT NULL");
            //    InsightWhere("t1.IssuedDate <= @insightCurrentDate", new { insightCurrentDate = DateTime.Now });
            //}

            return this;
        }

        public OutDebtSqlBuilder WhereOpeningDebtRange(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue && startingDebt > 0)
            {
                Having($"{OpeningDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod });
            }

            if (endingDebt.HasValue && endingDebt > 0)
            {
                Having($"{OpeningDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod });
            }

            return this;
        }
        public OutDebtSqlBuilder WhereIncurredDebtRange(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue && startingDebt > 0)
            {
                Having($"{IncurredDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue && endingDebt > 0)
            {
                Having($"{IncurredDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public OutDebtSqlBuilder WhereTotalDebtRange(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue && startingDebt > 0)
            {
                Having($"{TotalDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue && endingDebt > 0)
            {
                Having($"{TotalDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            return this;
        }
        public OutDebtSqlBuilder WhereRedutionAmountRange(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue && startingDebt > 0 )
            {
                Having($"{ReductionAmountSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue && endingDebt > 0 )
            {
                Having($"{ReductionAmountSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            return this;
        }
        public OutDebtSqlBuilder WhereClearingAmountRange(decimal? startingClearingAmount, decimal? endingClearingAmount)
        {
            if (startingClearingAmount.HasValue && startingClearingAmount > 0 )
            {
                Having($"{ClearingAmountSelectClause} >= @startingDebt", new { startingClearingAmount, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingClearingAmount.HasValue && endingClearingAmount > 0 )
            {
                Having($"{ClearingAmountSelectClause} <= @endingDebt", new { endingClearingAmount, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public OutDebtSqlBuilder WherePaidAmountRange(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue && startingDebt > 0 )
            {
                Having($"{PaidAmountSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue && endingDebt > 0 )
            {
                Having($"{PaidAmountSelectClause} <= @endingDebt", new { endingDebt, stVoucherartPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }
            return this;
        }
        public OutDebtSqlBuilder WhereClosingDebtRange(decimal? startingDebt, decimal? endingDebt)
        {
            if (startingDebt.HasValue && startingDebt > 0 )
            {
                Having($"{ClosingDebtSelectClause} >= @startingDebt", new { startingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            if (endingDebt.HasValue && endingDebt > 0)
            {
                Having($"{ClosingDebtSelectClause} <= @endingDebt", new { endingDebt, startPeriod = StartingDebtPeriod, endPeriod = EndingDebtPeriod });
            }

            return this;
        }

        #region Select cashier debt

        public string SelectNumberOfPaymentDetailsStatement;
        public OutDebtSqlBuilder SelectNumberOfPaymentDetails()
        {
            SelectNumberOfPaymentDetailsStatement = $"";

            return this;
        }

        public string CashierOpeningDebtSelectStatement;
        public OutDebtSqlBuilder SelectCashierOpeningDebtStatement()
        {
            // Truy vấn công nợ đầu kỳ
            CashierOpeningDebtSelectStatement = "SUM(CASE" +
                $" WHEN DATE(dh.IssuedDate) < DATE(@startPeriod)" +
                " THEN dh.CashingPaidTotal*t1.ExchangeRate" +
                " ELSE 0" +
                " END)";

            Select($"{CashierOpeningDebtSelectStatement} AS OpeningDebt",
                new
                {
                    startPeriod = StartingDebtPeriod
                });
            return this;
        }

        public string CashierIncurredDebtSelectStatement;
        public OutDebtSqlBuilder SelectCashierIncurredDebtStatement()
        {
            // Truy vấn công nợ phát sinh
            CashierIncurredDebtSelectStatement = "SUM(CASE" +
                $" WHEN DATE(dh.IssuedDate) >= DATE(@startPeriod) AND DATE(dh.IssuedDate) <= DATE(@endPeriod)" +
                " THEN dh.CashingPaidTotal*t1.ExchangeRate" +
                " ELSE 0" +
                " END)";

            Select($"{CashierIncurredDebtSelectStatement} AS IncurredDebt", new
            {
                startPeriod = StartingDebtPeriod,
                endPeriod = EndingDebtPeriod
            });
            return this;
        }

        public string CashierClearingTotalSelectStatement;
        public OutDebtSqlBuilder SelectCashierClearingTotalStatement()
        {
            // Truy vấn giá trị bù trừ (chi phí phân chia doanh thu, hoa hồng, KH chuyển khoản trực tiếp về HTC)
            CashierClearingTotalSelectStatement = "SUM(CASE" +
                $" WHEN DATE(dh.IssuedDate) >= DATE(@startPeriod) AND DATE(dh.IssuedDate) <= DATE(@endPeriod) AND rpd.PaymentMethod = {VoucherPaymentMethod.Transfer.Id}" +
                " THEN (dh.TransferringPaidTotal + (t1.ClearingTotal/IF(t1.NumberOfDebtHistories > 0, t1.NumberOfDebtHistories, 1)))*t1.ExchangeRate" +
                " ELSE 0" +
                " END)";
            Select($"{CashierClearingTotalSelectStatement} AS ReductionAmount");
            return this;
        }

        public string CashierTotalDebtSelectStatement;
        public OutDebtSqlBuilder SelectCashierTotalDebtStatement()
        {
            // Truy vấn công nợ phải trả
            CashierTotalDebtSelectStatement = "SUM(dh.CashingPaidTotal*t1.ExchangeRate)";
            Select($"{CashierTotalDebtSelectStatement} AS TotalDebt");
            return this;
        }

        public string CashierPaidAmountSelectClause;
        public OutDebtSqlBuilder SelectCashierPaidDebtStatement()
        {
            // Truy vấn công nợ đã thanh toán
            CashierPaidAmountSelectClause = "SUM(dh.CashingAccountedTotal*t1.ExchangeRate)";
            Select($"{CashierPaidAmountSelectClause} AS PaidAmount");
            return this;
        }

        public string CashierClosingDebtSelectStatement;
        public OutDebtSqlBuilder SelectCashierClosingDebtStatement()
        {
            if (string.IsNullOrWhiteSpace(CashierTotalDebtSelectStatement))
            {
                this.SelectCashierClosingDebtStatement();
            }

            if (string.IsNullOrWhiteSpace(CashierPaidAmountSelectClause))
            {
                this.SelectCashierPaidDebtStatement();
            }
            // Truy vấn nợ cuối kỳ
            CashierClosingDebtSelectStatement = $"({TotalDebtSelectClause} - {PaidAmountSelectClause})";

            Select($"{CashierClosingDebtSelectStatement} AS ClosingDebt");
            return this;
        }

        #endregion

        #region Select customer debt

        public OutDebtSqlBuilder SelectOpeningDebt()
        {
            //// Truy vấn nợ đầu kỳ
            //OpeningDebtSelectClause = "SUM(CASE"
            //    + $" WHEN DATE(@startPeriod) > CURDATE()"
            //    + $" THEN t1.TargetDebtRemaningTotal/{NumberOfDebtHistoriesSelectClause}"
            //    + $" WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) AND DATE(t1.IssuedDate) <= DATE(@endPeriod) AND DATE(@startPeriod) <= CURDATE()" //AND odh.Status = {(int) PaymentDetailStatus.Accounted}
            //    + $" THEN IFNULL(odh.OpeningTargetDebtTotal, 0)"
            //    + $" ELSE 0"
            //    + $" END)";

            // Truy vấn nợ đầu kỳ
            OpeningDebtSelectClause = "SUM(CASE"
                + $" WHEN DATE(t1.IssuedDate) < DATE(@startPeriod)"
                + $" THEN t1.TargetDebtRemaningTotal*t1.ExchangeRate/{NumberOfDebtHistoriesSelectClause}"
                + $" ELSE 0"
                + $" END)";

            Select($"{OpeningDebtSelectClause} AS OpeningDebt",
                new
                {
                    startPeriod = StartingDebtPeriod
                });

            return this;
        }

        public OutDebtSqlBuilder OrderByOpeningDebt(bool dir = true)
        {
            InsightOrderBy($"{OpeningDebtSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"OpeningDebt {(dir ? " ASC" : "DESC")}");
            return this;
        }

        public OutDebtSqlBuilder SelectIncurredDebtStatement()
        {
            // Truy vấn nợ phát sinh
            IncurredDebtSelectClause = "SUM(CASE"
                + $" WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) AND DATE(t1.IssuedDate) <= DATE(@endPeriod)"
                + $" THEN t1.GrandTotal*t1.ExchangeRate/{NumberOfDebtHistoriesSelectClause}"
                + " ELSE 0"
                + " END)";

            Select($"{IncurredDebtSelectClause} AS IncurredDebt",
                new
                {
                    startPeriod = StartingDebtPeriod,
                    endPeriod = EndingDebtPeriod
                });
            return this;
        }

        public OutDebtSqlBuilder OrderByIncurredDebt(bool dir = true)
        {
            InsightOrderBy($"{IncurredDebtSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"IncurredDebt {(dir ? " ASC" : "DESC")}");
            return this;
        }

        public OutDebtSqlBuilder SelectTotalDebtStatement()
        {
            // Truy vấn công nợ phải trả
            TotalDebtSelectClause = $"({OpeningDebtSelectClause} + {IncurredDebtSelectClause})";
            Select($"{TotalDebtSelectClause} AS TotalDebt");
            return this;
        }

        public OutDebtSqlBuilder OrderByTotalDebt(bool dir = true)
        {
            InsightOrderBy($"{TotalDebtSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"TotalDebt {(dir ? " ASC" : "DESC")}");
            return this;
        }

        public OutDebtSqlBuilder SelectReductionAmountStatement()
        {
            // Truy vấn giảm trừ + khuyến mại 
            ReductionAmountSelectClause = "SUM(CASE"
               + $" WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) AND DATE(t1.IssuedDate) <= DATE(@endPeriod)"
               + $" THEN (t1.ReductionFreeTotal + t1.PromotionTotalAmount + t1.DiscountAmountSuspendTotal)*t1.ExchangeRate/{NumberOfDebtHistoriesSelectClause}"
               + " ELSE 0"
               + " END)";
            Select($"{ReductionAmountSelectClause} AS ReductionAmount");
            return this;
        }

        public OutDebtSqlBuilder OrderByReductionDebt(bool dir = true)
        {
            InsightOrderBy($"{ReductionAmountSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"ReductionAmount {(dir ? " ASC" : "DESC")}");
            return this;
        }

        public OutDebtSqlBuilder SelectClearingAmountStatement()
        {
            // Truy vấn bù trừ
            ClearingAmountSelectClause = "SUM(CASE"
               + $" WHEN DATE(t1.IssuedDate) >= DATE(@startPeriod) AND DATE(t1.IssuedDate) <= DATE(@endPeriod)"
               + $" THEN t1.ClearingTotal*t1.ExchangeRate/{NumberOfDebtHistoriesSelectClause}"
               + " ELSE 0"
               + " END)";
            Select($"{ClearingAmountSelectClause} AS ClearingAmount");
            return this;
        }

        public OutDebtSqlBuilder OrderByClearingAmount(bool dir = true)
        {
            InsightOrderBy($"{ClearingAmountSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"ClearingAmount {(dir ? " ASC" : "DESC")}");
            return this;
        }

        public OutDebtSqlBuilder SelectPaidAmountStatement()
        {
            // Truy vấn công nợ đã thanh toán
            PaidAmountSelectClause = "SUM(CASE"
                + $" WHEN DATE(dh.IssuedDate) >= DATE(@startPeriod) AND DATE(dh.IssuedDate) <= DATE(@endPeriod) AND dh.Status = {(int)PaymentStatus.Accounted}"
                + $" THEN (dh.CashingAccountedTotal + dh.TransferringAccountedTotal)*t1.ExchangeRate"
                + $" WHEN DATE(dh.IssuedDate) >= DATE(@startPeriod) AND DATE(dh.IssuedDate) <= DATE(@endPeriod) AND dh.Status <> {(int)PaymentStatus.Accounted}"
                + $" THEN dh.CashingPaidTotal*t1.ExchangeRate"
                + " ELSE 0"
                + " END)";

            Select($"{PaidAmountSelectClause} AS PaidAmount");
            return this;
        }

        public OutDebtSqlBuilder OrderByPaidAmount(bool dir = true)
        {
            InsightOrderBy($"{PaidAmountSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"PaidAmount {(dir ? " ASC" : "DESC")}");
            return this;
        }

        public OutDebtSqlBuilder SelectClosingDebtStatement()
        {
            if (string.IsNullOrWhiteSpace(TotalDebtSelectClause))
            {
                this.SelectTotalDebtStatement();
            }

            if (string.IsNullOrWhiteSpace(PaidAmountSelectClause))
            {
                this.SelectPaidAmountStatement();
            }
            // Truy vấn nợ cuối kỳ
            ClosingDebtSelectClause = $"({TotalDebtSelectClause} - {PaidAmountSelectClause})";

            Select($"{ClosingDebtSelectClause} AS ClosingDebt");
            return this;
        }

        public OutDebtSqlBuilder OrderByClosingAmount(bool dir = true)
        {
            InsightOrderBy($"{ClosingDebtSelectClause} {(dir ? "ASC" : "DESC")}");
            OrderBy($"ClosingDebt {(dir ? " ASC" : "DESC")}");
            return this;
        }


        #endregion
    }

    public interface IOutDebtManagementQueries : IQueryRepository
    {
        Task<IPagedList<OutDebtByCustomerDTO>> GetDebtByCustomers(DebtManagementFilterModel requestFilterModel);
        IPagedList<OutDebtByContractDTO> GetDebtByContracts(DebtManagementFilterModel requestFilterModel);
        IPagedList<OutDebtCollectionOnBehalfDTO> GetDebtCollectionOnBehalf(DebtManagementFilterModel requestFilterModel);
        decimal GetOpeningDebtAmount(string targetUserId, DateTime openingDebtPeriod, int? excludeVchrId = null);

        IEnumerable<OpeningDebtByReceiptVoucherModel> GetOpeningDebtByTarget(string targetId, DateTime openingDebtPeriod, int? excludeVchrId = null);
    }
    public class OutDebtManagementQueries : QueryRepository<ReceiptVoucher, int>, IOutDebtManagementQueries
    {
        private readonly IVoucherTargetQueries _voucherTargetQueries;
        public OutDebtManagementQueries(DebtDbContext context,
            IVoucherTargetQueries voucherTargetQueries
            )
            : base(context)
        {
            _voucherTargetQueries = voucherTargetQueries;
        }

        private (bool NeedToOrder, bool NeedToFilter) NeedToBeJoinDebtHistory(DebtManagementFilterModel requestFilterModel)
        {
            var needJoinToFilter = requestFilterModel.Any("OpeningDebt") ||
                requestFilterModel.Any("IncurredDebt") ||
                requestFilterModel.Any("TotalDebt") ||
                requestFilterModel.Any("ReductionAmount") ||
                requestFilterModel.Any("ClearingAmount") ||
                requestFilterModel.Any("PaidAmount") ||
                requestFilterModel.Any("ClosingDebt");

            var needJoinToOrder =
                "OpeningDebt".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "IncurredDebt".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "TotalDebt".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "ReductionAmount".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "ClearingAmount".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "PaidAmount".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "ClosingDebt".EqualsIgnoreCase(requestFilterModel.OrderBy);

            return (needJoinToOrder, needJoinToFilter);
        }

        private (bool NeedToOrder, bool NeedToFilter) NeedToJoinVoucherTarget(DebtManagementFilterModel requestFilterModel)
        {
            var needToOrder = "CustomerName".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "CustomerPhone".EqualsIgnoreCase(requestFilterModel.OrderBy);

            var needToFilter = requestFilterModel.Any("IsEnterprise") ||
                    requestFilterModel.Any("CustomerId") ||
                    requestFilterModel.Any("CustomerTaxCode") ||
                    requestFilterModel.Any("CustomerBRNo") ||
                    requestFilterModel.CustomerId > 0 ||
                    requestFilterModel.IsEnterprise.HasValue ||
                    !string.IsNullOrWhiteSpace(requestFilterModel.CustomerTaxCode) ||
                    !string.IsNullOrWhiteSpace(requestFilterModel.CustomerTaxCode);

            return (needToOrder, needToFilter);

        }

        public IPagedList<OutDebtByContractDTO> GetDebtByContracts(DebtManagementFilterModel requestFilterModel)
        {
            if (!requestFilterModel.StartPeriod.HasValue)
            {
                requestFilterModel.StartPeriod = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (!requestFilterModel.EndPeriod.HasValue)
            {
                requestFilterModel.EndPeriod = DateTime.Now;
            }
            else
            {
                requestFilterModel.EndPeriod = new DateTime(requestFilterModel.EndPeriod.Value.Year, requestFilterModel.EndPeriod.Value.Month, requestFilterModel.EndPeriod.Value.Day);
            }

            var sqlInnerTemplate = @"SELECT DISTINCT t1.OutContractId FROM `ReceiptVouchers` AS t1" +
                $"/**insightjoin**//**insightleftjoin**//**insightrightjoin**/" +
                $"/**innerwhere**/" +
                $"/**innergroupby**//**having**/" +
                $"/**innerorderby**//**take**//**skip**/";

            var sqlTemplate =
                $"SELECT\n/**select**/\nFROM" +
                $"\n(" +
                sqlInnerTemplate +
                $"\n) AS s" +
                $"\nINNER JOIN ReceiptVouchers AS t1 ON t1.OutContractId = s.OutContractId" +
                $"\nINNER JOIN VoucherTargets AS vt ON t1.TargetId = vt.Id" +
                $"\nLEFT JOIN ReceiptVoucherDebtHistories AS dh ON dh.ReceiptVoucherId = t1.Id" +
                $"/**where**//**groupby**//**orderby**/";

            var sqlCoutingTemplate = $"SELECT COUNT(1) FROM (" +
                $"\nSELECT DISTINCT t1.OutContractId FROM `ReceiptVouchers` AS t1" +
                $"/**insightjoin**//**insightleftjoin**//**innerwhere**//**innergroupby**//**having**/" +
                $"\n) s";

            var needJoinToDebtHistory = this.NeedToBeJoinDebtHistory(requestFilterModel);

            var dapperExecution = Build<OutDebtByContractDTO, OutDebtSqlBuilder>();
            dapperExecution.SqlBuilder.SetDebtPeriod(requestFilterModel.StartPeriod.Value, requestFilterModel.EndPeriod.Value);
            dapperExecution.SqlBuilder.Select("t1.OutContractId AS OutContractId");
            dapperExecution.SqlBuilder.Select("t1.ContractCode AS ContractCode");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId AS CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS CurrencyUnitCode");

            dapperExecution.SqlBuilder.Select("vt.TargetFullName AS CustomerName");

            // Build debt select statement
            dapperExecution.SqlBuilder.SelectOpeningDebt()
                .SelectIncurredDebtStatement()
                .SelectReductionAmountStatement()
                .SelectTotalDebtStatement()
                .SelectPaidAmountStatement()
                .SelectClosingDebtStatement();

            if (needJoinToDebtHistory.NeedToFilter
                || needJoinToDebtHistory.NeedToOrder
                || !string.IsNullOrWhiteSpace(requestFilterModel.CashierUserId))
            {
                dapperExecution.SqlBuilder.InsightLeftJoin("ReceiptVoucherDebtHistories dh ON dh.ReceiptVoucherId = t1.Id");
                dapperExecution.SqlBuilder.InsightGroupBy("t1.OutContractId");
            }

            // Build debt amount where clauses
            dapperExecution
               .SqlBuilder.BuildTargetDebtTotalFilterClause(requestFilterModel);

            if (requestFilterModel.CustomerId > 0)
            {
                dapperExecution.SqlBuilder.InsightWhere("t1.TargetId = @customerId", new { customerId = requestFilterModel.CustomerId });
            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel.CashierUserId))
            {
                dapperExecution.SqlBuilder.InsightWhere("dh.CashierUserId = @cashierUserId", new { cashierUserId = requestFilterModel.CashierUserId });
            }

            dapperExecution.SqlBuilder.WhereValidVoucher(UserIdentity);

            if (requestFilterModel.Any("contractCode"))
            {
                dapperExecution.SqlBuilder.Where("t1.ContractCode = @contractCode",
                    new { contractCode = requestFilterModel.Get("contractCode") }
                );
            }

            if (requestFilterModel.Any("serviceId"))
            {
                dapperExecution.SqlBuilder.InsightWhere(
                    "EXISTS (SELECT 1 FROM ReceiptVoucherDetails WHERE ReceiptVoucherId = t1.Id AND ServiceId = @serviceId)",
                    new { serviceId = requestFilterModel.Get("serviceId") });
            }

            if (requestFilterModel.Any("projectId"))
            {
                dapperExecution.SqlBuilder.InsightWhere(
                    "EXISTS (SELECT 1 FROM ReceiptVoucherDetails WHERE ReceiptVoucherId = t1.Id AND ProjectId = @projectId)",
                    new { projectId = requestFilterModel.Get("projectId") });
            }

            if (requestFilterModel?.Paging == true)
            {
                dapperExecution.SqlBuilder.Take(requestFilterModel.Take)
                    .Skip(requestFilterModel.Skip);
            }

            bool orderDirection = requestFilterModel.Dir.EqualsIgnoreCase("ASC");
            if (requestFilterModel?.OrderBy.EqualsIgnoreCase("OpeningDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByOpeningDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("IncurredDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByIncurredDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("ReductionAmount") == true)
            {
                dapperExecution.SqlBuilder.OrderByReductionDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("TotalDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByTotalDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("PaidAmount") == true)
            {
                dapperExecution.SqlBuilder.OrderByPaidAmount(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("ClosingDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByClosingAmount(orderDirection);
            }
            else
            {
                dapperExecution.SqlBuilder.InsightOrderBy("t1.Id", dir: false);
            }
            //dapperExecution.SqlBuilder.Where("t1.IssuedDate BETWEEN DATE(@startPeriod) AND DATE(@endPeriod)", new { startPeriod = requestFilterModel.StartPeriod.Value, endPeriod = requestFilterModel.EndPeriod.Value });

            dapperExecution.SqlBuilder.GroupBy("t1.OutContractId");

            dapperExecution.AddTemplate(sqlTemplate);
            dapperExecution.AddCountingTemplate(sqlCoutingTemplate);
            return dapperExecution.ExecutePaginateQuery();
        }

        public Task<IPagedList<OutDebtByCustomerDTO>> GetDebtByCustomers(DebtManagementFilterModel requestFilterModel)
        {
            var sqlInnerTemplate = "SELECT DISTINCT vt.Id FROM `VoucherTargets` AS vt" +
                $"/**insightjoin**//**insightleftjoin**/" +
                $"/**innerwhere**/" +
                $"/**innergroupby**//**having**/";

            var sqlTemplate =
                $"SELECT\n/**select**/\nFROM" +
                $"\n(" +
                sqlInnerTemplate +
                $"/**innerorderby**//**take**//**skip**/" +
                $"\n) AS s" +
                $"\nINNER JOIN VoucherTargets AS vt ON s.Id = vt.Id" +
                $"\nINNER JOIN ReceiptVouchers AS t1 ON t1.TargetId = vt.Id" +
                $"\nLEFT JOIN ReceiptVoucherDebtHistories AS dh ON dh.ReceiptVoucherId = t1.Id" +
                $"/**where**//**groupby**//**orderby**/";

            var sqlCoutingTemplate = $"SELECT COUNT(1) FROM (" +
                sqlInnerTemplate +
                $"\n) s";

            var needJoinToDebtHistory = this.NeedToBeJoinDebtHistory(requestFilterModel);

            var dapperExecution = Build<OutDebtByCustomerDTO, OutDebtSqlBuilder>();
            dapperExecution.SqlBuilder.SetDebtPeriod(requestFilterModel.StartPeriod, requestFilterModel.EndPeriod);

            //dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode");
            //dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("vt.Id AS CustomerId");
            dapperExecution.SqlBuilder.Select("vt.TargetFullName AS CustomerName");
            dapperExecution.SqlBuilder.Select("vt.TargetPhone AS CustomerPhone");
            dapperExecution.SqlBuilder.Select("vt.TargetTaxIdNo AS CustomerTaxNo");
            dapperExecution.SqlBuilder.Select("vt.TargetCode AS CustomerCode");
            dapperExecution.SqlBuilder.Select("COUNT(DISTINCT t1.OutContractId) as TotalContract");

            // Build debt select statement
            dapperExecution.SqlBuilder.SelectOpeningDebt()
                .SelectIncurredDebtStatement()
                .SelectReductionAmountStatement()
                .SelectTotalDebtStatement()
                .SelectPaidAmountStatement()
                .SelectClosingDebtStatement()
                .SelectClearingAmountStatement();

            dapperExecution.SqlBuilder.GroupBy("t1.TargetId");
            dapperExecution.SqlBuilder.InsightJoin("ReceiptVouchers t1 ON t1.TargetId = vt.Id");
            dapperExecution.SqlBuilder.InsightWhereValidVoucher(UserIdentity);

            if (requestFilterModel.IsEnterprise.HasValue)
            {
                dapperExecution.SqlBuilder
                    .InsightWhere("vt.IsEnterprise = @isEnterprise",
                        new { isEnterprise = requestFilterModel.IsEnterprise.Value });
            }

            if (requestFilterModel.CustomerId > 0)
            {
                dapperExecution.SqlBuilder.InsightWhere("vt.Id = @voucherTargetId",
                    new { voucherTargetId = requestFilterModel.CustomerId });
            }

            if (requestFilterModel.Any("serviceId"))
            {
                dapperExecution.SqlBuilder.InsightWhere(
                    "EXISTS (SELECT 1 FROM ReceiptVoucherDetails WHERE ReceiptVoucherId = t1.Id AND ServiceId = @serviceId)",
                    new { serviceId = requestFilterModel.Get("serviceId") });
            }

            if (requestFilterModel.Any("projectId"))
            {
                dapperExecution.SqlBuilder.InsightWhere(
                    "EXISTS (SELECT 1 FROM ReceiptVoucherDetails WHERE ReceiptVoucherId = t1.Id AND ProjectId = @projectId)",
                    new { projectId = requestFilterModel.Get("projectId") });
            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel.CustomerTaxCode))
            {
                dapperExecution.SqlBuilder
                    .InsightWhere("vt.TargetTaxIdNo LIKE @targetTaxNo", new { targetTaxNo = $"%{requestFilterModel.CustomerTaxCode}%" });
            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel.CustomerBRNo))
            {
                dapperExecution.SqlBuilder
                    .InsightWhere("vt.TargetBRNo LIKE @targetBRNo", new { targetBRNo = $"%{requestFilterModel.CustomerBRNo}%" });
            }

            // Build debt amount where clauses
            dapperExecution.SqlBuilder.BuildTargetDebtTotalFilterClause(requestFilterModel);

            dapperExecution.SqlBuilder.WhereValidVoucher(UserIdentity);

            if (requestFilterModel?.Paging == true)
            {
                dapperExecution.SqlBuilder
                    .Take(requestFilterModel.Take)
                    .Skip(requestFilterModel.Skip);
            }

            bool orderDirection = requestFilterModel.Dir.EqualsIgnoreCase("ASC");
            if (requestFilterModel?.OrderBy.EqualsIgnoreCase("OpeningDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByOpeningDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("IncurredDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByIncurredDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("ReductionAmount") == true)
            {
                dapperExecution.SqlBuilder.OrderByReductionDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("TotalDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByTotalDebt(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("PaidAmount") == true)
            {
                dapperExecution.SqlBuilder.OrderByPaidAmount(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("ClosingDebt") == true)
            {
                dapperExecution.SqlBuilder.OrderByClosingAmount(orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("CustomerName") == true)
            {
                dapperExecution.SqlBuilder.InsightOrderBy("vt.TargetFullName", dir: orderDirection);
            }
            else if (requestFilterModel?.OrderBy.EqualsIgnoreCase("CustomerPhone") == true)
            {
                dapperExecution.SqlBuilder.InsightOrderBy("vt.TargetPhone", dir: orderDirection);
            }
            else
            {
                dapperExecution.SqlBuilder.InsightOrderBy("vt.CurrentDebt", dir: orderDirection);
            }

            if (needJoinToDebtHistory.NeedToFilter || needJoinToDebtHistory.NeedToOrder)
            {
                dapperExecution.SqlBuilder.InsightLeftJoin("ReceiptVoucherDebtHistories dh ON dh.ReceiptVoucherId = t1.Id");
                dapperExecution.SqlBuilder.InsightGroupBy("t1.TargetId");
            }

            // dapperExecution.SqlBuilder.GroupBy("t1.CurrencyUnitId");
            dapperExecution.AddTemplate(sqlTemplate);
            dapperExecution.AddCountingTemplate(sqlCoutingTemplate);

            return dapperExecution.ExecutePaginateQueryAsync();
        }

        public decimal GetOpeningDebtAmount(string targetUserId, DateTime startPeriod, int? excludeVchrId = null)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<decimal, OutDebtSqlBuilder>();

            dapperExecution.SqlBuilder
                .SetDebtPeriod(startPeriod, DateTime.UtcNow.AddHours(7))
                .SelectOpeningDebt();

            dapperExecution.SqlBuilder.LeftJoin("ReceiptVoucherPaymentDetails rpd ON rpd.ReceiptVoucherId = t1.Id AND rpd.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.GroupBy("t1.TargetId");

            // Build debt amount where clauses
            var invalidStates = ReceiptVoucherStatus.InvalidStates().Select(s => s.Id).ToArray();
            dapperExecution.SqlBuilder
                .Where("t1.TargetId LIKE @targetUserId", new { targetUserId })
                .Where("t1.IsActive = TRUE")
                .Where("t1.StatusId NOT IN @invalidStates", new { invalidStates })
                .Where("DATE(t1.IssuedDate) <= CURDATE()");

            if (excludeVchrId.HasValue && excludeVchrId.Value > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.Id != @excludeVchrId", new { excludeVchrId });
            }
            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<OpeningDebtByReceiptVoucherModel> GetOpeningDebtByTarget(string targetId, DateTime openingDebtPeriod, int? excludeVchrId = null)
        {
            var dapperExecution = BuildByTemplate<OpeningDebtByReceiptVoucherModel, OutDebtSqlBuilder>();

            if (int.TryParse(targetId, out var targetIdAsInt))
            {
                dapperExecution.SqlBuilder.Where("t1.TargetId = @targetIdAsInt", new { targetIdAsInt });
            }
            else
            {
                dapperExecution.SqlBuilder.Where(
                    "t1.TargetId = (SELECT Id From VoucherTargets WHERE IdentityGuid = @targetId LIMIT 1)",
                    new { targetId });
            }


            dapperExecution.SqlBuilder.SetDebtPeriod(openingDebtPeriod, DateTime.Now);

            dapperExecution.SqlBuilder.Select("t1.`Id` AS `ReceiptVoucherId`");
            dapperExecution.SqlBuilder.Select("t1.`VoucherCode` AS `ReceiptVoucherCode`");
            dapperExecution.SqlBuilder.Select("t1.`Content` AS `ReceiptVoucherContent`");
            dapperExecution.SqlBuilder.Select("t1.`TargetDebtRemaningTotal` AS `OpeningTargetDebtTotal`");
            dapperExecution.SqlBuilder.Select("t1.`CashierDebtRemaningTotal` AS `OpeningCashierDebtTotal`");

            dapperExecution.SqlBuilder.Where("DATE(t1.IssuedDate) <= DATE(@openingDebtPeriod)", new { openingDebtPeriod })
                    .Where("t1.TargetDebtRemaningTotal > 0")
                    .Where("t1.IsActive = TRUE");
            if (excludeVchrId.HasValue && excludeVchrId.Value > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.Id != @excludeVchrId", new { excludeVchrId });
            }

            //dapperExecution.SqlBuilder.Where("NOT EXISTS ("
            //    + " SELECT rvpd.Id FROM ReceiptVoucherPaymentDetails rvpd"
            //    + " WHERE t1.Id = rvpd.ReceiptVoucherId"
            //    + " AND rvpd.IsDeleted = FALSE"
            //    + " AND rvpd.SubstituteVoucherId IS NOT NULL AND rvpd.SubstituteVoucherId <> ''"
            //    + " AND rvpd.Status <> @accountedStatus"
            //    + ")",
            //    new
            //    {
            //        accountedStatus = (int) PaymentDetailStatus.Accounted
            //    }
            //);
            //dapperExecution.SqlBuilder.Where("t1.StatusId <> @pendingStatus", new { pendingStatus = ReceiptVoucherStatus.Pending.Id });

            dapperExecution.SqlBuilder.WhereValidVoucher(UserIdentity);
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<OutDebtCollectionOnBehalfDTO> GetDebtCollectionOnBehalf(DebtManagementFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<OutDebtCollectionOnBehalfDTO, OutDebtSqlBuilder>(requestFilterModel);
            dapperExecution.SqlBuilder.SetDebtPeriod(requestFilterModel.StartPeriod, requestFilterModel.EndPeriod);
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId AS CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode AS CurrencyUnitCode");
            dapperExecution.SqlBuilder.Select("dh.CashierUserId AS CashierUserId");
            dapperExecution.SqlBuilder.Select("dh.CashierUserName AS CashierUserName");
            dapperExecution.SqlBuilder.Select("dh.CashierFullName AS CashierFullName");

            // Build debt select statement
            dapperExecution.SqlBuilder.SelectCashierOpeningDebtStatement()
                .SelectCashierIncurredDebtStatement()
                .SelectCashierTotalDebtStatement()
                .SelectCashierPaidDebtStatement()
                .SelectCashierTotalDebtStatement();

            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDebtHistories dh ON t1.Id = dh.ReceiptVoucherId");
            dapperExecution.SqlBuilder.GroupBy("dh.CashierUserId");

            dapperExecution.SqlBuilder.Where("dh.CashierUserId IS NOT NULL AND dh.CashierUserId <> ''");

            if (requestFilterModel.CustomerId > 0)
            {
                dapperExecution.SqlBuilder.Where("dh.CashierUserId LIKE @cashierUserId",
                    new { cashierUserId = requestFilterModel.CashierUserId });
            }

            // Build debt amount where clauses
            dapperExecution.SqlBuilder.BuildCashierDebtTotalFilterClause(requestFilterModel);

            dapperExecution.SqlBuilder.Where("t1.StatusId <> @pendingStatus", new { pendingStatus = ReceiptVoucherStatus.Pending.Id });
            dapperExecution.SqlBuilder.WhereValidVoucher(UserIdentity);

            return dapperExecution.ExecutePaginateQuery();
        }

    }
}