using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ContractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel.App
{
    public class CUTransactionServicePackageApp
    {
        public CUTransactionServicePackageApp()
        {
            //TransactionEquipments = new List<CUTransactionEquipmentApp>();
            TimeLine = new BillingTimeLine();
            TransactionPromotionForContracts = new List<TransactionPromotionForContact>();
        }
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        //public InstallationAddress InstallationAddress { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public ContractorDTO PaymentTarget { get; set; }
        //public List<CUTransactionEquipmentApp> TransactionEquipments { get; set; }
        public List<TransactionPromotionForContact> TransactionPromotionForContracts { get; set; }
        public decimal? InstallationFee { get; set; }
        public decimal? OtherFee { get; set; }
        public decimal PackagePrice { get; set; }
        //public decimal? SpInstallationFee { get; set; }
        //public decimal? SpPackagePrice { get; set; }
        //public decimal? EpInstallationFee { get; set; }
        //public decimal? EpPackagePrice { get; set; }
        public CUTransactionChannelPointCommandApp StartPoint { get; set; }
        public CUTransactionChannelPointCommandApp EndPoint { get; set; }
    }


}
