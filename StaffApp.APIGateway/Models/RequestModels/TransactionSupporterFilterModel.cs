using Global.Models.Filter;
using System;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class TransactionSupporterFilterModel 
    {
        public string UserIdentity { get; set; }
        public string ProjectIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
