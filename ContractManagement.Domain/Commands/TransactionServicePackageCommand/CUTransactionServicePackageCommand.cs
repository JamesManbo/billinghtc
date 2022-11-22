using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.ServicePackagePriceCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;

namespace ContractManagement.Domain.Commands.TransactionServicePackageCommand
{
    public class CUTransactionServicePackageCommand :
        CUDeploymentChannelCommand<CUTransactionEquipmentCommand, CUTransactionChannelPointCommand>,
        IRequest<ActionResponse<TransactionServicePackageDTO>>, IBind
    {
        public CUTransactionServicePackageCommand()
        {
            TransactionChannelTaxes = new List<CUTransactionTaxCommand>();
            ServiceLevelAgreements = new List<CUTransactionSLACommand>();
            TransactionPromotionForContracts = new List<CUTransactionPromotionForContractCommand>();
            TimeLine = new BillingTimeLine();
            PackagePriceCurrencys = new CUServicePackagePriceCommand();
            PriceBusTables = new List<CUTransactionPriceBusTableCommand>();
        }
        public int OutContractServicePackageId { get; set; }
        public int TransactionId { get; set; }
        public bool? IsAcceptanced { get; set; }
        public bool? IsOld { get; set; }
        public int? OldId { get; set; }
        public bool CreateFromCustomer { get; set; }
        public bool IsMultipleTransaction { get; set; }
        public int TransactionType { get; set; }
        public CUServicePackagePriceCommand PackagePriceCurrencys { get; set; }

        public List<CUTransactionSLACommand> ServiceLevelAgreements { get; set; }
        public List<CUTransactionTaxCommand> TransactionChannelTaxes { get; set; }
        public List<CUTransactionPromotionForContractCommand> TransactionPromotionForContracts { get; set; }
        public List<CUTransactionPriceBusTableCommand> PriceBusTables { get; set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUOutContractChannelCommand channelCommand)
            {
                this.Id = channelCommand.Id;
                this.Uid = channelCommand.Uid;
                this.CurrencyUnitId = channelCommand.CurrencyUnitId;
                this.CurrencyUnitCode = channelCommand.CurrencyUnitCode;
                this.OutContractId = channelCommand.OutContractId;
                this.InContractId = channelCommand.InContractId;
                this.ServicePackageId = channelCommand.ServicePackageId;
                this.ServiceId = channelCommand.ServiceId;
                this.ServiceName = channelCommand.ServiceName;
                this.PackageName = channelCommand.PackageName;
                this.InstallationFee = channelCommand.InstallationFee;
                this.OtherFee = channelCommand.OtherFee;
                this.OrgPackagePrice = channelCommand.OrgPackagePrice;
                this.IsHasPrice = channelCommand.IsHasPrice;
                this.PackagePrice = channelCommand.PackagePrice;
                this.PromotionAmount = channelCommand.PromotionAmount;
                this.BandwidthLabel = channelCommand.BandwidthLabel;
                this.InternationalBandwidth = channelCommand.InternationalBandwidth;
                this.DomesticBandwidth = channelCommand.DomesticBandwidth;
                this.InternationalBandwidthUom = channelCommand.InternationalBandwidthUom;
                this.DomesticBandwidthUom = channelCommand.DomesticBandwidthUom;
                this.CId = channelCommand.CId;
                this.RadiusAccount = channelCommand.RadiusAccount;
                this.RadiusPassword = channelCommand.RadiusPassword;

                this.StartPointChannelId = channelCommand.StartPointChannelId;
                this.EndPointChannelId = channelCommand.EndPointChannelId;

                this.IsFreeStaticIp = channelCommand.IsFreeStaticIp;
                this.IsInFirstBilling = channelCommand.IsInFirstBilling;
                this.TimeLine = channelCommand.TimeLine;
                this.CreatedBy = channelCommand.CreatedBy;
                this.CreatedDate = channelCommand.CreatedDate;
                this.HasStartAndEndPoint = channelCommand.HasStartAndEndPoint;
                this.StatusId = channelCommand.StatusId;
                this.TransactionServicePackageId = channelCommand.TransactionServicePackageId;
                this.IsTechnicalConfirmation = channelCommand.IsTechnicalConfirmation;
                this.IsDefaultSLAByServiceId = channelCommand.IsDefaultSLAByServiceId;
                this.IsActive = channelCommand.IsActive;
                this.ChannelGroupId = channelCommand.ChannelGroupId;
                this.PaymentTargetId = channelCommand.PaymentTargetId;
                this.PaymentTarget = channelCommand.PaymentTarget;
                this.DeletedEquipments = channelCommand.DeletedEquipments;
                this.PackagePriceCurrencys = channelCommand.PackagePriceCurrencys;

                FlexiblePricingTypeId = channelCommand.FlexiblePricingTypeId;

                LineQuantity = channelCommand.LineQuantity;
                CableKilometers = channelCommand.CableKilometers;

                HasDistinguishBandwidth = channelCommand.HasDistinguishBandwidth;
                IsInFirstBilling = channelCommand.IsInFirstBilling;

                MaxSubTotal = channelCommand.MaxSubTotal;
                MinSubTotal = channelCommand.MinSubTotal;

                if (channelCommand.StartPoint != null)
                {
                    this.StartPoint = new CUTransactionChannelPointCommand();
                    this.StartPoint.Binding(channelCommand.StartPoint);
                }

                this.EndPoint = new CUTransactionChannelPointCommand();
                this.EndPoint.Binding(channelCommand.EndPoint);

                if (channelCommand.OutContractServicePackageTaxes.Count > 0)
                {
                    foreach (var channelTax in channelCommand.OutContractServicePackageTaxes)
                    {
                        var transactionTaxCommand = new CUTransactionTaxCommand()
                        {
                            TransactionId = this.TransactionId,
                            TransactionServicePackageId = this.Id,
                            TaxCategoryId = channelTax.TaxCategoryId,
                            TaxCategoryCode = channelTax.TaxCategoryCode,
                            TaxCategoryName = channelTax.TaxCategoryName,
                            TaxValue = channelTax.TaxValue
                        };

                        this.TransactionChannelTaxes.Add(transactionTaxCommand);
                    }
                }

                if (channelCommand.PriceBusTables != null && channelCommand.PriceBusTables.Count > 0)
                {
                    foreach (var priceTableValue in channelCommand.PriceBusTables)
                    {
                        var transPriceBusTableCmd = new CUTransactionPriceBusTableCommand
                        {
                            CurrencyUnitCode = priceTableValue.CurrencyUnitCode,
                            UsageValueFrom = priceTableValue.UsageValueFrom,
                            UsageValueFromUomId = priceTableValue.UsageValueFromUomId,
                            UsageValueTo = priceTableValue.UsageValueTo,
                            UsageValueToUomId = priceTableValue.UsageValueToUomId,
                            PriceValue = priceTableValue.PriceValue,
                            PriceUnitUomId = priceTableValue.PriceUnitUomId,
                            IsDomestic = priceTableValue.IsDomestic
                        };
                        this.PriceBusTables.Add(transPriceBusTableCmd);
                    }
                }
            }
            else if (command is OutContractServicePackageDTO channel)
            {

                this.CurrencyUnitId = channel.CurrencyUnitId;
                this.CurrencyUnitCode = channel.CurrencyUnitCode;
                this.OutContractId = channel.OutContractId;
                this.OutContractServicePackageId = channel.Id;

                this.InContractId = channel.InContractId;
                this.ServiceId = channel.ServiceId;
                this.ServiceName = channel.ServiceName;

                this.ServicePackageId = channel.ServicePackageId;
                this.PackageName = channel.PackageName;
                this.BandwidthLabel = channel.BandwidthLabel;
                this.InternationalBandwidth = channel.InternationalBandwidth;
                this.DomesticBandwidth = channel.DomesticBandwidth;
                this.InternationalBandwidthUom = channel.InternationalBandwidthUom;
                this.DomesticBandwidthUom = channel.DomesticBandwidthUom;

                this.PackagePrice = channel.PackagePrice;
                this.InstallationFee = channel.InstallationFee;
                this.OtherFee = channel.OtherFee;
                this.OrgPackagePrice = channel.OrgPackagePrice;
                this.IsHasPrice = channel.IsHasPrice;
                this.PromotionAmount = channel.PromotionAmount;
                this.CId = channel.CId;
                this.RadiusAccount = channel.RadiusAccount;
                this.RadiusPassword = channel.RadiusPassword;

                // this.StartPointChannelId = channel.StartPointChannelId;
                // this.EndPointChannelId = channel.EndPointChannelId;

                this.IsFreeStaticIp = channel.IsFreeStaticIp;
                this.IsInFirstBilling = channel.IsInFirstBilling;
                this.TimeLine = channel.TimeLine;
                this.HasStartAndEndPoint = channel.HasStartAndEndPoint;
                this.StatusId = channel.StatusId;
                this.TransactionServicePackageId = channel.TransactionServicePackageId;
                this.OldId = channel.OldId;
                this.IsTechnicalConfirmation = channel.IsTechnicalConfirmation;
                this.IsDefaultSLAByServiceId = channel.IsDefaultSLAByServiceId;
                this.IsActive = true;
                this.ChannelGroupId = channel.ChannelGroupId;
                this.PaymentTargetId = channel.PaymentTargetId;

                FlexiblePricingTypeId = channel.FlexiblePricingTypeId;

                LineQuantity = channel.LineQuantity;
                CableKilometers = channel.CableKilometers;

                HasDistinguishBandwidth = channel.HasDistinguishBandwidth;
                IsInFirstBilling = channel.IsInFirstBilling;

                MaxSubTotal = channel.MaxSubTotal;
                MinSubTotal = channel.MinSubTotal;


                if (channel.StartPoint != null)
                {
                    this.StartPoint = new CUTransactionChannelPointCommand();
                    this.StartPoint.Binding(channel.StartPoint);
                }

                this.EndPoint = new CUTransactionChannelPointCommand();
                this.EndPoint.Binding(channel.EndPoint);

                if (channel.OutContractServicePackageTaxes.Count > 0)
                {
                    foreach (var channelTax in channel.OutContractServicePackageTaxes)
                    {
                        var transactionTaxCommand = new CUTransactionTaxCommand()
                        {
                            TransactionId = this.TransactionId,
                            TransactionServicePackageId = this.Id,
                            TaxCategoryId = channelTax.TaxCategoryId,
                            TaxCategoryCode = channelTax.TaxCategoryCode,
                            TaxCategoryName = channelTax.TaxCategoryName,
                            TaxValue = channelTax.TaxValue
                        };

                        this.TransactionChannelTaxes.Add(transactionTaxCommand);
                    }
                }

                if (channel.PriceBusTables != null && channel.PriceBusTables.Count > 0)
                {
                    foreach (var priceTableValue in channel.PriceBusTables)
                    {
                        var transPriceBusTableCmd = new CUTransactionPriceBusTableCommand
                        {
                            CurrencyUnitCode = priceTableValue.CurrencyUnitCode,
                            UsageValueFrom = priceTableValue.UsageValueFrom,
                            UsageValueFromUomId = priceTableValue.UsageValueFromUomId,
                            UsageValueTo = priceTableValue.UsageValueTo,
                            UsageValueToUomId = priceTableValue.UsageValueToUomId,
                            PriceValue = priceTableValue.PriceValue,
                            PriceUnitUomId = priceTableValue.PriceUnitUomId,
                            IsDomestic = priceTableValue.IsDomestic
                        };
                        this.PriceBusTables.Add(transPriceBusTableCmd);
                    }
                }
            }
        }
    }
}
