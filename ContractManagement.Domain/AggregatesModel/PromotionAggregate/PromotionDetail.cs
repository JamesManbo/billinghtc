using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.PromotionAggregate
{
    [Table("PromotionDetails")]
    public class PromotionDetail : Entity
    {
        public int PromotionId { get; set; }
        public int PromotionValueType { get; set; }
        public string PromotionValue { get; set; }
        public int Quantity { get; set; }

        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string Country { get; set; }
        public string CountryId { get; set; }
        public int ServiceId { get; set; }
        public int ProjectId { get; set; }
        public string SubjectId { get; set; }
        public int ServicePackageId { get; set; }
        public int? MinPaymentPeriod { get; set; }

        public int NumberOfMonthApplied { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}
