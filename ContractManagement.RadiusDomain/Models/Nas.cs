using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class Nas
    {
        public int Id { get; set; }
        public string Nasname { get; set; }
        public string Shortname { get; set; }
        public string Type { get; set; }
        public int? Ports { get; set; }
        public string Secret { get; set; }
        public string Community { get; set; }
        public string Description { get; set; }
        public string Starospassword { get; set; }
        public sbyte Ciscobwmode { get; set; }
        public string Apiusername { get; set; }
        public string Apipassword { get; set; }
        public sbyte Enableapi { get; set; }
    }
}
