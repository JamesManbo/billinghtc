using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler
{
    public class TransactionValidator : AbstractValidator<CUTransactionCommandHandler>
    {
        public TransactionValidator()
        {

        }
    }
}
