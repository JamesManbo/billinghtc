using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.BaseContractModels
{
    public class ServiceLevelAgreement
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
    }
}
