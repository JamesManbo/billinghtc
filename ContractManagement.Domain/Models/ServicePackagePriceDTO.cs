using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ServicePackagePriceDTO
    {
        public int Id { get; set; }
        public int? ChannelId { get; set; }
        public decimal PriceValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string StateLabel => !IsActive ? "Đã khóa" : "Đang hoạt động";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }

        public ServicePackagePriceDTO()
        {
        }

        public ServicePackagePriceDTO(int id, decimal priceValue, DateTime startDate, DateTime endDate
            , int currencyUnitId, string? currencyUnitCode)
        {
            this.Id = id;
            this.PriceValue = priceValue;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.CurrencyUnitId = currencyUnitId;
            this.CurrencyUnitCode = currencyUnitCode;
        }
    }    
}
