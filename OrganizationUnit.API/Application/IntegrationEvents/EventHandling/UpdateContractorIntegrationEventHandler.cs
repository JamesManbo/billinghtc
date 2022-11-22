using OrganizationUnit.API.Application.IntegrationEvents.Events.ApplicationUserEvents;
using EventBus.Abstractions;
using System;
using System.Threading.Tasks;
using OrganizationUnit.Infrastructure.Repositories;

namespace OrganizationUnit.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateContractorIntegrationEventHandler : IIntegrationEventHandler<UpdateContractorIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;

        public UpdateContractorIntegrationEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateContractorIntegrationEvent @event)
        {
            var user = await _userRepository.FindByIdentityGuidAsync(@event.Partner.IdentityGuid);
            var applicationUser = await _userRepository.FindByApplicationUserIdentityGuidAsync(@event.Customer.IdentityGuid);
            if (user == null && applicationUser == null)
            {
                return;
            }
            else
            {

                if (applicationUser != null && applicationUser.IdentityGuid != @event.Partner.IdentityGuid)
                {
                    applicationUser.IsCustomer = false;
                    applicationUser.ApplicationUserIdentityGuid = string.Empty;
                    _userRepository.Update(applicationUser);
                }

                if (user != null && user.ApplicationUserIdentityGuid != @event.Customer.IdentityGuid)
                {
                    user.IsCustomer = !string.IsNullOrEmpty(@event.Customer.IdentityGuid);
                    user.ApplicationUserIdentityGuid = @event.Customer.IdentityGuid;
                    _userRepository.Update(user);
                }

                await _userRepository.SaveChangeAsync();
            }

        }
    }
}
