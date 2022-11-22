using ContractManagement.Domain.Commands.ContractFormCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.ContractFormCommandHandler
{
    public class CUContractFormCommandValidator : AbstractValidator<CUContractFormCommand>
    {
        public CUContractFormCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Tên mẫu hợp đồng là bắt buộc");

            //RuleFor(c => c.ServiceId)
            //    .GreaterThan(0).WithMessage("Dịch vụ của mẫu hợp đồng là bắt buộc");

            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Nội dung mẫu hợp đồng là bắt buộc");

            RuleFor(c => c.DigitalSignature)
                .NotNull()
                .WithMessage("Chữ ký dại diện bên cung cấp dịch vụ là bắt buộc");

            When(c => c.DigitalSignature != null && c.DigitalSignature.Id == 0, () =>
            {
                RuleFor(c => c.DigitalSignature.TemporaryUrl)
                .NotEmpty().WithMessage("Chữ ký dại diện bên cung cấp dịch vụ là bắt buộc");
            });
        }
    }
}
