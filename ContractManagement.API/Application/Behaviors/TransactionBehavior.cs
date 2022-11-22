using System;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.Infrastructure;
using EventBus.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace ContractManagement.API.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IActionResponse, new()
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly ContractDbContext _dbContext;
        private readonly IContractIntegrationEventService _contractIntegrationEventServices;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger,
            ContractDbContext dbContext,
            IContractIntegrationEventService contractIntegrationEventServices)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contractIntegrationEventServices = contractIntegrationEventServices;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        response = await next();
                        transactionId = transaction.TransactionId;
                        var resultType = response?.GetType();
                        if (response is IActionResponse actionResponse && !actionResponse.IsSuccess)
                        {
                            _dbContext.RollbackTransaction();
                            transactionId = Guid.Empty;
                        }
                        else
                        {
                            await _dbContext.CommitTransactionAsync(transaction);
                        }
                    }

                    if (transactionId != Guid.Empty)
                    {
                        await _contractIntegrationEventServices.PublishEventsThroughEventBusAsync(transactionId);
                    }
                });

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}