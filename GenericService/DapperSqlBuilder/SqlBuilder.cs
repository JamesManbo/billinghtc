using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;

namespace GenericRepository.DapperSqlBuilder
{
    public class SqlBuilder
    {
        public readonly string TableName;
        public int TakeNumber { get; set; }
        public int SkipNumber { get; set; }
        public Dictionary<string, string> _innerJoinTables = new Dictionary<string, string>();
        private readonly Dictionary<string, Clauses> _data = new Dictionary<string, Clauses>();
        private int _seq;

        private class Clause
        {
            public string Sql { get; set; }
            public object Parameters { get; set; }
            public bool IsInclusive { get; set; }
        }

        private class Clauses : List<Clause>
        {
            private readonly string _joiner, _prefix, _postfix;

            public Clauses(string joiner, string prefix = "", string postfix = "")
            {
                _joiner = joiner;
                _prefix = prefix;
                _postfix = postfix;
            }

            public string ResolveClauses(DynamicParameters p)
            {
                foreach (var item in this)
                {
                    p.AddDynamicParams(item.Parameters);
                }

                return this.Any(a => a.IsInclusive)
                    ? _prefix +
                      string.Join(_joiner,
                          this.Where(a => !a.IsInclusive)
                              .Select(c => c.Sql)
                              .Union(new[]
                              {
                                  " ( " +
                                  string.Join(" OR ", this.Where(a => a.IsInclusive).Select(c => c.Sql).ToArray()) +
                                  " ) "
                              }).ToArray()) + _postfix
                    : _prefix + string.Join(_joiner, this.Select(c => c.Sql).ToArray()) + _postfix;
            }
        }

        public class Template
        {
            private readonly string _sql;
            private readonly SqlBuilder _builder;
            private readonly object _initParams;
            private int _dataSeq = -1; // Unresolved

            public Template(SqlBuilder builder, string sql, dynamic parameters)
            {
                _initParams = parameters;
                _sql = sql;
                _builder = builder;
            }

            private static readonly Regex _regex = new Regex(@"\/\*\*.+?\*\*\/",
                RegexOptions.Compiled | RegexOptions.Multiline);

            public void ResolveInnerJoinClauses(string alias)
            {
                if (_builder._data.TryGetValue("select", out Clauses clauses))
                {
                    if (clauses != null)
                    {
                        foreach (var clause in clauses)
                        {
                            if (clause.Sql != null
                                && clause.Sql.StartsWith(alias, StringComparison.OrdinalIgnoreCase))
                            {
                                _builder.InsightSelect(clause.Sql, clause.Parameters);
                                clause.Sql = $"t1{clause.Sql.Substring(alias.Length)}";
                            }
                        }
                    }
                }
            }

            private void ResolveSql()
            {
                //if (_builder._innerJoinTables.Any())
                //{
                //    foreach (var tableAlias in _builder._innerJoinTables)
                //    {
                //        ResolveInnerJoinClauses(tableAlias.Key);
                //    }
                //}

                if (_dataSeq != _builder._seq)
                {
                    var p = new DynamicParameters(_initParams);

                    rawSql = _sql;

                    foreach (var pair in _builder._data)
                    {
                        rawSql = rawSql.Replace("/**" + pair.Key + "**/", pair.Value.ResolveClauses(p));
                    }

                    parameters = p;

                    // replace all that is left with empty
                    rawSql = _regex.Replace(rawSql, "");

                    _dataSeq = _builder._seq;
                }
            }

            private string rawSql;
            private object parameters;

            public string RawSql
            {
                get
                {
                    ResolveSql();
                    return rawSql;
                }
            }

            public object Parameters
            {
                get
                {
                    ResolveSql();
                    return parameters;
                }
            }
        }

        public Template AddTemplate(string sql, dynamic parameters = null) =>
            new Template(this, sql, parameters);

