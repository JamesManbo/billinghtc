using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeEquipments
{
    public class CUChangeEquipmentsCommandValidator : AbstractValidator<CUChangeEquipmentsCommand>
    {
        public CUChangeEquipmentsCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages)
                .NotEmpty().WithMessage("Vui lòng chọn kênh muốn thực hiện thay thế thiết bị");

            RuleForEach(c => c.TransactionServicePackages)
                .SetValidator(new ChangeEquipmentTransactionSrvPackageValidator());
        }

        public class ChangeEquipmentTransactionSrvPackageValidator : AbstractValidator<CUTransactionServicePackageCommand>
        {
            public ChangeEquipmentTransactionSrvPackageValidator()
            {
                When(c => c.HasStartAndEndPoint, () =>
                {
                    When(c => c.StartPoint.Equipments.Count == 0, () =>
                    {
                        RuleFor(c => c.EndPoint.Equipments)
                        .NotEmpty().WithMessage(c => $"Vui lòng chọn ít nhất 01 thiết bị muốn thay thế tại kênh {c.CId}");
                    });
                })
                .Otherwise(() =>
                {
                    RuleFor(c => c.EndPoint.Equipments)
                    .NotEmpty().WithMessage(c => $"Vui lòng chọn ít nhất 01 thiết bị muốn thay thế tại kênh {c.CId}");
                });
            }
        }
    }
}
