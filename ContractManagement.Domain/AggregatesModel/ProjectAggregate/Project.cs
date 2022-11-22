using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ProjectAggregate
{
    [Table("Projects")]
    public class Project : Entity
    {
        [StringLength(256)]
        public string ProjectName { get; set; }
        [StringLength(256)]
        public string ProjectCode { get; set; }
        public int? MarketAreaId { get; set; }
        public int? AgentContractCodeId { get; set; }
        [StringLength(256)]
        public string NumberOfUnits { get; set; }
        public string Description { get; set; }
        public int? NumberOfRooms { get; set; }
        public int NumberOfSupporters { get; set; }
        [StringLength(128)]
        public string IdentityGuid { get; set; }
        public int? BusinessBlockId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
    }
}
