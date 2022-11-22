using Global.Models.StateChangedResponse;
using MediatR;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ContractModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class CUTransactionServicePackage
    {
        public CUTransactionServicePackage()
        {
            //TransactionEquipments = new List<CUTransactionEquipment>();
            TimeLine = new BillingTimeLineDTO();
            TransactionPromotionForContracts = new List<TransactionPromotionForContact>();
        }

        public int Id { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int TransactionId { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public BillingTimeLineDTO TimeLine { get;  set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public int? OutletChannelId { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public decimal TaxAmount { get; set; }
        public Discount Discount { get; set; }
        public ContractorDTO PaymentTarget { get; set; }
        public decimal InstallationFee { get;  set; }
        public decimal OtherFee { get;  set; }
        public decimal PackagePrice { get;  set; }
        public int StatusId { get; set; }
        public bool? IsOld { get; set; }
        public int? OldId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //public List<CUTransactionEquipment> TransactionEquipments { get; set; }
        public List<TransactionPromotionForContact> TransactionPromotionForContracts { get; set; }
        public CUTransactionChannelPointCommand StartPoint { get; set; }
        public CUTransactionChannelPointCommand EndPoint { get; set; }
    }
}
