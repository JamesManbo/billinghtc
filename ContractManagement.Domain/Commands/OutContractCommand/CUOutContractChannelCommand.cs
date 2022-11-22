using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Commands.ServicePackagePriceCommand;
using ContractManagement.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUOutContractChannelCommand
        : CUDeploymentChannelCommand<
            CUContractEquipmentCommand,
            CUOutputChannelPointCommand
            >, IRequest
    {
        public List<CUServiceLevelAgreementCommand> ServiceLevelAgreements { get; set; }
        public CUOutContractChannelCommand()
        {
            ServiceLevelAgreements = new List<CUServiceLevelAgreementCommand>();
            PromotionForContractNews = new List<CreateAppliedPromotionCommand>();
            PromotionForContractDels = new List<CreateAppliedPromotionCommand>();
            OutContractServicePackageTaxes = new List<CUOutContractServicePackageTaxCommand>();
            PackagePriceCurrencys = new CUServicePackagePriceCommand();
            PriceBusTables = new List<CUChannelPriceBusTableCommand>();
        }
        public List<CUOutContractServicePackageTaxCommand> OutContractServicePackageTaxes { get; set; }
        public List<CreateAppliedPromotionCommand> PromotionForContractNews { get; set; }
        public List<CreateAppliedPromotionCommand> PromotionForContractDels { get; set; }
        public CUServicePackagePriceCommand PackagePriceCurrencys { get; set; }
        public List<CUChannelPriceBusTableCommand> PriceBusTables { get; set; }

        public void Binding(ServicePackageDTO packageInfo)
        {
            this.ServicePackageId = packageInfo.Id;
            this.PackageName = packageInfo.PackageName;
            this.OrgPackagePrice = packageInfo.Price;
            this.BandwidthLabel = packageInfo.BandwidthLabel;
            this.InternationalBandwidth = packageInfo.InternationalBandwidth;
            this.DomesticBandwidth = packageInfo.DomesticBandwidth;
            this.InternationalBandwidthUom = packageInfo.InternationalBandwidthUom;
            this.DomesticBandwidth = packageInfo.DomesticBandwidth;
            this.DomesticBandwidthUom = packageInfo.DomesticBandwidthUom;
        }
    }
}