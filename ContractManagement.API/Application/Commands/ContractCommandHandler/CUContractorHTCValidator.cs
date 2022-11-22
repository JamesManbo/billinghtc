using ContractManagement.Domain.Commands.OutContractCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.ContractCommandHandler
{
    public class CUContractorHTCValidator : AbstractValidator<CUContractorHTCCommand>
    {
        public CUContractorHTCValidator()
        {
            RuleFor(c => c.ContractorFullName)
                .NotNull().WithMessage("Tên chủ thể là bắt buộc")
                .NotEmpty().WithMessage("Tên chủ thể là bắt buộc");

            RuleFor(c => c.ContractorCode)
                .NotNull().WithMessage("Mã chủ thể là bắt buộc")
                .NotEmpty().WithMessage("Mã chủ thể là bắt buộc")
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");

            RuleFor(c => c.ContractorAddress)
                .NotNull().WithMessage("Địa chỉ là bắt buộc")
                .NotEmpty().WithMessage("Địa chỉ là bắt buộc");

            RuleFor(x => x.ContractorPhone)
                .NotNull()
                .WithMessage("Số điện thoại liên hệ là bắt buộc")
                .NotEmpty()
                .WithMessage("Số điện thoại liên hệ là bắt buộc")
                .MinimumLength(10)
                .WithMessage("Số điện thoại liên hệ không hợp lệ");
        }
    }
}
