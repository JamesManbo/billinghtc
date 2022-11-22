using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Extensions;
using FluentValidation;
using Global.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IActionResponse, new()
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehavior(IValidator<TRequest>[] validators,
            ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            failures = failures.GroupBy(x => x.PropertyName)
                .Select(v => v.First())
                .ToList();

            if (failures.Any())
            {
                var actionResponse = new TResponse();
                actionResponse.AddValidationResults(failures);
                return actionResponse;
            }

            return await next();
        }
    }
}