using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.IntegrationEventCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using EventBus.Abstractions;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class TerminateServicePackagesIntegrationEventHandler : IIntegrationEventHandler<TerminateServicePackagesIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IWrappedConfigAndMapper _wrappedConfigAndMapper;

        public TerminateServicePackagesIntegrationEventHandler(IMediator mediator, IWrappedConfigAndMapper wrappedConfigAndMapper)
        {
            _mediator = mediator;
            _wrappedConfigAndMapper = wrappedConfigAndMapper;
        }

        public async Task Handle(TerminateServicePackagesIntegrationEvent @event)
        {
            if (@event == null || !@event.OutContractServicePackageIds.Any())
            {
                return;
            }

            var terminateServicePackagesIntegrationEventCommand = @event.MapTo<TerminateServicePackagesIntegrationEventCommand>(_wrappedConfigAndMapper.MapperConfig);
            await _mediator.Send(terminateServicePackagesIntegrationEventCommand);
        }
    }
}
