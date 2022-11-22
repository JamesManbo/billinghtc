using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Global.Models.Filter
{
    public class RequestFilterModel
    {
        public static class RequestFilterOperator
        {
            public static string Equal = "eq";
            public static string NotEqual = "neq";
            public static string IsNull = "isnull";
            public static string IsNotNull = "isnotnull";
            public static string GreaterThan = "gt";
            public static string GreaterThanOrEqual = "gte";
            public static string LessThan = "lt";
            public static string LessThanOrEqual = "lte";
            public static string StartsWith = "startswith";
            public static string EndsWith = "endswith";
            public static string Contains = "contains";
            public static string DoesNotContain = "doesnotcontain";
            public static string IsEmpty = "isempty";
            public static string IsNotEmpty = "isnotempty";
        }

        private string _orderBy;
        private string _dir;
        private int _skip;
        private int _take;

        public int Skip
        {
            get => _skip < 0 ? 0 : _skip;
            set => _skip = value;
        }

        public int Take
        {
            get => _take <= 0 ? 10 : _take;
            set => _take = value;
        }

        public string Filters { get; set; }

        public virtual string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "CreatedDate";

            set => _orderBy = value;
        }

        /// <summary>
        /// Order direction (asc|desc)
        /// </summary>
        public string Dir
        {
            get => !string.IsNullOrWhiteSpace(_dir) ? _dir : "DESC";

            set => _dir = value;
        }

        /// <summary>
        /// Selection type
        /// </summary>
        public RequestType Type { get; set; } = RequestType.Grid;
        public bool Paging { get; set; } = true;

        /// <summary>
        /// TRUE: Không cho phép sắp xếp với các trường đẩy lên ko tồn tại trong bảng t1
        /// </summary>
        public bool RestrictOrderBy { get; set; }
        public string Keywords { get; set; } = string.Empty;
        private List<PropertyFilterModel> _propertyFilterModels;
        public IReadOnlyList<PropertyFilterModel> PropertyFilterModels
        {
            get
            {
                if (_propertyFilterModels != null && _propertyFilterModels.Any())
                {
                    return _propertyFilterModels;
                }

                if (!string.IsNullOrWhiteSpace(this.Filters))
                {
                    DissectFilters();
                }
                return _propertyFilterModels;
            }
        }

        public RequestFilterModel()
        {
            _propertyFilterModels = new List<PropertyFilterModel>();
        }

        private void DissectFilters()
        {
            var conditions = this.Filters.Split('|');
            foreach (var condition in conditions)
            {
                if (string.IsNullOrEmpty(condition)
                    || !condition.Contains("::", StringComparison.OrdinalIgnoreCase)) continue;

                var propertyFilter = new PropertyFilterModel();
                var conditionPaths = condition.Split("::");
                propertyFilter.Field = conditionPaths[0].Trim().ToUpperFirstLetter();
                propertyFilter.FilterValue = conditionPaths[1].Trim();
                propertyFilter.Operator = conditionPaths.Length > 2 ? conditionPaths[2].Trim() : RequestFilterOperator.Equal;
                _propertyFilterModels.Add(propertyFilter);
            }
        }

        public object Get(string field, string @operator = "")
        {
            if (this.PropertyFilterModels.Count == 0 || string.IsNullOrWhiteSpace(field)) return null;

            return this.PropertyFilterModels.FirstOrDefault(
                p => p.Field.Equals(field, StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrWhiteSpace(@operator) || @operator.Equals(p.Operator, StringComparison.OrdinalIgnoreCase)))
                    ?.FilterValue;

        }

        public PropertyFilterModel GetProperty(string field)
        {
            if (this.PropertyFilterModels.Count == 0 || string.IsNullOrWhiteSpace(field)) return null;

            return this.PropertyFilterModels.FirstOrDefault(
                p => p.Field.Equals(field, StringComparison.OrdinalIgnoreCase));
        }
        public PropertyFilterModel[] GetProperties(string field)
        {
            if (this.PropertyFilterModels.Count == 0 || string.IsNullOrWhiteSpace(field)) return null;

            return this.PropertyFilterModels.Where(
                p => p.Field.Equals(field, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        public TResult Get<TResult>(string field, string @operator = "")
        {
            var result = Get(field, @operator);
            try
            {
                Type convertedType = Nullable.GetUnderlyingType(typeof(TResult)) ?? typeof(TResult);
                var convertedObject = Convert.ChangeType(result, convertedType);
                return (TResult)convertedObject;
            }
            catch (InvalidCastException)
            {
                return default;
            }
            catch (FormatException)
            {
                return default;
            }
            catch (ArgumentNullException)
            {
                return default;
            }
            catch (OverflowException)
            {
                return default;
            }
        }
        public bool Any(string field, string @operator = "")
        {
            if (this.PropertyFilterModels.Count == 0 || string.IsNullOrWhiteSpace(field)) return false;
            return this.PropertyFilterModels
                .Any(p => field.Equals(p.Field, StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrWhiteSpace(@operator) || @operator.Equals(p.Operator, StringComparison.OrdinalIgnoreCase)));

        }

        public RequestFilterModel ClearFilter()
        {
            this.Filters = string.Empty;
            this._propertyFilterModels?.Clear();
            return this;
        }

        public RequestFilterModel ClearOrder()
        {
            this._orderBy = "";
            return this;
        }

        public RequestFilterModel NoPaging()
        {
            this.Paging = false;
            return this;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"filter_model_");
            stringBuilder.Append($"{this.Type}{Take}{Skip}{Paging}{OrderBy}{Dir}{Keywords}_{Filters}");

            return stringBuilder.ToString();
        }
    }

    public enum RequestType
    {
        Grid = 1,
        Selection = 2,
        Hierarchical = 3,
        SimpleAll = 4,
        Autocomplete = 5,
        AutocompleteSimple = 6
    }

    internal static class Extensions
    {
        public static string ToUpperFirstLetter(this string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return string.Empty;

            var chars = src
                .Select((c, i) => i == 0 ? c.ToString().ToUpper() : c.ToString())
                .ToArray();

            return string.Join("", chars);
        }
    }

    public class ReportFilterBase : RequestFilterModel
    {
        public string ContractCode { get; set; }
        public string CustomerCode { get; set; }
        public DateTime? TimelineSignedStart { get; set; }
        public DateTime? TimelineSignedEnd { get; set; }
        public int ServiceId { get; set; }
        public int ProjectId { get; set; }
        public int MarketAreaId { get; set; }
        public int? ReportYear { get; set; }
        public string ContractorFullName { get; set; }
        public DateTime? TimelineEffectiveStart { get; set; }
        public DateTime? TimelineEffectiveEnd { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}