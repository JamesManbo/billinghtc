using System.Collections.Generic;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.Commons
{
    public class Address : ValueObject
    {
        public string Street { get; set; } = "";
        public string District { get; set; } = "";
        public string DistrictId { get; set; } = "";
        public string City { get; set; } = "";
        public string CityId { get; set; } = "";

        protected Address()
        {

        }

        public Address(string street)
        {
            Street = street;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return District;
            yield return DistrictId;
            yield return City;
            yield return CityId;
        }
    }
}
