using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpgradeEquipments
{
    public class CUUpgradeEquipmentsCommandValidator: AbstractValidator<CUUpgradeEquipmentsCommand>
    {
        public CUUpgradeEquipmentsCommandValidator()
        {
            RuleFor(c => c.TransactionEquipments).NotEmpty().WithMessage("Thiết bị là bắt buộc");
            RuleForEach(x => x.TransactionEquipments).SetValidator(new CUTransactionEquipmentCommandValidator());
            RuleFor(c => c.Type).NotEmpty().WithMessage("Loại phụ lục là bắt buộc");
            RuleFor(c => c.StatusId).NotEmpty().WithMessage("Trạng thái phụ lục là bắt buộc");
        }
    }
}
