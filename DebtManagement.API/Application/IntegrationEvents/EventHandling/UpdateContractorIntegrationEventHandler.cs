using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Infrastructure.Repositories;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateContractorIntegrationEventHandler : IIntegrationEventHandler<UpdateContractorIntegrationEvent>
    {
        private readonly IVoucherTargetRepository _voucherTargetRepository;
        private readonly IVoucherTargetPropertyRepository _voucherTargetPropRepository;

        public UpdateContractorIntegrationEventHandler(IVoucherTargetRepository voucherTargetRepository, 
            IVoucherTargetPropertyRepository voucherTargetPropRepository)
        {
            this._voucherTargetRepository = voucherTargetRepository;
            this._voucherTargetPropRepository = voucherTargetPropRepository;
        }

        public async Task Handle(UpdateContractorIntegrationEvent @event)
        {
            VoucherTarget customer = null, partner = null;

            if (@event.Customer != null 
                && !string.IsNullOrEmpty(@event.Customer.IdentityGuid))
            {
                customer = await _voucherTargetRepository
                    .FindVoucherTargetByIdentityGuid(@event.Customer.IdentityGuid);
            }

            if (@event.Partner != null && !string.IsNullOrEmpty(@event.Partner.IdentityGuid))
            {
                partner = await _voucherTargetRepository.FindVoucherTargetByIdentityGuid(@event.Partner.IdentityGuid);
            }

            if (customer == null 
                && partner == null)
            {
                return;
            }

            if (customer != null)
            {
                customer.ApplicationUserIdentityGuid = customer.IdentityGuid;
                customer.UserIdentityGuid = @event.Partner.IdentityGuid.ToString();
                if (@event.Customer.HasUpdate)
                {
                    customer.TargetFullName = @event.Customer.FullName;
                    customer.TargetPhone = @event.Customer.MobilePhoneNo;
                    customer.TargetFax = @event.Customer.FaxNo;
                    customer.TargetEmail = @event.Customer.Email;
                    customer.TargetIdNo = @event.Customer.IdNo;
                    customer.TargetTaxIdNo = @event.Customer.TaxIdNo;

                    customer.District = @event.Customer.District;
                    customer.DistrictId = @event.Customer.DistrictId;
                    customer.City = @event.Customer.City;
                    customer.CityId = @event.Customer.CityId;
                    customer.TargetAddress = @event.Customer.Address;
                }
                _voucherTargetRepository.Update(customer);

                var vchrTargetProperty = _voucherTargetPropRepository.FindByApplicationUserId(customer.IdentityGuid);
                if (vchrTargetProperty != null)
                {
                    vchrTargetProperty.StructureId = @event.Customer.ContractorStructureId;
                    vchrTargetProperty.CategoryId = @event.Customer.ContractorCategoryId;
                    vchrTargetProperty.GroupIds = @event.Customer.ContractorGroupIds;
                    vchrTargetProperty.GroupNames = @event.Customer.ContractorGroupNames;
                    vchrTargetProperty.ClassId = @event.Customer.ContractorClassId;
                    vchrTargetProperty.TypeId = @event.Customer.ContractorTypeId;
                    vchrTargetProperty.IndustryIds = @event.Customer.ContractorIndustryIds;
                    vchrTargetProperty.StructureName = @event.Customer.ContractorStructureName;
                    vchrTargetProperty.CategoryName = @event.Customer.ContractorCategoryName;
                    vchrTargetProperty.ClassName = @event.Customer.ContractorClassName;
                    vchrTargetProperty.TypeName = @event.Customer.ContractorTypeName;
                    vchrTargetProperty.IndustryNames = @event.Customer.ContractorIndustryNames;
                    _voucherTargetPropRepository.Update(vchrTargetProperty);
                }
                else
                {
                    var newContractProps = new VoucherTargetProperty
                    {
                        TargetId = customer.Id,
                        ApplicationUserIdentityGuid = customer.ApplicationUserIdentityGuid,
                        StructureId = @event.Customer.ContractorStructureId,
                        CategoryId = @event.Customer.ContractorCategoryId,
                        GroupIds = @event.Customer.ContractorGroupIds,
                        GroupNames = @event.Customer.ContractorGroupNames,
                        ClassId = @event.Customer.ContractorClassId,
                        TypeId = @event.Customer.ContractorTypeId,
                        IndustryIds = @event.Customer.ContractorIndustryIds,
                        StructureName = @event.Customer.ContractorStructureName,
                        CategoryName = @event.Customer.ContractorCategoryName,
                        ClassName = @event.Customer.ContractorClassName,
                        TypeName = @event.Customer.ContractorTypeName,
                        IndustryNames = @event.Customer.ContractorIndustryNames
                    };
                    _voucherTargetPropRepository.Create(newContractProps);
                }

                if (!string.IsNullOrEmpty(customer.UserIdentityGuid) 
                    && customer.UserIdentityGuid != @event.Partner.IdentityGuid)
                {
                    var oldContractor = await _voucherTargetRepository.FindVoucherTargetByIdentityGuid(customer.UserIdentityGuid);
                    if (oldContractor != null)
                    {
                        oldContractor.ApplicationUserIdentityGuid = null;
                        _voucherTargetRepository.Update(oldContractor);
                    }
                }                
            }

            if (partner != null)
            {
                if (!string.IsNullOrEmpty(partner.ApplicationUserIdentityGuid)
                    && partner.ApplicationUserIdentityGuid != @event.Customer.IdentityGuid)
                {
                    var oldContractor = await _voucherTargetRepository.FindVoucherTargetByIdentityGuid(partner.ApplicationUserIdentityGuid);
                    if (oldContractor != null)
                    {
                        oldContractor.UserIdentityGuid = null;
                        _voucherTargetRepository.Update(oldContractor);
                    }
                }

                partner.UserIdentityGuid = partner.IdentityGuid;
                partner.ApplicationUserIdentityGuid = @event.Customer.IdentityGuid;
                if (@event.Partner.HasUpdate)
                {
                    partner.TargetFullName = @event.Partner.FullName;
                    partner.TargetPhone = @event.Partner.MobilePhoneNo;
                    partner.TargetFax = @event.Partner.FaxNo;
                    partner.TargetEmail = @event.Partner.Email;
                    partner.TargetIdNo = @event.Partner.IdNo;
                    partner.TargetTaxIdNo = @event.Partner.TaxIdNo;
                    partner.TargetAddress = @event.Partner.Address;
                }
                _voucherTargetRepository.Update(partner);
            }
            await _voucherTargetRepository.UpdateAccountingCodes();
            await _voucherTargetRepository.SaveChangeAsync();
        }
    }
}
