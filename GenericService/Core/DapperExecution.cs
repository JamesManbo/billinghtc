using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using GenericRepository.DapperSqlBuilder;
using GenericRepository.Models;
using Global.Models.Filter;
using Global.Models.PagedList;

namespace GenericRepository.Core
{
    public interface IDapperExecution<TResult>
    {
        TResult ExecuteScalarQuery();
        TResult ExecuteScalarQuery<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn);
        TResult ExecuteScalarQuery<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn);
        TResult ExecuteScalarQuery<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn);
        TResult ExecuteScalarQuery<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn);
        TResult ExecuteScalarQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn);

        Task<TResult> ExecuteScalarQueryAsync();
        Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn);
        Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn);
        Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn);
        Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn);
        Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn);

        IEnumerable<TResult> ExecuteQuery();
        IEnumerable<TResult> ExecuteQuery<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn);
        IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn);
        IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn);
        IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn);
        IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn);

        Task<IEnumerable<TResult>> ExecuteQueryAsync();
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn);
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn);
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn);
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn);
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn);

        IPagedList<TResult> ExecutePaginateQuery();
        IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn);
        IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn);
        IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn);
        IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn);
        IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn);

        Task<IPagedList<TResult>> ExecutePaginateQueryAsync();
        Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn);
        Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn);
        Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn);
        Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn);
        Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn);
    }

    public class DapperExecution<TResult, TSQLBuilder> : DapperExecution<TResult> where TSQLBuilder : SqlBuilder
    {
        public new TSQLBuilder SqlBuilder { get; set; }

        public DapperExecution(IQueryRepository queryRepository, TSQLBuilder sqlBuilder)
            : base(queryRepository, sqlBuilder)
        {
            this.SqlBuilder = sqlBuilder;
        }
    }

    public class DapperExecution<TResult> : IDapperExecution<TResult>
    {

        public DapperExecution(IQueryRepository queryRepository, SqlBuilder sqlBuilder)
        {
            this._queryRepository = queryRepository;
            this.SqlBuilder = sqlBuilder;
        }

        private readonly IQueryRepository _queryRepository;
        public SqlBuilder SqlBuilder { get; set; }
        public SqlBuilder.Template ExecutionCountingTemplate { get; set; }
        public SqlBuilder.Template ExecutionTemplate { get; set; }
        private RequestFilterModel _filterModel;
        public RequestFilterModel FilterModel
        {
            get => _filterModel;
            set
            {
                _filterModel = value;
                if (_filterModel != null)
                {
                    this.Paging = _filterModel.Paging;
                }
            }
        }
        public bool Paging { get; set; }

        public TResult ExecuteScalarQuery()
        {
            return _queryRepository.GetScalarByTemplate<TResult>(ExecutionTemplate);
        }

        public TResult ExecuteScalarQuery<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public TResult ExecuteScalarQuery<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public TResult ExecuteScalarQuery<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public TResult ExecuteScalarQuery<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public TResult ExecuteScalarQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public IEnumerable<TResult> ExecuteQuery()
        {
            return _queryRepository.GetByTemplate<TResult>(ExecutionTemplate);
        }
        public IEnumerable<TResult> ExecuteQuery(Type[] types, Func<object[], TResult> map, string splitOnColumn = "Id")
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, types, map, splitOnColumn);
        }

        public IEnumerable<TResult> ExecuteQuery<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }
        public IEnumerable<TResult> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplate(ExecutionTemplate, callback, splitOnColumn);
        }

        public int ExecuteTotalRecordsQuery()
        {
            if (ExecutionCountingTemplate == null)
            {
                string sqlCountingTemplate;
                if (SqlBuilder.HasGroupByStatement())
                {
                    var groupByField = SqlBuilder.GetGroupByField();
                    sqlCountingTemplate =
                          $"SELECT COUNT(*) FROM (SELECT COUNT(DISTINCT {groupByField}) FROM `{SqlBuilder.TableName}` AS t1 /**innerjoin**//**leftjoin**//**rightjoin**//**where**//**groupby**//**having**/) AS _source";
                }
                else
                {
                    sqlCountingTemplate =
                        $"SELECT COUNT(DISTINCT t1.Id) FROM `{SqlBuilder.TableName}` AS t1 /**innerjoin**//**leftjoin**//**rightjoin**//**where**/";
                }
                // Add counting query template
                ExecutionCountingTemplate = SqlBuilder.AddTemplate(sqlCountingTemplate);
            }
            return _queryRepository.GetScalarByTemplate<int>(ExecutionCountingTemplate);
        }

        public Task<int> ExecuteTotalRecordsQueryAsync()
        {
            if (ExecutionCountingTemplate == null)
            {
                string sqlCountingTemplate;
                if (SqlBuilder.HasGroupByStatement())
                {
                    var groupByField = SqlBuilder.GetGroupByField();
                    sqlCountingTemplate =
                          $"SELECT COUNT(1) FROM (SELECT COUNT(DISTINCT {groupByField}) FROM `{SqlBuilder.TableName}` AS t1 /**innerjoin**//**leftjoin**//**rightjoin**//**where**//**groupby**//**having**/) AS _source";
                }
                else
                {
                    sqlCountingTemplate =
                        $"SELECT COUNT(DISTINCT t1.Id) FROM `{SqlBuilder.TableName}` AS t1 /**innerjoin**//**leftjoin**//**rightjoin**//**where**/";
                }
                // Add counting query template
                ExecutionCountingTemplate = SqlBuilder.AddTemplate(sqlCountingTemplate);
            }

            return _queryRepository.GetScalarByTemplateAsync<int>(ExecutionCountingTemplate);
        }

        public void AddTemplate(string sql, dynamic parameters = null)
        {
            this.ExecutionTemplate = this.SqlBuilder.AddTemplate(sql, parameters);
        }

        public void AddCountingTemplate(string sql, dynamic parameters = null)
        {
            this.ExecutionCountingTemplate = this.SqlBuilder.AddTemplate(sql, parameters);
            this.Paging = true;
        }

        public IPagedList<TResult> ExecutePaginateQuery()
        {
            return QueryPaginateHandler(_queryRepository
                 .GetByTemplateAsync<TResult>(ExecutionTemplate));

        }

        public IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandler(_queryRepository
                .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }
        public IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandler(_queryRepository
                .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandler(_queryRepository
                .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandler(_queryRepository
                .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }
        public IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandler(_queryRepository
                .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public IPagedList<TResult> ExecutePaginateQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandler(_queryRepository
                .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public Task<TResult> ExecuteScalarQueryAsync()
        {
            return _queryRepository.GetScalarByTemplateAsync<TResult>(ExecutionTemplate);
        }

        public Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<TResult> ExecuteScalarQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetScalarByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync()
        {
            return _queryRepository.GetByTemplateAsync<TResult>(ExecutionTemplate);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        public Task<IEnumerable<TResult>> ExecuteQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn)
        {
            return _queryRepository.GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn);
        }

        private IPagedList<TResult> QueryPaginateHandler(Task<IEnumerable<TResult>> queryDataTsk)
        {
            var queryDataIsolateTsk = Task.Run(() => queryDataTsk);
            var queryCountingTsk = Task.Run(() => ExecuteTotalRecordsQueryAsync());
            Task.WhenAll(queryDataIsolateTsk, queryCountingTsk).Wait();

            var dataSource = queryDataTsk.Result?.Distinct()?.ToList();

            int totalRecords = this.Paging ? queryCountingTsk.Result : dataSource?.Count() ?? 0;

            return new PagedList<TResult>(
                this.Paging ? SqlBuilder.SkipNumber : 0,
                this.Paging ? SqlBuilder.TakeNumber : Int32.MaxValue,
                totalRecords)
            {
                Subset = dataSource
            };
        }

        private async Task<IPagedList<TResult>> QueryPaginateHandlerAsync(Task<IEnumerable<TResult>> queryDataTsk)
        {
            var queryDataIsolateTask = Task.Run(() => queryDataTsk);
            var queryCountingTsk = Task.Run(() => ExecuteTotalRecordsQueryAsync());
            await Task.WhenAll(queryDataIsolateTask, queryCountingTsk);

            var dataSource = queryDataTsk.Result?.Distinct()?.ToList();
            int totalRecords = this.Paging ? queryCountingTsk.Result : dataSource?.Count() ?? 0;

            return new PagedList<TResult>(
                this.Paging ? SqlBuilder.SkipNumber : 0,
                this.Paging ? SqlBuilder.TakeNumber : Int32.MaxValue,
                totalRecords)
            {
                Subset = dataSource
            };
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync()
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync<TResult>(ExecutionTemplate));
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond>(Func<TFirst, TSecond, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird>(Func<TFirst, TSecond, TThird, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth>(Func<TFirst, TSecond, TThird, TFourth, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }

        public Task<IPagedList<TResult>> ExecutePaginateQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> callback, string splitOnColumn)
        {
            return QueryPaginateHandlerAsync(_queryRepository
                 .GetByTemplateAsync(ExecutionTemplate, callback, splitOnColumn));
        }
    }
}
