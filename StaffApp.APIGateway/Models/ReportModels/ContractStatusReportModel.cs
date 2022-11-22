using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ReportModels
{
    public class ContractStatusReportModel
    {
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public int Amount { get; set; }
    }
}
