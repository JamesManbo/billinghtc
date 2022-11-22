using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.TransactionModels.RequestApp
{
    public class CUTransactionServicePackageCommandApp
    {
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public int OutContractId { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public CUTransactionChannelPointCommandApp StartPoint { get; set; }
        public CUTransactionChannelPointCommandApp EndPoint { get; set; }
    }
}
