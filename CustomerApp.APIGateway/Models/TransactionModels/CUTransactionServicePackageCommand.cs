using CustomerApp.APIGateway.Models.OutContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.TransactionModels
{
    public class CUTransactionServicePackageCommand
    {
        public bool CreateFromCustomer { get; set; }
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public int OutContractId { get; set; }
        public ContractorDTO PaymentTarget { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public CUTransactionChannelPointCommand StartPoint { get; set; }
        public CUTransactionChannelPointCommand EndPoint { get; set; }
    }
}
