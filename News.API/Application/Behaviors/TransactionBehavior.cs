using System;
using System.Threading;
using System.Threading.Tasks;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using News.API.Infrastructure;
using Serilog.Context;

namespace News.API.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse :  IActionResponse, new()
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly NewsDbContext _dbContext;
        //private readonly IContractIntegrationEventService _contractIntegrationEventServices;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger,
            NewsDbContext dbContext
            //IContractIntegrationEventService billingIntegrationEventServices
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            //_contractIntegrationEventServices = billingIntegrationEventServices;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            //var typeName = request.GetGenericTypeName();

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
                        //_logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})",
                        //    transaction.TransactionId, typeName, request);
                        //response = await next();
                        //_logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}",
                        //    transaction.TransactionId, typeName);

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

                    //await _contractIntegrationEventServices.PublishEventsThroughEventBusAsync(transactionId);
                });

                return response;
            }
            catch (Exception e)
            {
                //_logger.LogError(e, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}