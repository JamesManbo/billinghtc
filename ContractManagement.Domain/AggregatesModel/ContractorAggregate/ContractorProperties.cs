using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ContractorAggregate
{
    [Table("ContractorProperties")]
    public class ContractorProperties : Entity
    {
        public ContractorProperties()
        {

        }
        public int ContractorId { get; set; }
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public string ContractorGroupIds { get; set; }
        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public string ContractorIndustryIds { get; set; }
        [StringLength(68)]
        public string ApplicationUserIdentityGuid { get; set; }
        public string ContractorStructureName { get; set; }
        public string ContractorCategoryName { get; set; }
        public string ContractorGroupNames { get; set; }
        public string ContractorClassName { get; set; }
        public string ContractorTypeName { get; set; }
        public string ContractorIndustryNames { get; set; }
    }
}
