using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.ContractContentCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public abstract class ContractAbstraction : Entity, IAggregateRoot
    {
        [StringLength(68)]
        public string IdentityGuid { get; set; }
        [StringLength(256)]
        public string ContractCode { get; set; }
        [StringLength(256)] public string AgentId { get; set; }
        [StringLength(256)] public string AgentCode { get; set; }
        public int? MarketAreaId { get; set; }
        [StringLength(256)]
        public string MarketAreaName { get; set; }
        [StringLength(128)]
        public string CityId { get; set; }
        [StringLength(256)]
        public string CityName { get; set; }
        [StringLength(128)]
        public string DistrictId { get; set; }
        [StringLength(256)]
        public string DistrictName { get; set; }
        public int? ProjectId { get; set; }
        [StringLength(256)]
        public string ProjectName { get; set; }
        public int? ContractTypeId { get; set; }
        public int ContractStatusId { get; set; }
        public int? ContractorId { get; set; }
        public int? ContractorHTCId { get; set; }
        public string SignedUserId { get; set; }
        [StringLength(256)]
        public string SignedUserName { get; set; }
        [StringLength(256)]
        public string OrganizationUnitName { get; set; }
        [StringLength(256)]
        public string OrganizationUnitCode { get; set; }
        public int? SalesmanId { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        public PaymentMethod Payment { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public string InvoicingAddress { get; set; } // Địa chỉ xuất hóa đơn
        public float TotalTaxPercent { get; set; }

        public bool IsIncidentControl { get; set; }

        public bool IsControlUsageCapacity { get; set; }
        public int NumberBillingLimitDays { get; set; }

        public int? InterestOnDefferedPayment { get; set; }
        public int? ContractViolation { get; set; }
        public int? ContractViolationType { get; set; }
        public bool AutoRenew { get; set; } = false;

        public virtual Contractor Contractor { get; set; }
        public virtual Contractor ContractorHTC { get; set; }
        public virtual ContractStatus ContractStatus { get; set; }
        public virtual ContractContent ContractContent { get; set; }

        public int CurrencyUnitId { get; set; }
        [StringLength(256)]
        public string CurrencyUnitCode { get; set; }

        public bool IsHasOneCurrency { get; set; }
        public string AccountingCustomerCode { get; set; }

        public IReadOnlyCollection<OutContractServicePackage> ServicePackages => _servicePackages.Where(s => !s.IsDeleted).ToList();
        protected List<OutContractServicePackage> _servicePackages;

        public IReadOnlyCollection<OutContractServicePackage> ActiveServicePackages
            => _servicePackages.Where(o => o.StatusId != OutContractServicePackageStatus.Terminate.Id &&
                    o.StatusId != OutContractServicePackageStatus.Replaced.Id &&
                    o.StatusId != OutContractServicePackageStatus.UpgradeBandwidths.Id &&
                    !o.IsDeleted)
                .ToList();

        public IReadOnlyCollection<ContractSharingRevenueLine> ContractSharingRevenues => _contractSharingRevenues.Where(s => !s.IsDeleted).ToList();
        protected List<ContractSharingRevenueLine> _contractSharingRevenues;
        public ContractTotalByCurrency ContractTotal => ContractTotalByCurrencies.FirstOrDefault();
        public IReadOnlyCollection<ContractTotalByCurrency> ContractTotalByCurrencies => this._contractTotalByCurrencies;
        protected List<ContractTotalByCurrency> _contractTotalByCurrencies { get; set; }
        public bool HaveEquipment { get; set; }

        protected void DetectHaveEquipment()
        {
            this.HaveEquipment = this.ServicePackages != null &&
            this.ServicePackages.Any(s => (s.HasStartAndEndPoint && s.StartPoint.Equipments.Count > 0) ||
                s.EndPoint.Equipments.Count > 0);
        }

        private void StatusChangeException(ContractStatus orderStatusToChange)
        {
            throw new ContractDomainException($"Is not possible to change the order status from {ContractStatus.Name} to {orderStatusToChange.Name}.");
        }

        public abstract void CalculateTotal();

        public void AddOrUpdateContractContent(CUContractContentCommand contractContentCommand)
        {
            if (contractContentCommand.Id == 0)
            {
                ContractContent = new ContractContent(contractContentCommand);
            }
            else
            {
                ContractContent.UpdatedDate = DateTime.Now;
                ContractContent.UpdatedBy = this.UpdatedBy;
                ContractContent.DigitalSignatureId = contractContentCommand.DigitalSignatureId;
                ContractContent.Content = contractContentCommand.Content;
                ContractContent.ContractFormId = contractContentCommand.ContractFormId;
            }
        }

        public void RemoveContractSharingRevenues(int[] removeIds)
        {
            _contractSharingRevenues.RemoveAll(e => removeIds.Contains(e.Id));
        }

        public void ClearInContractSharingRevenues()
        {
            _contractSharingRevenues.Clear();
        }
    }
}
