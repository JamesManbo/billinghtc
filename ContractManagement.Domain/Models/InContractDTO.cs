using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.Models.SharingRevenueModels;

namespace ContractManagement.Domain.Models
{
   public class InContractDTO : ContractBaseDTO
    {
        public InContractDTO()
        {
            ServicePackages = new List<OutContractServicePackageDTO>();
            ContractSharingRevenues = new List<ContractSharingRevenueLineDTO>();
            TaxCategories = new List<TaxCategoryDTO>();
            //InContractServices = new List<InContractServiceDTO>();
            Direction = "in";
        }
        public string Label { get; set; }
        public DateTime BillingDate { get; set; }
        public string FiberNodeInfo { get; set; }
        public bool IsChangeable => Id == 0 || AggregatesModel.BaseContract.ContractStatus.Draft.Id == ContractStatusId;
        public List<OutContractServicePackageDTO> ServicePackages { get; set; }
        public List<ContractSharingRevenueLineDTO> ContractSharingRevenues { get; set; }
        //public List<InContractServiceDTO> InContractServices { get; set; }
        public List<ContractOfTaxDTO> ContractOfTaxes { get; set; }
    }
}
