using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Events;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace ContractManagement.API.Application.DomainEventHandlers.ContractEventHandlers
{
    public class SignedContractStartedDomainEventHandler : IRequestHandler<SignedContractStartedDomainEvent, ActionResponse>
    {
        private readonly IContractIntegrationEventService _integrationEventLogService;
        private readonly IContractorQueries _contractorQueires;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IContractSrvPckRepository _contractSrvPckRepository;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IMediator _mediator;

        public SignedContractStartedDomainEventHandler(IContractIntegrationEventService integrationEventLogService,
            IContractorQueries contractorQueires,
            IMarketAreaQueries marketAreaQueries,
            IMediator mediator,
            IContractSrvPckRepository contractSrvPckRepository,
            ITransactionQueries transactionQueries)
        {
            _integrationEventLogService = integrationEventLogService ?? throw new ArgumentNullException(nameof(integrationEventLogService));
            _contractorQueires = contractorQueires;
            _marketAreaQueries = marketAreaQueries;
            _mediator = mediator;
            _contractSrvPckRepository = contractSrvPckRepository;
            this._transactionQueries = transactionQueries;
        }

        public async Task<ActionResponse> Handle(SignedContractStartedDomainEvent request, CancellationToken cancellationToken)
        {
            var response = new ActionResponse();

            if (request.Contract.Contractor == null)
            {
                request.Contract.Contractor = _contractorQueires.FindById(request.Contract.ContractorId);
            }

            var newChannels = request.Contract.ActiveServicePackages.Where(
                    p => !p.TimeLine.Effective.HasValue &&
                        (request.ExcludeChannelIds == null ||
                        !request.ExcludeChannelIds.Contains(p.Id)
                        ))
                .ToList();

            if (newChannels.Count > 0)
            {
                /// Thêm mới giao dịch triển khai hợp đồng mới
                foreach (var channel in newChannels)
                {
                    if (channel.PaymentTarget == null)
                    {
                        channel.PaymentTarget = _contractorQueires.FindById(channel.PaymentTargetId);
                    }

                    var transactionDeployCommand = new CreateDeployNewOutContractCommand
                    {
                        TransactionDate = DateTime.UtcNow,
                        OutContractId = request.Contract.Id,
                        ContractCode = request.Contract.ContractCode,
                        ProjectId = channel.ProjectId,
                        MarketAreaId = request.Contract.MarketAreaId,
                        MarketAreaName = request.Contract.MarketAreaName,
                        IsTechnicalConfirmation = channel.IsTechnicalConfirmation,
                        IsSupplierConfirmation = channel.IsSupplierConfirmation,
                        ContractType = request.Contract.ContractTypeId,
                        ContractorId = request.Contract.ContractorId,
                        Note = request.Contract.ContractNote,
                        HasEquipment = (channel.StartPoint != null && channel.StartPoint.Equipments.Any())
                            || channel.EndPoint.Equipments.Any(),
                        Contractor = request.Contract.Contractor
                    };

                    transactionDeployCommand.Code = this._transactionQueries
                        .GenerateTransactionCode(transactionDeployCommand.ContractCode, false);

                    var transactionChannel = new CUTransactionServicePackageCommand();
                    transactionChannel.Binding(channel);
                    transactionChannel.OutContractServicePackageId = channel.Id;
                    transactionDeployCommand.TransactionServicePackages.Add(transactionChannel);

                    var saveDeployChannelTransaction = await _mediator.Send(transactionDeployCommand);
                    if (!saveDeployChannelTransaction.IsSuccess)
                    {
                        if (saveDeployChannelTransaction.Errors.Any(c => c.MemberName == "Code"))
                        {
                            bool endRepairTransCodeLoop = false;
                            var transactionIndex = this._transactionQueries.GetOrderNumberByContractCode(transactionDeployCommand.ContractCode, false);
                            transactionIndex = transactionIndex + 1;
                            do
                            {
                                transactionDeployCommand.Code = $"TS{transactionIndex:D2}_{transactionDeployCommand.ContractCode}";

                                var retryResponse = await _mediator.Send(transactionDeployCommand);
                                if (retryResponse.IsSuccess)
                                {
                                    endRepairTransCodeLoop = true;
                                }
                                else
                                {
                                    transactionIndex++;
                                }
                            } while (!endRepairTransCodeLoop);
                        }
                        else
                        {
                            response.CombineResponse(saveDeployChannelTransaction);
                            return response;
                        }
                    }
                }

                var radiusChannels = newChannels
                    .Where(c => !string.IsNullOrEmpty(c.RadiusAccount));

                if (radiusChannels.Any())
                {
                    var duplicateRadiusUsers = radiusChannels
                        .GroupBy(n => n.RadiusAccount.Trim().ToLower())
                        .Where(g => g.Count() > 1);

                    if (duplicateRadiusUsers.Any())
                    {
                        var duplicateChannels = duplicateRadiusUsers
                            .First()
                            .Select(c => string.IsNullOrEmpty(c.CId) ? c.ServiceName : c.CId);

                        return ActionResponse.Failed($"Tài khoản Radius \"{duplicateRadiusUsers.First().Key}\"" +
                            $" khai báo bị trùng ở các kênh: {string.Join(", ", duplicateChannels)}");
                    }

                    /// Tạo tài khoản người dùng trên hệ thống Radius
                    var createRadiusAccountEvent = new CreateNewRadiusUserCommand()
                    {
                        ContractCode = request.Contract.ContractCode,
                        ContractId = request.Contract.Id,
                        OutContractServicePackages = newChannels,
                        Contractor = request.Contract.Contractor
                    };

                    response.CombineResponse(await _mediator.Send(createRadiusAccountEvent));
                    if (!response.IsSuccess)
                    {
                        return response;
                    }
                }
            }

            #region add/save contract signed integration event

            // Do not handle this event when all of package prepaid months is zero
            if (request.Contract.IsAutomaticGenerateReceipt &&
                newChannels.Any(s => s.TimeLine.PrepayPeriod > 0 &&
                    s.FlexiblePricingTypeId == FlexiblePricingType.FixedPricing.Id))
            {
                var contractSignedEvent =
                    new CreateFirstBillingReceiptIntegrationEvent(request.Contract);

                contractSignedEvent.OutContract.ServicePackages = new List<OutContractServicePackageDTO>();
                contractSignedEvent.OutContract.ActiveServicePackages = new List<OutContractServicePackageDTO>();

                contractSignedEvent.OutContract.ActiveServicePackages
                    = newChannels
                        .Where(c => c.TimeLine.PrepayPeriod > 0 &&
                                    c.FlexiblePricingTypeId == FlexiblePricingType.FixedPricing.Id)
                        .ToList();

                if (request.Contract.MarketAreaId.HasValue)
                {
                    var marketArea = _marketAreaQueries.Find(request.Contract.MarketAreaId.Value);
                    contractSignedEvent.MarketAreaCode = marketArea.MarketCode;
                }

                //if (request.Contract.ProjectId.HasValue)
                //{
                //    var project = _projectQueries.Find(request.Contract.ProjectId.Value);
                //    contractSignedEvent.ProjectCode = project?.ProjectCode;
                //}

                await _integrationEventLogService.AddAndSaveEventAsync(contractSignedEvent);
                await _contractSrvPckRepository.UpdateFirstBillingIsCreated(
                       contractSignedEvent.OutContract.ActiveServicePackages
                        .Select(a => a.Id)
                        .ToArray()
                    );
            }

            #endregion

            return response;
        }
    }
}