using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Exceptions;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("OutContracts")]
    public class OutContract : ContractAbstraction
    {
        public string FiberNodeInfo { get; set; }
        public string ContractNote { get; set; }
        public string AgentContractCode { get; set; } // Mã hợp đồng đại lý
        public string OrganizationUnitId { get; set; }
        [StringLength(68)]
        public string CashierUserId { get; set; }
        [StringLength(256)]
        public string CashierUserName { get; set; }
        [StringLength(256)]
        public string CashierFullName { get; set; }
        public bool IsAutomaticGenerateReceipt { get; set; }
        public string CustomerCareStaffUserId { get; set; }
        public IReadOnlyCollection<ContactInfo> ContactInfos => _contactInfos.Where(s => !s.IsDeleted).ToList();
        private readonly List<ContactInfo> _contactInfos;

        public IReadOnlyCollection<Transaction> Transactions => _transactions.Where(s => !s.IsDeleted).ToList();
        private readonly List<Transaction> _transactions;

        public OutContract()
        {
            _servicePackages = new List<OutContractServicePackage>();
            _contactInfos = new List<ContactInfo>();
            _contractSharingRevenues = new List<ContractSharingRevenueLine>();
            _transactions = new List<Transaction>();
            _contractTotalByCurrencies = new List<ContractTotalByCurrency>();

            ContractStatusId = ContractStatus.Draft.Id;
            CurrencyUnitId = CurrencyUnit.VND.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }

        public OutContract(CreateContractCommand contractCommand)
        {
            _servicePackages = new List<OutContractServicePackage>();
            _contactInfos = new List<ContactInfo>();
            _contractSharingRevenues = new List<ContractSharingRevenueLine>();
            _transactions = new List<Transaction>();
            _contractTotalByCurrencies = new List<ContractTotalByCurrency>();

            ContractStatusId = ContractStatus.Draft.Id;

            CreatedBy = contractCommand.CreatedBy;
            ContractTypeId = contractCommand.ContractTypeId;
            IdentityGuid = Guid.NewGuid().ToString();
            ContractCode = contractCommand.ContractCode;
            AgentContractCode = contractCommand.AgentContractCode;
            MarketAreaId = contractCommand.MarketAreaId;
            ProjectId = contractCommand.ProjectId;

            CurrencyUnitId = contractCommand.CurrencyUnitId;
            CurrencyUnitCode = contractCommand.CurrencyUnitCode;

            MarketAreaName = contractCommand.MarketAreaName;
            ProjectName = contractCommand.ProjectName;
            OrganizationUnitName = contractCommand.OrganizationUnitName;
            OrganizationUnitCode = contractCommand.OrganizationUnitCode;
            SignedUserName = contractCommand.SignedUserName;

            AgentId = contractCommand.AgentId;
            AgentCode = contractCommand.AgentCode;
            AgentContractCode = contractCommand.AgentContractCode;
            IsIncidentControl = contractCommand.IsIncidentControl;
            IsControlUsageCapacity = contractCommand.IsControlUsageCapacity;
            TimeLine = contractCommand.TimeLine;
            FiberNodeInfo = contractCommand.FiberNodeInfo;
            ContractNote = contractCommand.ContractNote;
            SignedUserId = contractCommand.SignedUserId;
            OrganizationUnitId = contractCommand.OrganizationUnitId;
            IsAutomaticGenerateReceipt = contractCommand.IsAutomaticGenerateReceipt;
            CustomerCareStaffUserId = contractCommand.CustomerCareStaffUserId;
            NumberBillingLimitDays = contractCommand.NumberBillingLimitDays;

            CashierUserId = contractCommand.CashierUserId;
            CashierUserName = contractCommand.CashierUserName;
            CashierFullName = contractCommand.CashierFullName;

            InterestOnDefferedPayment = contractCommand.InterestOnDefferedPayment;
            ContractViolation = contractCommand.ContractViolation;
            ContractViolationType = contractCommand.ContractViolationType;

            InvoicingAddress = contractCommand.InvoicingAddress;
            AutoRenew = contractCommand.AutoRenew;
            AccountingCustomerCode = contractCommand.AccountingCustomerCode;

            SetPaymentMethod(contractCommand.Payment);
        }

        public void Update(UpdateContractCommand updateCommand)
        {
            UpdatedBy = updateCommand.UpdatedBy;
            MarketAreaId = updateCommand.MarketAreaId;
            ProjectId = updateCommand.ProjectId;
            MarketAreaName = updateCommand.MarketAreaName;
            ProjectName = updateCommand.ProjectName;
            OrganizationUnitId = updateCommand.OrganizationUnitId;
            OrganizationUnitCode = updateCommand.OrganizationUnitCode;
            OrganizationUnitName = updateCommand.OrganizationUnitName;
            SignedUserId = updateCommand.SignedUserId;
            SignedUserName = updateCommand.SignedUserName;

            AgentId = updateCommand.AgentId;
            AgentCode = updateCommand.AgentCode;
            AgentContractCode = updateCommand.AgentContractCode;

            CurrencyUnitId = updateCommand.CurrencyUnitId;
            CurrencyUnitCode = updateCommand.CurrencyUnitCode;

            IsIncidentControl = updateCommand.IsIncidentControl;
            IsControlUsageCapacity = updateCommand.IsControlUsageCapacity;
            TimeLine = updateCommand.TimeLine;
            FiberNodeInfo = updateCommand.FiberNodeInfo;
            ContractNote = updateCommand.ContractNote;
            IsAutomaticGenerateReceipt = updateCommand.IsAutomaticGenerateReceipt;
            CustomerCareStaffUserId = updateCommand.CustomerCareStaffUserId;
            NumberBillingLimitDays = updateCommand.NumberBillingLimitDays;

            CashierUserId = updateCommand.CashierUserId;
            CashierUserName = updateCommand.CashierUserName;
            CashierFullName = updateCommand.CashierFullName;

            InterestOnDefferedPayment = updateCommand.InterestOnDefferedPayment;
            ContractViolation = updateCommand.ContractViolation;
            ContractViolationType = updateCommand.ContractViolationType;

            AutoRenew = updateCommand.AutoRenew;
            AccountingCustomerCode = updateCommand.AccountingCustomerCode;

            SetPaymentMethod(updateCommand.Payment);
        }

        public void SetPaymentMethod(PaymentMethod paymentMethod)
        {
            this.Payment = paymentMethod;

            if (this.ServicePackages.Any())
            {
                foreach (var srvPackage in this.ServicePackages)
                {
                    srvPackage.SetPaymentForm(this.Payment.Form);
                }
            }
        }

        public void UpdateStatus(int statusId)
        {
            if (ContractStatus.Signed.Is(statusId))
            {
                SetSignedStatus();
            }
            else if (ContractStatus.Liquidated.Is(statusId))
            {
                SetLiquidatedStatus();
            }
            else
            {
                SetDraftStatus();
            }
        }

        public void SetContractor(int contractorId)
        {
            ContractorId = contractorId;
        }

        public void SetContractorHTC(int contractorHTCId)
        {
            ContractorHTCId = contractorHTCId;
        }

        public void SetTaxValue(float taxValue)
        {
            if (taxValue < 0 || taxValue >= 100)
            {
                throw new ContractDomainException("Tax percent is not valid");
            }

            TotalTaxPercent = taxValue;
        }

        public void AddContractSharingRevenue(CUContractSharingRevenueLineCommand cUContractSharingRevenueCommand)
        {
            cUContractSharingRevenueCommand.OutContractId = this.Id;
            var newContractSharingRevenue = new ContractSharingRevenueLine(cUContractSharingRevenueCommand)
            {
                CreatedBy = this.CreatedBy,
                CreatedDate = this.CreatedDate
            };
            _contractSharingRevenues.Add(newContractSharingRevenue);
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

        /// <summary>
        /// Add new service package into the output contract
        /// </summary>
        /// <returns></returns>
        public OutContractServicePackage AddServicePackage(CUOutContractChannelCommand addChannelCmd, bool forceBind = false)
        {
            addChannelCmd.OutContractId = Id;
            addChannelCmd.TimeLine.PaymentForm = this.Payment.Form;

            var newChannel = new OutContractServicePackage(addChannelCmd, forceBind);
            newChannel.ChannelIndex = ServicePackages.Count;
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
            channelEntity.CalculateTotal();
            this.DetectHaveEquipment();
            this.DetectContractProjectId();
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
            cUContactInfoCommand.OutContractId = Id;
            var newContactInfo = new ContactInfo(cUContactInfoCommand);
            _contactInfos.Add(newContactInfo);

            return newContactInfo;
        }

        public void ClearContactInfos()
        {
            _contactInfos.Clear();
        }

        public void SetDraftStatus()
        {
            ContractStatusId = ContractStatus.Draft.Id;
        }

        public void SetSignedStatus()
        {
            if (ContractStatusId != ContractStatus.Draft.Id
                && ContractStatusId != ContractStatus.Signed.Id)
            {
                StatusChangeException(ContractStatus.Signed);
                return;
            }

            if (ContractStatusId != ContractStatus.Signed.Id)
            {
                ContractStatusId = ContractStatus.Signed.Id;
            }
        }

        public void SetLiquidatedStatus()
        {
            if (ContractStatusId == ContractStatus.Signed.Id)
            {
                StatusChangeException(ContractStatus.Liquidated);
            }

            ContractStatusId = ContractStatus.Liquidated.Id;
            Description = "Hợp đồng đã bị thanh lý.";
            //AddDomainEvent(new OrderShippedDomainEvent(this));
        }

        public void SetCancelledStatus()
        {
            ContractStatusId = ContractStatus.Cancelled.Id;
            Description = "Hợp đồng đã bị hủy.";
            //AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        public void SetTerminatedStatus()
        {
            ContractStatusId = ContractStatus.Liquidated.Id;
            Description = "Hợp đồng đã bị chấm dứt.";
            //AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        private void StatusChangeException(ContractStatus orderStatusToChange)
        {
            throw new ContractDomainException(
                $"Không thể chuyển trạng thái hợp đồng từ {ContractStatus?.Name ?? ContractStatus.Draft.Name} sang {orderStatusToChange.Name}.");
        }

        protected ContractTotalByCurrency CreateNewContractTotal(int? currencyUnitId = null, string currencyUnitCode = "")
        {
            var newContractTotal = new ContractTotalByCurrency()
            {
                OutContractId = this.Id,
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

                if (this.ContractTotal == null) this.CreateNewContractTotal();

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