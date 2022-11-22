using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Configs.SystemArgument
{
    public class DepartmentCode
    {
        public string BusinessDepartmentCode { get; set; }
        public string CustomerCareDepartmentCode { get; set; }
        public string BoardOfDirectorsCode { get; set; }
        public string ServiceProviderDepartmentCode { get; set; }
        public string SupporterDepartmentCode { get; set; }
    }

    public class ServiceConfigs
    {
        public string FTTHService { get; set; }
        public string TVService { get; set; }
    }
}
