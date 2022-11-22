using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.RequestModels
{
    public class ReceiptVoucherFilterModel : RequestFilterModel
    {
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public string ProjectIds { get; set; }
        public int? OutContractId { get; set; }
        public string? OutContractIds { get; set; }
    }
}
