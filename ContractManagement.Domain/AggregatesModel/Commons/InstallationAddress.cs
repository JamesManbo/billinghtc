using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.Commons
{    
    public class InstallationAddress : ValueObject
    {
        [StringLength(256)] public string Building { get; set; } = "";
        [StringLength(256)] public string Floor { get; set; } = "";
        [StringLength(256)] public string RoomNumber { get; set; } = "";
        [StringLength(256)] public string Street { get; set; } = "";
        [StringLength(256)] public string District { get; set; } = "";
        [StringLength(256)] public string DistrictId { get; set; } = "";
        [StringLength(256)] public string City { get; set; } = "";
        [StringLength(256)] public string CityId { get; set; } = "";
        [StringLength(256)] public string Country { get; set; } = "";
        [StringLength(256)] public string CountryId { get; set; } = "";

        public string FullAddress =>
            (string.IsNullOrWhiteSpace(RoomNumber) ? string.Empty : $"Phòng {RoomNumber}, ") +
            (string.IsNullOrWhiteSpace(Floor) ? string.Empty : $"Tầng {Floor}, ") +
            (string.IsNullOrWhiteSpace(Building) ? string.Empty : $"Tòa nhà {Building}, ") +
            (string.IsNullOrWhiteSpace(Street) ? string.Empty : $"{Street}") +
            (string.IsNullOrWhiteSpace(District) ? string.Empty : $", {District}") +
            (string.IsNullOrWhiteSpace(City) ? string.Empty : $", {City}") +
            (string.IsNullOrWhiteSpace(Country) ? string.Empty : $", {Country}");

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return District;
            yield return DistrictId;
            yield return City;
            yield return CityId;
            yield return Building;
            yield return Floor;
            yield return RoomNumber;
            yield return Country;
            yield return CountryId;
        }

        public new InstallationAddress GetCopy()
        {
            return (InstallationAddress)base.GetCopy();
        }
    }
}
