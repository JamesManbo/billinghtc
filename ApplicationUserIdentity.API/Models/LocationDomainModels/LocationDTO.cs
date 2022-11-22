using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.LocationDomainModels
{
    public class LocationDTO
    {
        public string Id { get; set; }
        public string LocationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public bool Active { get; set; }
        public bool IsShow { get; set; }
        public bool IsDeleted { get; set; }
        public string CountryCode { get; set; }
    }
}
