using Dapper;
using GenericRepository.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Core;
using GenericRepository.DapperSqlBuilder;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Microsoft.EntityFrameworkCore.Metadata;
using Global.Models.Auth;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace GenericRepository
{
    public abstract class QueryRepository<TEntity, TEType> : IQueryRepository
        where TEntity : class, IEntity<TEType>, new()
    {
        public UserIdentity UserIdentity { get; set; }
        public bool IsRestrictByOrganization { get; set; }
        public int OrganizeDataLevel { get; set; } = 0;
        public bool IsRestrictByRole { get; set; }

        protected readonly string TableName;
        protected string ConnectionString;
        protected readonly IEnumerable<IProperty> TableColumns;

        private readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>();
        private string sqlParameterPrefix = "__";

        protected readonly DbContext CurrentContext;

        protected QueryRepository(DbContext context)
        {
            this.CurrentContext = context;
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            this.ConnectionString = context.Database.GetDbConnection().ConnectionString;

            this.TableName = entityType.GetTableName();
            this.TableColumns = entityType.GetProperties();

            _typeMap[typeof(byte)] = DbType.Byte;
            _typeMap[typeof(sbyte)] = DbType.SByte;
            _typeMap[typeof(short)] = DbType.Int16;
            _typeMap[typeof(ushort)] = DbType.UInt16;
            _typeMap[typeof(int)] = DbType.Int32;
            _typeMap[typeof(uint)] = DbType.UInt32;
            _typeMap[typeof(long)] = DbType.Int64;
            _typeMap[typeof(ulong)] = DbType.UInt64;
            _typeMap[typeof(float)] = DbType.Single;
            _typeMap[typeof(double)] = DbType.Double;
            _typeMap[typeof(decimal)] = DbType.Decimal;
            _typeMap[typeof(bool)] = DbType.Boolean;
            _typeMap[typeof(string)] = DbType.String;
            _typeMap[typeof(char)] = DbType.StringFixedLength;
            _typeMap[typeof(Guid)] = DbType.Guid;
            _typeMap[typeof(DateTime)] = DbType.DateTime;
            _typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            _typeMap[typeof(byte[])] = DbType.Binary;
            _typeMap[typeof(byte?)] = DbType.Byte;
            _typeMap[typeof(sbyte?)] = DbType.SByte;
            _typeMap[typeof(short?)] = DbType.Int16;
            _typeMap[typeof(ushort?)] = DbType.UInt16;
            _typeMap[typeof(int?)] = DbType.Int32;
            _typeMap[typeof(uint?)] = DbType.UInt32;
            _typeMap[typeof(long?)] = DbType.Int64;
            _typeMap[typeof(ulong?)] = DbType.UInt64;
            _typeMap[typeof(float?)] = DbType.Single;
            _typeMap[typeof(double?)] = DbType.Double;
            _typeMap[typeof(decimal?)] = DbType.Decimal;
            _typeMap[typeof(bool?)] = DbType.Boolean;
            _typeMap[typeof(char?)] = DbType.StringFixedLength;
            _typeMap[typeof(Guid?)] = DbType.Guid;
            _typeMap[typeof(DateTime?)] = DbType.DateTime;
            _typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
        }

        public void SetConnectionString(string connString)
        {
            if (string.IsNullOrWhiteSpace(connString)) return;
            this.ConnectionString = connString;
        }


        private IDbConnection GetConnection(string connectionString)
        {
            try
            {
                var dbConnection = new MySqlConnection(connectionString);
                dbConnection.Open();

                return dbConnection;
            }
            catch (Exception e)
            {
                e.Data["SqlGenericRepository.Message-CreateDbConnection"] = "Not new SqlConnection";
                e.Data["SqlGenericRepository.ConnectionString"] = connectionString;
                throw;
            }
        }

        private async Task<IDbConnection> GetConnectionAsync(string connectionString)
        {
            try
            {
                var dbConnection = new MySqlConnection(connectionString);
                await dbConnection.OpenAsync();

                return dbConnection;
            }
            catch (Exception e)
            {
                e.Data["SqlGenericRepository.Message-CreateDbConnection"] = "Not new SqlConnection";
                e.Data["SqlGenericRepository.ConnectionString"] = connectionString;
                throw;
            }
        }

        protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, T> handler)
        {
            var dbConnection = await GetConnectionAsync(this.ConnectionString);
            try
            {
                return handler(dbConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (MySqlException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            finally
            {
                dbConnection?.Close();
            }
        }
        //protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, IDbTransaction, T> connection)
        //{
        //    try
        //    {
        //        var dbConnection = await GetConnectionAsync(this.ConnectionString);
        //        var currentTransaction = this.CurrenctContext.Database.CurrentTransaction?.GetDbTransaction();
        //        return connection(dbConnection, currentTransaction);
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //}

        protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, Task<T>> handler)
        {
            var dbConnection = await GetConnectionAsync(this.ConnectionString);
            try
            {
                return await handler(dbConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (MySqlException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            finally
            {
                dbConnection?.Close();
            }
        }

        //protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> connection)
        //{
        //    try
        //    {
        //        var dbConnection = await GetConnectionAsync(this.ConnectionString);
        //        var currentTransaction = this.CurrenctContext.Database.CurrentTransaction?.GetDbTransaction();
        //        return await connection(dbConnection, currentTransaction);
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //}

        protected T WithConnection<T>(Func<IDbConnection, T> handler)
        {
            var dbConnection = GetConnection(this.ConnectionString);
            try
            {
                return handler(dbConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (MySqlException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            finally
            {
                dbConnection?.Close();
            }
        }
        protected T WithCurrentConnection<T>(Func<IDbConnection, T> handler)
        {
            try
            {
                var currentSqlConnection = (MySqlConnection)CurrentContext.Database.GetDbConnection();
                return handler(currentSqlConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (MySqlException ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
                ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
                throw;
            }
        }

        //protected T WithConnection<T>(Func<IDbConnection, IDbTransaction, T> connection)
        //{
        //    try
        //    {
        //        var dbConnection = GetConnection(this.ConnectionString);
        //        var currentTransaction = this.CurrenctContext.Database.CurrentTransaction?.GetDbTransaction();
        //        return connection(dbConnection, currentTransaction);
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute TimeoutException";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute SqlException";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data["SqlGenericRepository.Message-WithConnection"] = "Execute Exception";
        //        ex.Data["SqlGenericRepository.ConnectionString"] = ConnectionString;
        //        throw;
        //    }
        //}

        private Type GetTypeOfColumn(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName)
                || TableColumns.All(e => !columnName.Equals(e.GetColumnName(), StringComparison.OrdinalIgnoreCase)))
                return default;

            var columnType = TableColumns
                    .First(e => e.GetColumnName().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    .ClrType;

            return columnType;

        }

        private DapperExecution<TResult, TSQLBuilder> BuildBaseExecution<TResult, TSQLBuilder>
            (TSQLBuilder sqlBuilder, RequestFilterModel filterModel, bool buildSelectStatement = true, bool useDefaultTemplate = true)
            where TSQLBuilder : SqlBuilder
        {
            var sqlDynamicParameters = new DynamicParameters();
            var dapperExecution = new DapperExecution<TResult, TSQLBuilder>(this, sqlBuilder)
            {
                FilterModel = filterModel
            };

            var resultType = typeof(TResult);

            if (this.IsRestrictByOrganization)
            {
                if (this.UserIdentity?.OrganizationPaths?.Length > 0 || this.UserIdentity?.MngOrganizationPaths?.Length > 0)
                {
                    var organizeDataSql = "(t1.OrganizationPath IS NULL OR t1.OrganizationPath = ''";
                    for (int i = 0; i < this.UserIdentity.OrganizationPaths.Length; i++)
                    {
                        var organizationPath = this.UserIdentity.OrganizationPaths.ElementAt(i);
                        if (this.OrganizeDataLevel > 0)
                        {
                            organizeDataSql += @$" OR (t1.OrganizationPath NOT LIKE @organizationPath__{i} AND t1.OrganizationPath LIKE @organizationPath__{i})";
                        }
                        else
                        {
                            organizeDataSql += @$" OR t1.OrganizationPath LIKE @organizationPath__{i}";
                        }

                        sqlDynamicParameters.Add($"@organizationPath__{i}"
                            , $"{organizationPath}%"
                            , _typeMap[typeof(string)]);
                    }

                    for (int i = 0; i < this.UserIdentity.MngOrganizationPaths.Length; i++)
                    {
                        var organizationPath = this.UserIdentity.MngOrganizationPaths.ElementAt(i);
                        organizeDataSql += @$" OR t1.OrganizationPath LIKE @mngOrganizationPath__{i}";
                        sqlDynamicParameters.Add($"@mngOrganizationPath__{i}"
                            , $"{organizationPath}%"
                            , _typeMap[typeof(string)]);
                    }

                    if (this.OrganizeDataLevel > 0 && !string.IsNullOrEmpty(this.UserIdentity.UserName))
                    {
                        organizeDataSql += @$" OR t1.CreatedBy LIKE '{this.UserIdentity.UserName}'";
                    }

                    organizeDataSql += ")";
                    sqlBuilder.Where(organizeDataSql);
                }
            }

            if (!string.IsNullOrWhiteSpace(filterModel?.OrderBy))
            {
                if (!filterModel.RestrictOrderBy || this.IsBelongToPrimaryTable(filterModel.OrderBy))
                {
                    var orderByPrefix = this.IsBelongToPrimaryTable(filterModel.OrderBy) ? "t1." : string.Empty;

                    if (string.IsNullOrEmpty(filterModel.Dir) || filterModel.Dir.Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlBuilder.OrderBy($"{orderByPrefix}`{filterModel.OrderBy.ToUpperFirstLetter()}`");
                    }
                    else
                    {
                        sqlBuilder.OrderDescBy($"{orderByPrefix}`{filterModel.OrderBy.ToUpperFirstLetter()}`");
                    }
                }
            }

            if (resultType.IsClass
                && resultType != typeof(string)
                && !resultType.IsArray
                && buildSelectStatement)
            {
                var selectStatements = FindQueryColumns(resultType);
                if (selectStatements.Any()) sqlBuilder.Select(string.Join(",\n", selectStatements));
            }

            sqlBuilder.Where("t1.`IsDeleted` = FALSE");

            // Build dynamic query from filter expression string
            if (filterModel != null && !string.IsNullOrEmpty(filterModel.Filters))
            {
                for (int i = 0; i < filterModel.PropertyFilterModels.Count; i++)
                {
                    var propertyFilter = filterModel.PropertyFilterModels[i];

                    if (this.TableColumns
                            .All(e => !e.GetColumnName().Equals(propertyFilter.Field, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var predicateParameterName =
                        $"{sqlParameterPrefix}{propertyFilter.Field}{i}";

                    var alias = "t1.";

                    Type fieldType = GetTypeOfColumn(propertyFilter.Field);

                    propertyFilter.Field = $"{alias}{propertyFilter.Field.ToUpperFirstLetter()}";
                    bool isNullableType = Nullable.GetUnderlyingType(fieldType) != null;

                    propertyFilter.FilterValue = ConvertToSafeValue(propertyFilter.FilterValue.ToString(),
                        Nullable.GetUnderlyingType(fieldType) ?? fieldType);

                    if (sqlBuilder.AppendPredicate(ref propertyFilter, predicateParameterName, _typeMap[fieldType], isNullableType))
                    {
                        sqlDynamicParameters.Add(
                            $"@{predicateParameterName}",
                            propertyFilter.FilterValue,
                            _typeMap[fieldType]
                        );
                    }
                }
            }

            if (filterModel?.Paging == true)
            {
                sqlBuilder.Take(filterModel.Take)
                    .Skip(filterModel.Skip);
            }

            sqlBuilder.AddParameters(sqlDynamicParameters);

            if (useDefaultTemplate)
            {
                string sqlSelectingTemplate;
                if (filterModel != null
                    && filterModel.Paging)
                {
                    sqlSelectingTemplate =
                        $"SELECT\n/**select**/\nFROM" +
                        $"\n(\nSELECT\n/**innerselect**/\nFROM `{this.TableName}` AS t1\n/**innerjoin**//**leftjoin**//**rightjoin**//**where**//**innergroupby**//**groupby**//**having**//**orderby**//**take**//**skip**/) AS s" +
                        $"\nINNER JOIN\n{this.TableName} AS t1 ON t1.Id = s.Id /**innerjoin**//**leftjoin**//**rightjoin**//**where**//**groupby**//**having**//**orderby**/";

                    sqlBuilder.InsightSelect("DISTINCT t1.Id");
                }
                else
                {
                    sqlSelectingTemplate = $"SELECT\n/**select**/\nFROM `{this.TableName}` AS t1 /**innerjoin**//**leftjoin**//**rightjoin**//**where**//**groupby**//**having**//**orderby**//**take**//**skip**/";
                }

                // Add query template
                dapperExecution.ExecutionTemplate = sqlBuilder.AddTemplate(sqlSelectingTemplate);
            }

            return dapperExecution;
        }

        private HashSet<string> FindQueryColumns(Type resultType)
        {
            // Build sql select statement based on {TItem} type properties
            var selectingColumns = new HashSet<string>();
            selectingColumns.Add($"t1.`Id` AS `Id`");
            foreach (var propertyInfo in resultType.GetProperties())
            {
                if (propertyInfo.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)
                    || !propertyInfo.CanWrite
                    || !propertyInfo.PropertyType.IsPublic
                    || (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
                    || (!propertyInfo.PropertyType.IsValueType && propertyInfo.PropertyType != typeof(string))
                    || this.TableColumns.All(e =>
                        !e.Name.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase))) continue;

                var columnInfo = this.TableColumns.First(e =>
                    e.Name.Equals(propertyInfo.Name, StringComparison.OrdinalIgnoreCase));
                selectingColumns.Add($"t1.`{columnInfo.GetColumnName()}` AS `{columnInfo.Name}`");
            }

            return selectingColumns;
        }

        private object ConvertToSafeValue(string valueAsString, Type valueType)
        {
            if ("null".Equals(valueAsString, StringComparison.OrdinalIgnoreCase)
                || valueAsString == null)
            {
                return null;
            }

            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.Boolean:
                    return "true".Equals(valueAsString, StringComparison.OrdinalIgnoreCase) ? true : false;
                case TypeCode.DateTime:
                case TypeCode.DBNull:
                    if (!DateTime.TryParseExact(valueAsString,
                        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var safeTypedValue))
                    {
                        throw new FormatException("String was not recognized as a valid DateTime");
                    }

                    safeTypedValue = safeTypedValue.AddHours(7);

                    return new DateTime(safeTypedValue.Year, safeTypedValue.Month,
                        safeTypedValue.Day, 0, 0, 0);
                default:
                    return valueAsString;
            }
        }
        protected bool IsBelongToPrimaryTable(string columnName)
        {
            return this.TableColumns.Any(e =>
                    e.GetColumnName().Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }

        public void RestrictByOrganization(int level = 0)
        {
            this.IsRestrictByOrganization = true;
            this.OrganizeDataLevel = level;
        }

        public void RestrictByRole()
        {
            this.IsRestrictByRole = true;
        }

        #region Synchronous handler
        public TResult GetScalarByTemplate<TResult>(SqlBuilder.Template sqlBuilderTemplate)
        {
            return WithConnection((conn) =>
                conn.QueryFirstOrDefault<TResult>(sqlBuilderTemplate.RawSql, sqlBuilderTemplate.Parameters));
        }

        public TResult GetScalarByTemplate<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate,
            Func<TFirst, TSecond, TResult> response, string splitOnColumn = null)
        {
            return GetByTemplate(sqlBuilderTemplate, response, splitOnColumn)
                .FirstOrDefault();
        }

        public TResult GetScalarByTemplate<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate,
            Func<TFirst, TSecond, TThird, TResult> response, string splitOnColumn = null)
        {
            return GetByTemplate(sqlBuilderTemplate, response, splitOnColumn)
                .FirstOrDefault();
        }

        public TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TResult>(
            SqlBuilder.Template sqlBuilderTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TResult> response, string splitOnColumn = null)
        {
            return GetByTemplate(sqlBuilderTemplate, response, splitOnColumn)
                .FirstOrDefault();
        }

        public TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
            SqlBuilder.Template sqlBuilderTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> response, string splitOnColumn = null)
        {
            return GetByTemplate(sqlBuilderTemplate, response, splitOnColumn)
                .FirstOrDefault();
        }

        public TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
            SqlBuilder.Template sqlBuilderTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> response, string splitOnColumn = null)
        {
            return GetByTemplate(sqlBuilderTemplate, response, splitOnColumn)
                .FirstOrDefault();
        }

        public TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
            SqlBuilder.Template sqlBuilderTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> response,
            string splitOnColumn = null)
        {
            return GetByTemplate(sqlBuilderTemplate, response, splitOnColumn)
                .FirstOrDefault();
        }
        public IEnumerable<TResult> GetByTemplate<TResult>(SqlBuilder.Template sqlTemplate)
        {
            return WithConnection((conn) => conn.Query<TResult>(sqlTemplate.RawSql, sqlTemplate.Parameters));
        }

        public IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TResult>(SqlBuilder.Template sqlTemplate,
            Func<TFirst, TSecond, TResult> response, string splitOnColumn = null)
        {
            return WithConnection((conn) => conn.Query(sqlTemplate.RawSql, response,
                param: sqlTemplate.Parameters, splitOn: splitOnColumn));
        }

        public IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlTemplate,
            Func<TFirst, TSecond, TThird, TResult> response, string splitOnColumn = null)
        {
            return WithConnection((conn) => conn.Query(sqlTemplate.RawSql, response,
                param: sqlTemplate.Parameters, splitOn: splitOnColumn));
        }

        public IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TResult>(
            SqlBuilder.Template sqlTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TResult> response, string splitOnColumn = null)
        {
            return WithConnection((conn) => conn.Query(sqlTemplate.RawSql,
                response, param: sqlTemplate.Parameters, splitOn: splitOnColumn));
        }

        public IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(
            SqlBuilder.Template sqlTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> response, string splitOnColumn = null)
        {
            return WithConnection((conn) =>
                conn.Query(sqlTemplate.RawSql, response,
                    param: sqlTemplate.Parameters, splitOn: splitOnColumn));
        }

        public IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(
            SqlBuilder.Template sqlTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> response, string splitOnColumn = null)
        {
            return WithConnection((conn) =>
                conn.Query(sqlTemplate.RawSql, response,
                    param: sqlTemplate.Parameters, splitOn: splitOnColumn));
        }

        public IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(
            SqlBuilder.Template sqlTemplate,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> response,
            string splitOnColumn = null)
        {
            return WithConnection((conn) =>
                conn.Query(sqlTemplate.RawSql,
                    response, param: sqlTemplate.Parameters, splitOn: splitOnColumn));
        }

        public DapperExecution<TResult> BuildByTemplateWithoutSelect<TResult>(RequestFilterModel filterModel = null, string from = null)
        {
            var defaultSqlBuilder = new SqlBuilder(this.TableName);
            return this.BuildBaseExecution<TResult, SqlBuilder>(defaultSqlBuilder, filterModel, false);
        }

        public DapperExecution<TResult, TSQLBuilder> BuildByTemplateWithoutSelect<TResult, TSQLBuilder>
            (RequestFilterModel filterModel = null, string from = null) where TSQLBuilder : SqlBuilder, new()
        {
            var sqlBuilderGenericContructor = typeof(TSQLBuilder);
            var sqlBuilder = (TSQLBuilder)Activator.CreateInstance(sqlBuilderGenericContructor, from ?? this.TableName);
            return this.BuildBaseExecution<TResult, TSQLBuilder>(sqlBuilder, filterModel, false);
        }

        public DapperExecution<TResult> BuildByTemplate<TResult>(RequestFilterModel filterModel = null)
        {
            var defaultSqlBuilder = new SqlBuilder(this.TableName);
            return this.BuildBaseExecution<TResult, SqlBuilder>(defaultSqlBuilder, filterModel);
        }

        public DapperExecution<TResult, TSQLBuilder> BuildByTemplate<TResult, TSQLBuilder>
            (RequestFilterModel filterModel = null) where TSQLBuilder : SqlBuilder, new()
        {
            var sqlBuilderGenericContructor = typeof(TSQLBuilder);
            var sqlBuilder = new TSQLBuilder();
            try
            {
                sqlBuilder = (TSQLBuilder)Activator.CreateInstance(sqlBuilderGenericContructor, this.TableName);
            }
            catch (Exception)
            {
            }
            return this.BuildBaseExecution<TResult, TSQLBuilder>(sqlBuilder, filterModel);
        }


        public DapperExecution<TResult, TSQLBuilder> Build<TResult, TSQLBuilder>(RequestFilterModel requestFilterModel = null)
            where TSQLBuilder : SqlBuilder, new()
        {
            var sqlBuilderType = typeof(TSQLBuilder);
            var sqlBuilder = (TSQLBuilder)Activator.CreateInstance(sqlBuilderType, this.TableName);
            return this.BuildBaseExecution<TResult, TSQLBuilder>(sqlBuilder, requestFilterModel, useDefaultTemplate: false);
        }

        public DapperExecution<TResult> Build<TResult>(RequestFilterModel requestFilterModel = null)
        {
            var defaultSqlBuilder = new SqlBuilder(this.TableName);
            return this.BuildBaseExecution<TResult, SqlBuilder>(defaultSqlBuilder, requestFilterModel, useDefaultTemplate: false);
        }

        public IEnumerable<TResult> GetByTemplate<TResult>(SqlBuilder.Template sqlBuilderTemplate, Type[] types, Func<object[], TResult> map, string splitOnColumn = "Id")
        {
            return WithConnection((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, types, map, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }

        #endregion

        #region Asynchronous handler
        public Task<TResult> GetScalarByTemplateAsync<TResult>(SqlBuilder.Template sqlBuilderTemplate)
        {
            return WithConnectionAsync((conn) =>
                conn.QueryFirstOrDefault<TResult>(
                    sqlBuilderTemplate.RawSql, sqlBuilderTemplate.Parameters));
        }

        public Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn)
                    .FirstOrDefault());
        }

        public Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn)
                    .FirstOrDefault());
        }

        public Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn)
                    .FirstOrDefault());
        }

        public Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn)
                    .FirstOrDefault());
        }

        public Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn)
                    .FirstOrDefault());
        }

        public Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn)
                    .FirstOrDefault());
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TResult>(SqlBuilder.Template sqlTemplate)
        {
            return WithConnectionAsync((conn) => conn.Query<TResult>(sqlTemplate.RawSql, sqlTemplate.Parameters));
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }

        public Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resp, string splitOnColumn = null)
        {
            return WithConnectionAsync((conn) =>
                conn.Query(sqlBuilderTemplate.RawSql, resp, sqlBuilderTemplate.Parameters, splitOn: splitOnColumn));
        }
        #endregion
    }
}