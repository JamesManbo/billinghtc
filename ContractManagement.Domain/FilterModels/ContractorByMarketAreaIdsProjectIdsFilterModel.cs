using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class ContractorByMarketAreaIdsProjectIdsFilterModel
    {
        public string ProjectIds { get; set; }
        public string MarketAreaIds { get; set; }
    }
}
