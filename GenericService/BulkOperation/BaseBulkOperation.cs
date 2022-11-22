using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FastMember;
using System.Reflection;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using GenericRepository.Models;
using CsvHelper.TypeConversion;

namespace GenericRepository.BulkOperation
{
    public abstract class BaseBulkOperation<TEntity> : IBulkOperation<TEntity>
    {
        public readonly string ConnectionString;
        public readonly string TableName;
        public readonly IEnumerable<IProperty> TableColumns;
        private readonly TypeAccessor _tableColumnAccessor;
        private const int ExecutionTimeout = 10 * 1000;
        protected readonly DbContext DbContext;

        protected readonly string[] ExcludeBulkInsertColumns = new[]
        {
            "DomainEvents"
        };

        protected BaseBulkOperation(DbContext context)
        {
            DbContext = context;
            ConnectionString = context.Database.GetDbConnection().ConnectionString;
            TableName = context.Model.FindEntityType(typeof(TEntity)).GetTableName();
            TableColumns =
                context.Model.FindEntityType(typeof(TEntity)).GetProperties();
        }

        //private bool IsAllowBulkInsert(PropertyInfo property)
        //{
        //    object[] attrs = property.GetCustomAttributes(true);
        //    foreach (object attr in attrs)
        //    {
        //        var ignoreBulkInsertAttr = attr as IgnoreBulkInsertAttribute;
        //        return ignoreBulkInsertAttr == null;
        //    }
        //    return true;
        //}

        public async Task<int> InsertBulk<TModel>(List<TModel> entities)
        {
            if (entities.Count == 0) return await Task.FromResult(0);

            var temporaryFilePath =
                $"{TableName}-{Guid.NewGuid():N}.csv";
            var headers = new string[] { };
            using (var writer = new StreamWriter(temporaryFilePath, true, new UTF8Encoding(true)))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreReferences = true,
                Quote = '"',
                Escape = '"',
                NewLine = Environment.NewLine,
                Delimiter = ","
            }))
            {
                var options = new TypeConverterOptions { Formats = new[] { "yyyy-MM-dd HH:mm:ss" } };
                csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                csv.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(options);

                csv.Context.TypeConverterCache.AddConverter<bool>(new CsvBooleanConverter());
                csv.Context.TypeConverterCache.AddConverter<Enum>(new CsvEnumConverter());

                csv.Context.TypeConverterOptionsCache.GetOptions<int?>().NullValues.Add("NULL");
                csv.Context.TypeConverterOptionsCache.GetOptions<decimal?>().NullValues.Add("NULL");
                csv.Context.TypeConverterOptionsCache.GetOptions<float?>().NullValues.Add("NULL");
                csv.Context.TypeConverterOptionsCache.GetOptions<long?>().NullValues.Add("NULL");
                csv.Context.TypeConverterOptionsCache.GetOptions<DateTime?>().NullValues.Add("NULL");
                csv.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("NULL");
                csv.Context.TypeConverterOptionsCache.GetOptions<bool?>().NullValues.Add("NULL");

                csv.WriteRecords(entities);
                headers = csv.Context.Writer.HeaderRecord;
            }

            return await InsertBulkInFile(temporaryFilePath, headers);
        }

        public async Task<int> InsertBulkInFile(string filePath, string[] headers)
        {
            int effectRows;
            try
            {
                var bl = new MySqlBulkLoader((MySqlConnection)DbContext.Database.GetDbConnection())
                {
                    Local = true,
                    TableName = TableName,
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    FieldQuotationOptional = true,
                    FieldTerminator = ",",
                    LineTerminator = Environment.NewLine,
                    FileName = filePath,
                    NumberOfLinesToSkip = 1, // Bỏ qua header (tên các cột của bảng)
                    ConflictOption = MySqlBulkLoaderConflictOption.Replace,
                    CharacterSet = "utf8"
                };

                bl.Columns.Clear();

                foreach (var col in headers
                    .Where(c => !this.ExcludeBulkInsertColumns.Contains(c)))
                {
                    bl.Columns.Add(col);
                }

                effectRows = await bl.LoadAsync();
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }

            return effectRows;
        }
        
        public string GetPropertyValues(object obj)
        {
            var result = string.Empty;
            for (int i = 0; i < TableColumns.Count(); i++)
            {
                if (i > 0)
                {
                    result += ",";
                }

                result += _tableColumnAccessor[obj, TableColumns.ElementAt(i).Name];
            }

            return result;
        }

    }
    public class DefaultMap<TModel> : ClassMap<TModel>
    {
        public DefaultMap()
        {
        }
    }

    public class CsvBooleanConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
            {
                return "NULL";
            }

            var boolValue = (bool)value;
            return boolValue ? "1" : "0";
        }
    }

    public class CsvEnumConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == null || memberMapData == null) return default;

            var type = memberMapData.Member.GetType();
            if (!type.IsEnum) return default;

            var underlyingType = Enum.GetUnderlyingType(type);

            object enumValue;
            try
            {
                enumValue = Convert.ChangeType(text, underlyingType);
            }
            catch (Exception)
            {
                return default;
            }

            if (!Enum.IsDefined(type, enumValue))
                throw new FormatException($"Invalid '{type.Name}' value '{text}'");

            return Enum.ToObject(type, enumValue);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null)
                return "NULL";

            var type = value.GetType();
            if (!type.IsEnum)
                return "NULL";

            var underlyingType = Enum.GetUnderlyingType(value.GetType());

            try
            {
                return Convert.ChangeType(value, underlyingType).ToString();
            }
            catch (Exception)
            {
                return "NULL";
            }
        }
    }
}