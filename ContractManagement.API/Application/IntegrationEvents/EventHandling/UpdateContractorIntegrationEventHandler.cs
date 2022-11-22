using ContractManagement.API.Application.IntegrationEvents.Events.ApplicationUserEvents;
using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateContractorIntegrationEventHandler : IIntegrationEventHandler<UpdateContractorIntegrationEvent>
    {
        private readonly IContractorRepository _contractorRepository;
        private readonly IContractorPropertiesRepository _contractorPropertiesRepository;

        public UpdateContractorIntegrationEventHandler(IContractorRepository contractorRepository,
            IContractorPropertiesRepository contractorPropertiesRepository)
        {
            _contractorRepository = contractorRepository;
            this._contractorPropertiesRepository = contractorPropertiesRepository;
        }

        public async Task Handle(UpdateContractorIntegrationEvent @event)
        {
            Contractor customer = null, partner = null;

            if (@event.Customer != null && !string.IsNullOrEmpty(@event.Customer.IdentityGuid))
            {
                customer 
                    = await _contractorRepository.FindContractorByIdentityGuid(@event.Customer.IdentityGuid);
            }

            if (@event.Partner != null && !string.IsNullOrEmpty(@event.Partner.IdentityGuid))
            {
                partner 
                    = await _contractorRepository.FindContractorByIdentityGuid(@event.Partner.IdentityGuid);
            }

            if (customer == null 
                && partner == null)
            {
                return;
            }

            if (customer != null && @event.Customer.HasUpdate)
            {
                customer.ContractorUserName = @event.Customer.UserName;
                customer.ContractorFullName = @event.Customer.FullName;
                customer.ContractorPhone = @event.Customer.MobilePhoneNo;
                customer.ContractorFax = @event.Customer.FaxNo;
                customer.ContractorEmail = @event.Customer.Email;
                customer.ContractorIdNo = @event.Customer.IdNo;
                customer.ContractorTaxIdNo = @event.Customer.TaxIdNo;
                customer.ContractorDistrict = @event.Customer.District;
                customer.ContractorDistrictId = @event.Customer.DistrictId;
                customer.ContractorCity = @event.Customer.City;
                customer.ContractorCityId = @event.Customer.CityId;
                customer.ContractorAddress = @event.Customer.Address;
                customer.AccountingCustomerCode = @event.Customer.AccountingCustomerCode;

                var contractorProperties = _contractorPropertiesRepository.FindByApplicationUserId(customer.IdentityGuid);
                if (contractorProperties != null)
                {
                    contractorProperties.ContractorStructureId = @event.Customer.ContractorStructureId;
                    contractorProperties.ContractorCategoryId = @event.Customer.ContractorCategoryId;
                    contractorProperties.ContractorGroupIds = @event.Customer.ContractorGroupIds;
                    contractorProperties.ContractorClassId = @event.Customer.ContractorClassId;
                    contractorProperties.ContractorTypeId = @event.Customer.ContractorTypeId;
                    contractorProperties.ContractorIndustryIds = @event.Customer.ContractorIndustryIds;

                    contractorProperties.ContractorStructureName = @event.Customer.ContractorStructureName;
                    contractorProperties.ContractorCategoryName = @event.Customer.ContractorCategoryName;
                    contractorProperties.ContractorGroupNames = @event.Customer.ContractorGroupNames;
                    contractorProperties.ContractorClassName = @event.Customer.ContractorClassName;
                    contractorProperties.ContractorTypeName = @event.Customer.ContractorTypeName;
                    contractorProperties.ContractorIndustryNames = @event.Customer.ContractorIndustryNames;

                    _contractorPropertiesRepository.Update(contractorProperties);
                }
                else
                {
                    var newContractProps = new ContractorProperties
                    {
                        ContractorId = customer.Id,
                        ApplicationUserIdentityGuid = customer.ApplicationUserIdentityGuid,
                        ContractorStructureId = @event.Customer.ContractorStructureId,
                        ContractorCategoryId = @event.Customer.ContractorCategoryId,
                        ContractorGroupIds = @event.Customer.ContractorGroupIds,
                        ContractorGroupNames = @event.Customer.ContractorGroupNames,
                        ContractorClassId = @event.Customer.ContractorClassId,
                        ContractorTypeId = @event.Customer.ContractorTypeId,
                        ContractorIndustryIds = @event.Customer.ContractorIndustryIds,
                        ContractorStructureName = @event.Customer.ContractorStructureName,
                        ContractorCategoryName = @event.Customer.ContractorCategoryName,
                        ContractorClassName = @event.Customer.ContractorClassName,
                        ContractorTypeName = @event.Customer.ContractorTypeName,
                        ContractorIndustryNames = @event.Customer.ContractorIndustryNames
                    };
                    _contractorPropertiesRepository.Create(newContractProps);
                }
                _contractorRepository.Update(customer);
            }

            if (customer != null
                    && customer.UserIdentityGuid != @event.Partner.IdentityGuid)
            {
                if (!string.IsNullOrEmpty(customer.UserIdentityGuid))
                {
                    var oldContractor = await _contractorRepository
                        .FindContractorByIdentityGuid(customer.UserIdentityGuid);

                    if (oldContractor != null)
                    {
                        oldContractor.ApplicationUserIdentityGuid = null;
                        _contractorRepository.Update(oldContractor);
                    }
                }

                customer.ApplicationUserIdentityGuid = customer.IdentityGuid;
                customer.UserIdentityGuid = @event.Partner.IdentityGuid;
            }

            if(partner != null && @event.Partner.HasUpdate)
            {
                partner.UserIdentityGuid = partner.IdentityGuid;
                partner.ApplicationUserIdentityGuid = @event.Customer.IdentityGuid;
                if (@event.Partner.HasUpdate)
                {
                    partner.ContractorUserName = @event.Partner.UserName;
                    partner.ContractorFullName = @event.Partner.FullName;
                    partner.ContractorPhone = @event.Partner.MobilePhoneNo;
                    partner.ContractorFax = @event.Partner.FaxNo;
                    partner.ContractorEmail = @event.Partner.Email;
                    partner.ContractorIdNo = @event.Partner.IdNo;
                    partner.ContractorTaxIdNo = @event.Partner.TaxIdNo;
                    partner.ContractorAddress = @event.Partner.Address;
                    partner.ContractorDistrict = @event.Partner.District;
                    partner.ContractorDistrictId = @event.Partner.DistrictId;
                    partner.ContractorCity = @event.Partner.City;
                    partner.ContractorCityId = @event.Partner.CityId;
                }
                _contractorRepository.Update(partner);
            }

            if (partner != null && partner.ApplicationUserIdentityGuid != @event.Customer.IdentityGuid)
            {
                if (!string.IsNullOrEmpty(partner.ApplicationUserIdentityGuid))
                {
                    var oldContractor = await _contractorRepository.FindContractorByIdentityGuid(partner.ApplicationUserIdentityGuid);
                    if (oldContractor != null)
                    {
                        oldContractor.UserIdentityGuid = null;
                        _contractorRepository.Update(oldContractor);
                    }
                }
            }

            await _contractorRepository.SaveChangeAsync();
        }
    }
}
