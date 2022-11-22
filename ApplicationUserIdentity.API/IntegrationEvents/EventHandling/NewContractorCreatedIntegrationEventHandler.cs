using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.IntegrationEvents.EventModels;
using ApplicationUserIdentity.API.Models;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace ApplicationUserIdentity.API.IntegrationEvents.EventHandling
{
    public class NewContractorCreatedIntegrationEventHandler : IIntegrationEventHandler<NewContractorCreatedIntegrationEvent>
    {
        private readonly ILogger<NewContractorCreatedIntegrationEventHandler> _logger;
        private readonly IUserRepository _accountRepository;

        public NewContractorCreatedIntegrationEventHandler(ILogger<NewContractorCreatedIntegrationEventHandler> logger, IUserRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task Handle(NewContractorCreatedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - {@IntegrationEvent}",
                    @event.Id, Program.AppName, @event);
                try
                {
                    var newApplicationUser = new ApplicationUser()
                    {
                        UserName = $"GUEST-{@event.IdentityGuid}",
                        Address = @event.Address,
                        CreatedDate = DateTime.Now,
                        FullName = $"GUEST",
                        District = @event.District,
                        Province = @event.Province,
                        Password = string.Empty,
                        PasswordSalt = Guid.NewGuid().ToString()
                    };

                    var createResponse = await _accountRepository.CreateAndSave(newApplicationUser);

                    if (!createResponse.IsSuccess)
                    {
                        _logger.LogError("Create new ApplicationUser failed - {Message}", createResponse.Message);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Create new ApplicationUser failed - {e}", e);
                }
            }
        }
    }
}
