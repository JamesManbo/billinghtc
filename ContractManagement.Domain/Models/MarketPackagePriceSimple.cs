using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public class MarketPackagePriceSimple
    {
        public MarketPackagePriceSimple()
        {
        }

        public MarketPackagePriceSimple(int projectId, decimal priceValue)
        {
            this.ProjectId = projectId;
            this.Price = priceValue;
        }

        public int ProjectId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyUnitId { get; set; }
        public int PackagePriceType { get; set; }
    }
}
