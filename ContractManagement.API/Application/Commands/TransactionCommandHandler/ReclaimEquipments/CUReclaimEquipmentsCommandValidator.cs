using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ReclaimEquipments
{
    public class CUReclaimEquipmentsCommandValidator : AbstractValidator<CUReclaimEquipmentsCommand>
    {
        public CUReclaimEquipmentsCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages)
                .NotEmpty().WithMessage("Vui lòng chọn kênh muốn thực hiện thu hồi thiết bị");

            RuleForEach(c => c.TransactionServicePackages)
                .SetValidator(new ReclaimEquipmentTransactionSrvPackageValidator());
        }
        public class ReclaimEquipmentTransactionSrvPackageValidator : AbstractValidator<CUTransactionServicePackageCommand>
        {
            public ReclaimEquipmentTransactionSrvPackageValidator()
            {
                When(c => c.HasStartAndEndPoint, () =>
                {
                    When(c => c.StartPoint.Equipments.Count == 0, () =>
                    {
                        RuleFor(c => c.EndPoint.Equipments)
                        .NotEmpty().WithMessage(c => $"Vui lòng chọn ít nhất 01 thiết bị muốn thu hồi tại kênh {c.CId}");
                    });
                })
                .Otherwise(() =>
                {
                    RuleFor(c => c.EndPoint.Equipments)
                    .NotEmpty().WithMessage(c => $"Vui lòng chọn ít nhất 01 thiết bị muốn thu hồi tại kênh {c.CId}");
                });
            }
        }
    }
}
