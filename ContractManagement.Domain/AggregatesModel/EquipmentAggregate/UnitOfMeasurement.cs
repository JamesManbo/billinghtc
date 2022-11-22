using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.EquipmentAggregate
{
    [Table("UnitOfMeasurement")]
    public class UnitOfMeasurement : Entity
    {
        public static UnitOfMeasurement DefaultUnit = new UnitOfMeasurement(1, "Cái", "Cái", true, UnitOfMeasurementType.CountingUnit, true);
        public static UnitOfMeasurement Meter = new UnitOfMeasurement(2, "m", "Mét", true, UnitOfMeasurementType.Distance, true);
        public static UnitOfMeasurement Kilometer = new UnitOfMeasurement(9, "km", "Kilômét", true, UnitOfMeasurementType.Distance, false, 1000);
        public static UnitOfMeasurement Hour = new UnitOfMeasurement(3, "h", "Giờ", true, UnitOfMeasurementType.Timer, false, 60 * 60);
        public static UnitOfMeasurement Minute = new UnitOfMeasurement(4, "m", "Phút", true, UnitOfMeasurementType.Timer, false, 60);
        public static UnitOfMeasurement Second = new UnitOfMeasurement(5, "s", "Giây", true, UnitOfMeasurementType.Timer, true, 1);
        public static UnitOfMeasurement Kbps = new UnitOfMeasurement(6, "Kbps", "Kilobit/s", true, UnitOfMeasurementType.Bandwidth, true, 1);
        public static UnitOfMeasurement Mbps = new UnitOfMeasurement(7, "Mbps", "Megabit/s", true, UnitOfMeasurementType.Bandwidth, false, 1024);
        public static UnitOfMeasurement Gbps = new UnitOfMeasurement(8, "Gbps", "Gigabit/s", true, UnitOfMeasurementType.Bandwidth, false, 1048576);

        public static UnitOfMeasurement Kilobyte = new UnitOfMeasurement(9, "kB", "Kilobyte", true, UnitOfMeasurementType.Bytes, true, 1);
        public static UnitOfMeasurement Megabyte = new UnitOfMeasurement(10, "MB", "Megabyte", true, UnitOfMeasurementType.Bytes, false, 1024);
        public static UnitOfMeasurement GigaByte = new UnitOfMeasurement(11, "GB", "Gigabyte", true, UnitOfMeasurementType.Bytes, false, 1048576);
        public static UnitOfMeasurement TeraByte = new UnitOfMeasurement(12, "TB", "Terabyte", true, UnitOfMeasurementType.Bytes, false, 1073741824);
        public static UnitOfMeasurement PetaByte = new UnitOfMeasurement(13, "PB", "PetaByte", true, UnitOfMeasurementType.Bytes, false, 1099511627776);

        public UnitOfMeasurement()
        {
        }

        //public T DefaultConvertTo<T>(T value, string fromUnitCode) where T : struct,
        //    IComparable,
        //    IComparable<T>,
        //    IConvertible,
        //    IEquatable<T>,
        //    IFormattable
        //{
        //    if (value.CompareTo(0) == 0) return (T) Convert.ChangeType(0, typeof(T));


        //}

        public static IEnumerable<UnitOfMeasurement> Seeds() => new[]
            { DefaultUnit, Meter, Hour, Minute, Second, Kbps, Mbps, Gbps, Kilobyte, Megabyte, GigaByte, TeraByte, PetaByte };

        public UnitOfMeasurement(int id, string label, string description, bool isRequired, UnitOfMeasurementType type, bool isBaseOfType = false, decimal conversionRate = 1)
        {
            Id = id;
            Label = label;
            Description = description;
            IsRequired = isRequired;
            Type = type;
            IsBaseOfType = isBaseOfType;
            ConversionRate = conversionRate;
        }

        public string Label { get; set; }
        public string Description { get; set; }
        public UnitOfMeasurementType Type { get; set; }
        public bool IsRequired { get; set; } = true;
        public bool IsBaseOfType { get; set; }
        public decimal ConversionRate { get; set; }

        public static UnitOfMeasurement Find(Func<UnitOfMeasurement, bool> predicate)
        {
            return Seeds().FirstOrDefault(predicate);
        }

        public decimal ConvertToBase(decimal? value)
        {
            if (!value.HasValue) return 0;
            return this.ConversionRate * value.Value;
        }

        public decimal ConvertTo(decimal value, string desUnitCode)
        {
            var destinationUnit = Seeds().FirstOrDefault(c => desUnitCode.Equals(c.Label, StringComparison.OrdinalIgnoreCase));

            if (destinationUnit == null) return 0;

            return value * this.ConversionRate / destinationUnit.ConversionRate;
        }
        public decimal ConvertTo(decimal value, int desUnitId)
        {
            var destinationUnit = Seeds().FirstOrDefault(c => desUnitId == c.Id);

            if (destinationUnit == null) return 0;

            return value * this.ConversionRate / destinationUnit.ConversionRate;
        }
    }
}
