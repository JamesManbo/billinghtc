using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class PromotionDTO
    {
        public PromotionDTO()
        {
            PromotionDetails = new List<PromotionDetailDTO>();
        }
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeString { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }       
        public bool IsActive { get; set; }       
        public  List<PromotionDetailDTO> PromotionDetails{ get; set; }

    }
    public class AvailablePromotionDto
    {
        public AvailablePromotionDto()
        {
            PromotionDetails = new List<PromotionDetailDTO>();
        }
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

    public class PromotionContract
    {
        public int OutContractServicePackageId { get; set; }
        public List<PromotionDetailCommand> CurrentPromotionDetails { get; set; }
        public List<PromotionDetailCommand> NewPromotionDetails { get; set; }
        public List<PromotionDetailCommand> DelPromotionDetails { get; set; }
    }

    public class AvailablePromotionModelFilter : RequestFilterModel
    {

        public AvailablePromotionModelFilter()
        {
            ServiceIds = new List<int>();
            OutContractServicePackageIds = new List<int>();
            ProjectIds = new List<int>();
            SubjectIds = new List<int>();
            ServicePackageIds = new List<int>();
            CityIds = new List<string>();
            CountryIds = new List<string>();
            DistrictIds = new List<string>();
            FromDates = new List<DateTime>() { 
                DateTime.Now
            };
            ToDates = new List<DateTime>(){
                DateTime.Now 
            };
        }
        public List<int> ServiceIds { get; set; }
        public List<int> ServicePackageIds { get; set; }
        public List<string> CountryIds { get; set; }
        public List<string> CityIds { get; set; }
        public List<string> DistrictIds { get; set; }
        public List<int> OutContractServicePackageIds { get; set; }
        public List<int> ProjectIds { get; set; }
        public List<int> SubjectIds { get; set; }
        public int CurrencyUnitId { get; set; }
        public List<DateTime> FromDates { get; set; }
        public List<DateTime> ToDates { get; set; }
    }
}
