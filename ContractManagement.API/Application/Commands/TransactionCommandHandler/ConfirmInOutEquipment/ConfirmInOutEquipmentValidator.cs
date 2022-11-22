using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ConfirmInOutEquipment
{
    public class ConfirmInOutEquipmentValidator : AbstractValidator<CUTransactionCommand>
    {
        public ConfirmInOutEquipmentValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0).WithMessage("Phụ lục là bắt buộc");
        }
    }
}
