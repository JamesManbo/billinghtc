using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.CancelledTransactions
{
    public class CancelledTransactionsValidator : AbstractValidator<CUCancelledTransactionSimplesCommand>
    {
        public CancelledTransactionsValidator()
        {
            RuleFor(c => c.TransactionIds).NotNull().WithMessage("Phụ lục là bắt buộc ");
        }
    }
}
