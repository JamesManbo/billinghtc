using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUContractorCommand : IRequest<ActionResponse<ContractorDTO>>
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorShortName { get; set; }
        public string ContractorUserName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorEmail { get; set; }
        public string ContractorFax { get; set; }
        public string AccountingCustomerCode { get; set; }

        public string ContractorCity { get; set; }
        public string ContractorCityId { get; set; }
        public string ContractorDistrict { get; set; }
        public string ContractorDistrictId { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorIdNo { get; set; }
        public string ContractorTaxIdNo { get; set; }

        public bool IsEnterprise { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string Representative { get; set; }
        public string Position { get; set; }
        public string AuthorizationLetter { get; set; }

    }
}