        protected SqlBuilder AddClause(string name, string sql, object parameters, string joiner, string prefix = "",
            string postfix = "", bool isInclusive = false)
        {
            if (!_data.TryGetValue(name, out Clauses clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                _data[name] = clauses;
            }

            clauses.Add(new Clause { Sql = sql, Parameters = parameters, IsInclusive = isInclusive });
            _seq++;
            return this;
        }

        public SqlBuilder(string tableName)
        {
            TableName = tableName;
        }
        protected SqlBuilder()
        {

        }

        public bool HasGroupByStatement()
        {
            return _data.ContainsKey("groupby");
        }

        public string GetGroupByField()
        {
            if (!HasGroupByStatement()) return string.Empty;

            if (_data.TryGetValue("groupby", out var groupByClauses))
            {
                var groupByClause = groupByClauses.First();
                return groupByClause.Sql;
            }

            return string.Empty;
        }

        public SqlBuilder Intersect(string sql, dynamic parameters = null) =>
            AddClause("intersect", sql, parameters, "\nINTERSECT\n ", "\n ", "\n", false);

        public SqlBuilder InnerJoin(string sql, dynamic parameters = null)
        {
            var statementParts = Regex.Split(sql, @"\s+");
            if ("AS".Equals(statementParts[1], StringComparison.OrdinalIgnoreCase))
            {
                _innerJoinTables.Add(statementParts[2], statementParts[0]);
            }
            else if ("ON".Equals(statementParts[1], StringComparison.OrdinalIgnoreCase))
            {
                _innerJoinTables.Add(statementParts[0], statementParts[0]);
            }
            else
            {
                _innerJoinTables.Add(statementParts[1], statementParts[0]);
            }

            return AddClause("innerjoin", sql, parameters, "\nINNER JOIN ", "\nINNER JOIN ", "\n", false);
        }

        public SqlBuilder InsightJoin(string sql, dynamic parameters = null)
        {
            return AddClause("insightjoin", sql, parameters, "\nINNER JOIN ", "\nINNER JOIN ", "\n", false);
        }

        public SqlBuilder InsightLeftJoin(string sql, dynamic parameters = null)
        {
            return AddClause("insightleftjoin", sql, parameters, "\nLEFT JOIN ", "\nLEFT JOIN ", "\n", false);
        }

        public SqlBuilder InsightRightJoin(string sql, dynamic parameters = null)
        {
            return AddClause("insightrightjoin", sql, parameters, "\nRIGHT JOIN ", "\nRIGHT JOIN ", "\n", false);
        }

        public SqlBuilder LeftJoin(string sql, dynamic parameters = null) =>
            AddClause("leftjoin", sql, parameters, "\nLEFT JOIN ", "\nLEFT JOIN ", "\n", false);

        public SqlBuilder RightJoin(string sql, dynamic parameters = null) =>
            AddClause("rightjoin", sql, parameters, "\nRIGHT JOIN ", "\nRIGHT JOIN ", "\n", false);

        public SqlBuilder Where(string sql, dynamic parameters = null)
        {
            return AddClause("where", sql, parameters, " AND ", "\nWHERE ", "\n", false);
        }
        public SqlBuilder OrWhere(string sql, dynamic parameters = null)
        {
            return AddClause("where", sql, parameters, " OR ", "\nWHERE ", "\n", true);
        }

        public SqlBuilder InsightWhere(string sql, dynamic parameters = null) =>
            AddClause("innerwhere", sql, parameters, " AND ", "\nWHERE ", "\n", false);

        public SqlBuilder InsightOrWhere(string sql, dynamic parameters = null) =>
            AddClause("innerwhere", sql, parameters, " OR ", "\nWHERE ", "\n", true);

        public SqlBuilder OrderBy(string sql, dynamic parameters = null) =>
            AddClause("orderby", sql, parameters, " , ", "ORDER BY ", "\n", false);

        public SqlBuilder OrderDescBy(string sql, dynamic parameters = null) =>
            AddClause("orderby", sql, parameters, " , ", "ORDER BY ", " DESC\n", false);

        public SqlBuilder Take(int takeRecords = 10)
        {
            this.TakeNumber = takeRecords;
            return AddClause("take", "@takeRecords", new { takeRecords }, "\nLIMIT ", "\nLIMIT ", "\n", false);
        }

        public SqlBuilder Skip(int skipRecords = 0)
        {
            this.SkipNumber = skipRecords;
            return AddClause("skip", "@skipRecords", new { skipRecords }, "\nOFFSET ", "\nOFFSET ", "\n", false);
        }
            

        public SqlBuilder Select(string sql, dynamic parameters = null)
        {
            return AddClause("select", sql, parameters, " , ", "", "\n", false);
        }
        public SqlBuilder InsightSelect(string sql, dynamic parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql)) return null;

            var result = AddClause("innerselect", sql, parameters, " , ", "", "\n", false);

            //var existAliasKeywordRx = new Regex(@"\bAS\b", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //if (existAliasKeywordRx.IsMatch(sql))
            //{
            //    var outerSelect = new Regex(@"(?<=\bAS\s+)\w+$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline).Match(sql).Value;
            //    result.AddClause("select", $"s.{outerSelect}", parameters, " , ", "", "\n", false);
            //}
            //else
            //{
            //    result.AddClause("select", sql, parameters, " , ", "", "\n", false);
            //}

            return result;
        }

        public SqlBuilder AddParameters(dynamic parameters) =>
            AddClause("--parameters", "", parameters, "", "", "", false);

        public SqlBuilder Join(string sql, dynamic parameters = null) =>
            AddClause("join", sql, parameters, "\nJOIN ", "\nJOIN ", "\n", false);

        public SqlBuilder GroupBy(string sql, dynamic parameters = null) {
            AddClause("groupby", sql, parameters, " , ", "\nGROUP BY ", "\n", false);
            return this;
        }

        public SqlBuilder InsightGroupBy(string sql, dynamic parameters = null) {
            if (_data.TryGetValue("innergroupby", out var groupByClauses))
            {
                _data.Remove("innergroupby");
            }

            return AddClause("innergroupby", sql, parameters, " , ", "\nGROUP BY ", "\n", false);
        }

        public SqlBuilder InsightOrderBy(string sql, dynamic parameters = null, bool dir = true) {
            return AddClause("innerorderby", sql, parameters, " , ", "\nORDER BY ", dir ? "\n" : " DESC\n", false);
        }

        public SqlBuilder InsightOrderDescBy(string sql, dynamic parameters = null) {
            return AddClause("innerorderby", sql, parameters, " , ", "\nORDER BY ", " DESC\n", false);
        }

        public SqlBuilder Having(string sql, dynamic parameters = null) =>
            AddClause("having", sql, parameters, "\nAND ", "HAVING ", "\n", false);
    }
}