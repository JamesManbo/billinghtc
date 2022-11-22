using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class ReceiptVoucherFilterModel : RequestFilterModel
    {
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string ProjectIds { get; set; }
        public string ServiceIds { get; set; }
        public string StatusIds { get; set; }
        public bool IsOutOfDate { get; set; }
        public string CashierUserId { get; set; }
        public bool? OnlyProject { get; set; }//when has projectId not include null project

    }

    public class CollectedVoucherFilterModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string CashierUserId { get; set; }

    }
}
