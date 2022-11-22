using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.PromotionModels
{
    public class AvailablePromotionDTO
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string PromotionDetailId { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeString { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateFormat { get { return StartDate.ToString("dd/MM/yyyy"); } }
        public DateTime EndDate { get; set; }
        public string EndDateFormat { get { return EndDate.ToString("dd/MM/yyyy"); } }
        public string Description { get; set; }
        public int ContractId { get; set; }
        public int PromotionValueType { get; set; }
        public int Quantity { get; set; }
        public int NumberOfMonthApplied { get; set; }

        public int? OutContractServicePackageId { get; set; }
        public int? PromotionForContractId { get; set; }
    }
}
