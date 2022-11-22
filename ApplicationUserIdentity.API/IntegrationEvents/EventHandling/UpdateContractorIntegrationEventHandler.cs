using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.IntegrationEvents.EventModels;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.IntegrationEvents.EventHandling
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
            throw new NotImplementedException();
            //var applicationUser = await _userRepository.FindByIdentityGuidAsync(@event.Customer.IdentityGuid);
            //var user = await _userRepository.FindByUserIdentityGuidAsync(@event.Partner.IdentityGuid);
            //if (applicationUser == null && user == null)
            //{
            //    return;
            //}
            //else
            //{
            //    if(user != null && user.IdentityGuid != @event.Customer.IdentityGuid)
            //    {
            //        user.IsPartner = false;
            //        user.UserIdentityGuid = string.Empty;
            //        _userRepository.Update(user);
            //    }

            //    if (applicationUser != null && applicationUser.UserIdentityGuid != @event.Partner.IdentityGuid)
            //    {
            //        applicationUser.IsPartner = !string.IsNullOrWhiteSpace(@event.Partner.IdentityGuid);
            //        applicationUser.UserIdentityGuid = @event.Partner.IdentityGuid;
            //        _userRepository.Update(applicationUser);
            //    }

            //    await _userRepository.SaveChangeAsync();
            //}
        }
    }
}
