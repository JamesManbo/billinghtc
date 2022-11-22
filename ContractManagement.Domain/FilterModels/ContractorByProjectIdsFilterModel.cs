using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class ContractorByProjectIdsFilterModel: RequestFilterModel
    {
        public string ProjectIds { get; set; }
        public string ServiceIds { get; set; }
    }
}
