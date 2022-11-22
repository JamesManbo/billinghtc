using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ApprovedTransactions
{
    public class ApprovedTransactionsValidator : AbstractValidator<CUApprovedTransactionSimplesCommand>
    {
        public ApprovedTransactionsValidator()
        {
            RuleFor(c => c.TransactionSimpleCommands).NotNull().WithMessage("Phụ lục là bắt buộc ");
        }
    }
}
