using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using ApplicationUserIdentity.API.Models;

namespace ApplicationUserIdentity.API.Infrastructure.Seed
{
    public static class ApplicationUserContextRuntimeSeed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUserClass>().HasData(ApplicationUserClass.Seeds());
        }

        private static IEnumerable<TEntity> ReadDataFromCSV<TEntity>(string fileName) where TEntity : new()
        {
            var result = new List<TEntity>();
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            var pathOfFile = Path.Combine(dirPath, "Seed", "Data", fileName + ".csv");

            using (var reader = new StreamReader(pathOfFile))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    csv.ValidateHeader(typeof(TEntity));
                    var records = csv.GetRecords<TEntity>();
                    foreach (var record in records)
                    {
                        result.Add(record);
                    }
                }
            }

            return result;
        }
    }
}
