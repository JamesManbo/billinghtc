using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractorPropertiesDTO : BaseDTO
    {
        public ContractorPropertiesDTO()
        {
        }
        
        public int ContractorId { get; set; }
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public string ContractorGroupIds { get; set; }

        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public int? ContractorIndustryId { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string ContractorStructureName { get; set; }
        public string ContractorCategoryName { get; set; }
        public string ContractorGroupNames { get; set; }
        public string ContractorClassName { get; set; }
        public string ContractorTypeName { get; set; }
        public string ContractorIndustryName { get; set; }
    }
}
