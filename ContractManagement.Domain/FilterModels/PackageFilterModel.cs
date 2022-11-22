using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class PackageFilterModel : RequestFilterModel
    {
        public int ServiceId { get; set; }
        public bool OnlyRoot { get; set; }
    }
}
