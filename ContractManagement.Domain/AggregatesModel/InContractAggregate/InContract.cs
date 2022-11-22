using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("InContracts")]
    public class InContract : ContractAbstraction
    {
        public InContract()
        {
            _servicePackages = new List<OutContractServicePackage>();
            _contractSharingRevenues = new List<ContractSharingRevenueLine>();
            _contactInfos = new List<ContactInfo>();
            _inContractTax = new List<InContractTax>();
            _contractTotalByCurrencies = new List<ContractTotalByCurrency>();
        }

        public InContract(CreateInContractCommand contractCommand)
        {
            _servicePackages = new List<OutContractServicePackage>();
            _contractSharingRevenues = new List<ContractSharingRevenueLine>();
            _contactInfos = new List<ContactInfo>();
            _inContractTax = new List<InContractTax>();
            _contractTotalByCurrencies = new List<ContractTotalByCurrency>();

            CreatedBy = contractCommand.CreatedBy;
            ContractTypeId = contractCommand.ContractTypeId;
            IdentityGuid = Guid.NewGuid().ToString();
            ContractCode = contractCommand.ContractCode;
            MarketAreaId = contractCommand.MarketAreaId;
            ProjectId = contractCommand.ProjectId;
            CurrencyUnitId = contractCommand.CurrencyUnitId;
            CurrencyUnitCode = contractCommand.CurrencyUnitCode;

            MarketAreaName = contractCommand.MarketAreaName;
            ProjectName = contractCommand.ProjectName;
            IsIncidentControl = contractCommand.IsIncidentControl;
            IsControlUsageCapacity = contractCommand.IsControlUsageCapacity;
            ContractStatusId = contractCommand.ContractStatusId;
            TimeLine = contractCommand.TimeLine;
            Payment = contractCommand.Payment;
            FiberNodeInfo = contractCommand.FiberNodeInfo;
            ContractNote = contractCommand.ContractNote;
            OrganizationUnitId = contractCommand.OrganizationUnitId;
            OrganizationUnitCode = contractCommand.OrganizationUnitCode;
            OrganizationUnitName = contractCommand.OrganizationUnitName;
            SignedUserId = contractCommand.SignedUserId;
            SignedUserName = contractCommand.SignedUserName;
            CashierUserId = contractCommand.CashierUserId;
            InvoicingAddress = contractCommand.InvoicingAddress;
            NumberBillingLimitDays = contractCommand.NumberBillingLimitDays;

            InterestOnDefferedPayment = contractCommand.InterestOnDefferedPayment;
            ContractViolation = contractCommand.ContractViolation;
            ContractViolationType = contractCommand.ContractViolationType;

            AutoRenew = contractCommand.AutoRenew;
            AccountingCustomerCode = contractCommand.AccountingCustomerCode;
            //SetInstallationFeeValue(contractCommand.InstallationFee);
            //SetOtherFeeValue(contractCommand.OtherFee);
        }
        public string FiberNodeInfo { get; set; }
        public string ContractNote { get; set; }

        public string OrganizationUnitId { get; set; }
        public string CashierUserId { get; set; }
        public IReadOnlyCollection<ContactInfo> ContactInfos => _contactInfos;
        private readonly List<ContactInfo> _contactInfos;

        public IReadOnlyCollection<InContractTax> InContractTax => _inContractTax;
        private readonly List<InContractTax> _inContractTax;

        public void SetNextBillingDate(DateTime nextBilling)
        {
            TimeLine.NextBillingDate = nextBilling;
        }

        public void Update(UpdateInContractCommand updateCommand)
        {
            UpdatedBy = updateCommand.UpdatedBy;
            IdentityGuid = Guid.NewGuid().ToString();
            MarketAreaId = updateCommand.MarketAreaId;
            ProjectId = updateCommand.ProjectId;
            CurrencyUnitId = updateCommand.CurrencyUnitId;
            CurrencyUnitCode = updateCommand.CurrencyUnitCode;

            MarketAreaName = updateCommand.MarketAreaName;
            ProjectName = updateCommand.ProjectName;
            IsIncidentControl = updateCommand.IsIncidentControl;
            IsControlUsageCapacity = updateCommand.IsControlUsageCapacity;
            ContractStatusId = updateCommand.ContractStatusId;
            TimeLine = updateCommand.TimeLine;
            Payment = updateCommand.Payment;
            FiberNodeInfo = updateCommand.FiberNodeInfo;
            ContractNote = updateCommand.ContractNote;
            OrganizationUnitId = updateCommand.OrganizationUnitId;
            OrganizationUnitCode = updateCommand.OrganizationUnitCode;
            OrganizationUnitName = updateCommand.OrganizationUnitName;
            SignedUserId = updateCommand.SignedUserId;
            SignedUserName = updateCommand.SignedUserName;
            CashierUserId = updateCommand.CashierUserId;
            NumberBillingLimitDays = updateCommand.NumberBillingLimitDays;

            InterestOnDefferedPayment = updateCommand.InterestOnDefferedPayment;
            ContractViolation = updateCommand.ContractViolation;
            ContractViolationType = updateCommand.ContractViolationType;

            AutoRenew = updateCommand.AutoRenew;
            AccountingCustomerCode = updateCommand.AccountingCustomerCode;
        }
        private void DetectContractProjectId()
        {
            if (this.ActiveServicePackages != null && this.ActiveServicePackages.Count > 0)
            {
                var firstProject = this.ActiveServicePackages.FirstOrDefault(c => c.ProjectId.HasValue);
                this.ProjectId = firstProject?.ProjectId;
            }
            else
            {
                this.ProjectId = null;
            }
        }

        public ContractSharingRevenueLine AddContractSharingRevenue(CUContractSharingRevenueLineCommand cUContractSharingRevenueCommand)
        {
            var newContractSharingRevenue = new ContractSharingRevenueLine(cUContractSharingRevenueCommand)
            {
                SharingType = this.ContractTypeId.Value,
                InContractCode = this.ContractCode,
                InContractId = this.Id,
                CreatedDate = DateTime.Now,
                CreatedBy = this.CreatedBy
            };
            _contractSharingRevenues.Add(newContractSharingRevenue);

            return newContractSharingRevenue;
        }

        public OutContractServicePackage AddServicePackage(CUOutContractChannelCommand addChannelCmd, bool forceBind = false)
        {
            addChannelCmd.InContractId = Id;
            addChannelCmd.TimeLine.PaymentForm = this.Payment.Form;

            var newChannel = new OutContractServicePackage(addChannelCmd, false);
            newChannel.CalculateTotal();
            _servicePackages.Add(newChannel);
            this.DetectHaveEquipment();
            this.DetectContractProjectId();

            return newChannel;
        }

        public void UpdateServicePackage(CUOutContractChannelCommand updateChannelCmd, bool forceBind = false)
        {
            var channelEntity = this.ServicePackages.First(s => s.Id == updateChannelCmd.Id);
            channelEntity.UpdatedBy = this.UpdatedBy;
            channelEntity.UpdatedDate = DateTime.Now;

            updateChannelCmd.TimeLine.PaymentForm = this.Payment.Form;
            channelEntity.Update(updateChannelCmd, forceBind);
            this.DetectHaveEquipment();
            this.DetectContractProjectId();
            channelEntity.CalculateTotal();
        }
        public void RemoveServicePackage(int outServicePackageId)
        {
            var toRemoveElm = _servicePackages.Find(s => s.Id == outServicePackageId);
            _servicePackages.Remove(toRemoveElm);
            this.DetectHaveEquipment();
            this.DetectContractProjectId();
        }

        public ContactInfo AddContactInfo(CUContactInfoCommand cUContactInfoCommand)
        {
            cUContactInfoCommand.InContractId = Id;
            var newContactInfo = new ContactInfo(cUContactInfoCommand);
            _contactInfos.Add(newContactInfo);

            return newContactInfo;
        }

        //public void AddInContractServices(CUInContractServiceCommand inContractServiceCmd)
        //{
        //    var inContractService = new InContractService(inContractServiceCmd);
        //    this._inContractServices.Add(inContractService);
        //}

        public void AddInContractTax(InContractTax inContractTax)
        {
            _inContractTax.Add(inContractTax);
        }

        public void ClearContactInfos()
        {
            _contactInfos.Clear();
        }

        //public void ClearInContractServices()
        //{
        //    _inContractServices.Clear();
        //}

        public void SetContractor(int contractorId)
        {
            this.ContractorId = contractorId;
        }

        public void SetOtherFeeValue(decimal value)
        {
            if (value < 0)
            {
                throw new ContractDomainException("Other fee is not valid");
            }

            //this.OtherFee = value;
        }

        public void SetTaxValue(float taxValue)
        {
            if (taxValue < 0 || taxValue >= 100)
            {
                throw new ContractDomainException("Tax percent is not valid");
            }

            this.TotalTaxPercent = taxValue;
        }

        public void SetInstallationFeeValue(decimal installationFeeValue)
        {
            if (installationFeeValue < 0)
            {
                throw new ContractDomainException("Installation fee is not valid");
            }

            //this.InstallationFee = installationFeeValue;
        }

        public void SetSucceedStatus()
        {

            ContractStatusId = ContractStatus.Signed.Id;
            Description = "Hợp đồng đã có hiệu lực.";
            //AddDomainEvent(new OrderShippedDomainEvent(this));
        }

        public void SetCancelledStatus()
        {
            ContractStatusId = ContractStatus.Cancelled.Id;
            Description = $"Hợp đồng đã bị hủy.";
        }

        public void SetTerminatedStatus()
        {
            if (ContractStatusId != ContractStatus.Signed.Id)
            {
                StatusChangeException(ContractStatus.Liquidated);
            }

            ContractStatusId = ContractStatus.Liquidated.Id;
            Description = $"Hợp đồng đã bị thanh lý.";
            //AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        private void StatusChangeException(ContractStatus orderStatusToChange)
        {
            throw new ContractDomainException(
                $"Is not possible to change the order status from {ContractStatus.Name} to {orderStatusToChange.Name}.");
        }
        protected ContractTotalByCurrency CreateNewContractTotal(int? currencyUnitId = null, string currencyUnitCode = "")
        {
            var newContractTotal = new ContractTotalByCurrency()
            {
                InContractId = this.Id,
                CreatedBy = this.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            if (string.IsNullOrEmpty(currencyUnitCode))
            {
                newContractTotal.CurrencyUnitCode = this.CurrencyUnitCode ?? CurrencyUnit.VND.CurrencyUnitCode;
                newContractTotal.CurrencyUnitId = this.CurrencyUnitId == 0 ? CurrencyUnit.VND.Id : this.CurrencyUnitId;
            }
            else
            {
                newContractTotal.CurrencyUnitCode = currencyUnitCode;
                newContractTotal.CurrencyUnitId = currencyUnitId.Value;
            }

            this._contractTotalByCurrencies.Add(newContractTotal);

            return newContractTotal;
        }

        public sealed override void CalculateTotal()
        {
            if (this.ContractTypeId == InContractType.InChannelRental.Id &&
                this.ActiveServicePackages != null &&
                this.ActiveServicePackages.Count > 0)
            {
                var channelsGroupedByCurrency = ActiveServicePackages
                       .GroupBy(k => k.CurrencyUnitCode)
                       .Select(g => new
                       {
                           CurrencyUnitCode = g.Key,
                           CurrencyUnitId = g.First().CurrencyUnitId,
                           Channels = g
                       });

                IsHasOneCurrency = channelsGroupedByCurrency.Count() == 1;

                TotalTaxPercent = ActiveServicePackages.Sum(t => t.TaxPercent);
                if (IsHasOneCurrency)
                {
                    this.CurrencyUnitId = channelsGroupedByCurrency.First().CurrencyUnitId;
                    this.CurrencyUnitCode = channelsGroupedByCurrency.First().CurrencyUnitCode;

                    if (this.ContractTotal == null)
                    {
                        this.CreateNewContractTotal();
                    }
                    else
                    {
                        this.ContractTotal.CurrencyUnitCode = this.CurrencyUnitCode;
                        this.ContractTotal.CurrencyUnitId = this.CurrencyUnitId;
                    }

                    this.ContractTotal.Calculate(ActiveServicePackages);
                }
                else
                {
                    foreach (var channelsByCurrency in channelsGroupedByCurrency)
                    {
                        var totalByCurrency = this.ContractTotalByCurrencies
                            .FirstOrDefault(c => c.CurrencyUnitCode == channelsByCurrency.CurrencyUnitCode);
                        if (totalByCurrency == null)
                            totalByCurrency
                                = this.CreateNewContractTotal(channelsByCurrency.CurrencyUnitId, channelsByCurrency.CurrencyUnitCode);

                        totalByCurrency.Calculate(channelsByCurrency.Channels);
                    }
                }
            }
        }
    }
}
