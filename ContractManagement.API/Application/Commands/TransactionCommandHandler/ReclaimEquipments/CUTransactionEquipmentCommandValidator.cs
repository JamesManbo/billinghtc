using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ReclaimEquipments
{
    public class CUTransactionEquipmentCommandValidator : AbstractValidator<CUTransactionEquipmentCommand>
    {
        public CUTransactionEquipmentCommandValidator()
        {
            RuleFor(c => c.ContractEquipmentId).GreaterThan(0).WithMessage("Thiết bị hợp đồng đầu ra là bắt buộc");
            RuleFor(c => c.EquipmentId).GreaterThan(0).WithMessage("Thiết bị là bắt buộc");
            RuleFor(c => c.EquipmentName).NotEmpty().WithMessage("Tên thiết bị là bắt buộc");
            RuleFor(c => c.ReclaimedUnit).GreaterThan(0).WithMessage("Số lượng thu hồi là bắt buộc");
        }
    }
}
