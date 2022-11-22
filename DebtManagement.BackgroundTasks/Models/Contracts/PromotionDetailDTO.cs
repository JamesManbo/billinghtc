using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.BackgroundTasks.Models.Contracts
{
    public class PromotionDetailDTO
    {
        public PromotionDetailDTO()
        {
        }
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public int PromotionValueType { get; set; }
        public string PromotionValueTypeString { get; set; }
        public int? PromotionValue { get; set; }
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
    }
}
