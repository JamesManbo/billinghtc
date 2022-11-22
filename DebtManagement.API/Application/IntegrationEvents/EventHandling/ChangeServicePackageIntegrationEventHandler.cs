using AutoMapper;
using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.ContractEventsCommand;
using DebtManagement.Domain.Commands.IntegrationEventCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Events.ContractEvents;
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
    public class ChangeServicePackageIntegrationEventHandler : IIntegrationEventHandler<ChangeServicePackageIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly IWrappedConfigAndMapper _wrappedConfigAndMapper;

        public ChangeServicePackageIntegrationEventHandler(IMediator mediator,
            IWrappedConfigAndMapper wrappedConfigAndMapper)
        {
            _mediator = mediator;
            _wrappedConfigAndMapper = wrappedConfigAndMapper;
        }

        public async Task Handle(ChangeServicePackageIntegrationEvent @event)
        {
            if (@event == null)
            {
                return;
            }

            var changeServicePackageIntegrationEventCommand = @event.MapTo<ChangeServicePackageIntegrationEventCommand>(_wrappedConfigAndMapper.MapperConfig);
            await _mediator.Send(changeServicePackageIntegrationEventCommand);
        }
    }
}
