using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.BackgroundTasks.Models.Contracts
{
    public class PromotionDTO
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string PromotionDetailId { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeString { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int ContractId { get; set; }
        public int PromotionValueType { get; set; }
        public int Quantity { get; set; }
        public int NumberOfMonthApplied { get; set; }
        public List<PromotionDetailDTO> PromotionDetails { get; set; }
        public int? OutContractServicePackageId { get; set; }
        public int? PromotionForContractId { get; set; }

        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public int ProjectId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
    }
}
