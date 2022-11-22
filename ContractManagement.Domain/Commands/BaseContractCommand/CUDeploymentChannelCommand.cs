using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.BaseContractCommand
{
    public abstract class CUDeploymentChannelCommand
    {
        protected CUDeploymentChannelCommand()
        {
            TimeLine = new BillingTimeLine();
        }
        public int Id { get; set; }
        public string Uid { get; set; }
        public ServiceChannelType Type { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public int? ServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal OrgPackagePrice { get; set; }
        public bool IsHasPrice { get; set; }
        public decimal? PackagePrice { get; set; }
        public decimal? PromotionAmount { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public int? StartPointChannelId { get; set; }
        public int? EndPointChannelId { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public bool IsInFirstBilling { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? TransactionServicePackageId { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public byte IsDefaultSLAByServiceId { get; set; }
        public bool IsActive { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public int? ChannelGroupId { get; set; }
        public int ChannelIndex { get; set; }
        public int PaymentTargetId { get; set; }
        public CUContractorCommand PaymentTarget { get; set; }
        public List<int> DeletedEquipments { get; set; }
        public float LineQuantity { get; set; } // số lượng tuyến cáp
        public float? CableKilometers { get; set; } // số kilomet cáp
        public int FlexiblePricingTypeId { get; set; }
        public decimal? MaxSubTotal { get; set; } // Giá dịch vụ tối đa
        public decimal? MinSubTotal { get; set; } // Giá dịch vụ tối thiểu
        public string Note { get; set; }
        public string OtherNote { get; set; }
	    public bool? IsSupplierConfirmation { get; set; }
        public bool PreventRemoveEquipmentIfNotUpdate { get; set; }
    }

    public class CUDeploymentChannelCommand<
            TEquipmentCommand,
            TPointCommand            
        > : CUDeploymentChannelCommand 
        where TEquipmentCommand : CUDeploymentEquipmentCommand
        where TPointCommand : CUDeploymentChannelPointCommand<TEquipmentCommand>
    {
        public CUDeploymentChannelCommand()
        {
        }

        public TPointCommand StartPoint { get; set; }
        public TPointCommand EndPoint { get; set; }
    }
}
