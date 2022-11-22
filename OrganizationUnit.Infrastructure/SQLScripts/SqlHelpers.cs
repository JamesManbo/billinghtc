using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationUnit.Infrastructure.SqlScripts
{
    public static class SqlHelpers
    {
        private static string ReadStoredProcedureFromFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(
                    $"Value of {nameof(fileName)} must be supplied to {nameof(ReadStoredProcedureFromFile)}");
            }

            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var pathOfScripts = Path.Combine(dirPath, "SQLScripts", "StoredProcedures", fileName + ".sql");

            if (!File.Exists(pathOfScripts))
            {
                throw new ArgumentException($"The file {fileName} does not exist(path: {pathOfScripts}");
            }

            return File.ReadAllText(pathOfScripts);
        }

        public static void AlterStoredProcedure(this MigrationBuilder modelBuilder, string fileName)
        {
            modelBuilder.Sql(ReadStoredProcedureFromFile(fileName));
        }
    }
}