using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ContractModels
{
    public class OutContractDTO : ContractBaseDTO
    {
        public OutContractDTO()
        {
            ServicePackages = new List<OutContractServicePackageDTO>();
            ActiveServicePackages = new List<OutContractServicePackageDTO>();
            TaxCategories = new List<TaxCategoryDTO>();
            ContractSharingRevenues = new List<ContractSharingRevenueDTO>();
        }
        public string FiberNodeInfo { get; set; }
        public string AgentContractCode { get; set; } // Mã hợp đồng đại lý
        public string AccountingCustomerCode { get; set; } // Mã hợp đồng đại lý
        public decimal EquipmentAmount { get; set; }
        public decimal EquipmentAmountBeforeTax { get; set; }
        public decimal ServicePackageAmount { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal ServicePackageAmountBeforeTax { get; set; }
        public bool IsAutomaticGenerateReceipt { get; set; }
        public int? InterestOnDefferedPayment { get; set; }
        public int? ContractViolation { get; set; }
        public int? ContractViolationType { get; set; }
        public List<OutContractServicePackageDTO> ServicePackages { get; set; }
        public List<OutContractServicePackageDTO> ActiveServicePackages { get; set; }
        public List<ContractSharingRevenueDTO> ContractSharingRevenues { get; set; }
        //public bool IsChangeable => Id == 0 || AggregatesModel.BaseContract.ContractStatus.Draft.Id == ContractStatusId;
        public string CustomerCareStaffUserId { get; set; }
    }
}
