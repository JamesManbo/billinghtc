using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.ApplicationUsers;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUContractorPropertiesCommand : IRequest<ActionResponse<ContractorPropertiesDTO>>
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public string ContractorGroupIds { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public string ContractorIndustryIds { get; set; }
        public string ContractorStructureName { get; set; }
        public string ContractorCategoryName { get; set; }
        public string ContractorGroupNames { get; set; }
        public string ContractorClassName { get; set; }
        public string ContractorTypeName { get; set; }
        public string ContractorIndustryNames { get; set; }

        public CUContractorPropertiesCommand()
        {

        }
        public CUContractorPropertiesCommand(int contractorId, ApplicationUserDTO applicationUser)
        {
            ContractorId = contractorId;
            ContractorStructureId = applicationUser.CustomerStructureId;
            ContractorCategoryId = applicationUser.CustomerCategoryId;
            ContractorGroupIds = String.Join(",", applicationUser.GroupIds);
            ApplicationUserIdentityGuid = applicationUser.IdentityGuid;
            ContractorCategoryName = applicationUser.CustomerCategoryName;
            ContractorClassId = applicationUser.ClassId;
            ContractorClassName = applicationUser.ClassName;
            ContractorGroupNames = String.Join(",", applicationUser.GroupNames);
            ContractorStructureName = applicationUser.CustomerStructureName;
            ContractorTypeId = applicationUser.CustomerTypeId;
            ContractorTypeName = applicationUser.CustomerTypeName;
            ContractorIndustryIds = String.Join(",", applicationUser.IndustryIds);
            ContractorIndustryNames = String.Join(",", applicationUser.CustomerIndustryNames);
        }
    }
}
