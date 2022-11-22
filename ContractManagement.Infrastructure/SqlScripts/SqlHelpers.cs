using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractManagement.Infrastructure.SqlScripts
{
    public static class SqlHelpers
    {

        private static string ReadSqlFile(string folder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(
                    $"Value of {nameof(fileName)} must be supplied to {nameof(ReadStoredProcedureFromFile)}");
            }

            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var pathOfScripts = Path.Combine(dirPath, "SqlScripts", folder, fileName + ".sql");

            if (!File.Exists(pathOfScripts))
            {
                throw new ArgumentException($"The file {fileName} does not exist(path: {pathOfScripts}");
            }

            return File.ReadAllText(pathOfScripts);
        }

        private static string ReadStoredProcedureFromFile(string fileName)
        {
            return ReadSqlFile("StoredProcedures", fileName);
        }
        private static string ReadStoredFunctionFromFile(string fileName)
        {
            return ReadSqlFile("Functions", fileName);
        }

        public static void AlterStoredProcedure(this MigrationBuilder modelBuilder, string fileName)
        {
            modelBuilder.Sql(ReadStoredProcedureFromFile(fileName));
        }
        public static void AlterFunction(this MigrationBuilder modelBuilder, string fileName)
        {
            modelBuilder.Sql(ReadStoredFunctionFromFile(fileName));
        }
    }
}