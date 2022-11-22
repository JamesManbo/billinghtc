using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class PromotionDetailDTO
    {
        public PromotionDetailDTO()
        {
            PromotionProducts = new List<PromotionProductDTO>();
        }
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public int PromotionValueType { get; set; }
        public string PromotionValueTypeString { get; set; }
        public string PromotionValue { get; set; }
        public int Quantity { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string Country { get; set; }
        public string CountryId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int ServicePackageId { get; set; }
        public string ServicePackageName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsChange { get; set; }
        public bool IsActive { get; set; }
        public string UpdatePerson { get; set; }
        public int NumberOfMonthApplied { get; set; }
        public int? MinPaymentPeriod { get; set; }
        public string SubjectId { get; set; }
        public List<PromotionProductDTO> PromotionProducts { get; set; }
    }

    public class PromotionDetailCommand
    {
        public int Id { get; set; }
        public int PromotionValueType { get; set; }
        public int Quantity { get; set; }
        public bool IsApplied { get; set; }
        public int? MinPaymentPeriod { get; set; }
    }
}
