using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.RadiusAndBrasCommandHandler
{
    public class BrasInfoValidator : AbstractValidator<CuBrasInfoCommand>
    {
        public BrasInfoValidator()
        {
            RuleFor(r => r.Name).NotNull().WithMessage("Tên là bắt buộc")
                .NotEmpty().WithMessage("Tên là bắt buộc");
            RuleFor(r => r.IP).NotNull().WithMessage("Địa chỉ IP là bắt buộc")
                .NotEmpty().WithMessage("Địa chỉ IP là bắt buộc");
            RuleFor(r => r.ManualAPIPort)
                .LessThan(65535).WithMessage("Số port không hợp lệ")
                .GreaterThan(0).WithMessage("Số port không hợp lệ");
            RuleFor(r => r.SSHPort).LessThan(65535).WithMessage("Số port không hợp lệ")
                .GreaterThan(0).WithMessage("Số port không hợp lệ");
            RuleFor(r => r.UserName).NotNull().WithMessage("Tài khoản quản trị là bắt buộc")
                .NotEmpty().WithMessage("Tài khoản quản trị là bắt buộc");
            RuleFor(r => r.Password).NotNull().WithMessage("Mật khẩu quản trị là bắt buộc")
                .NotEmpty().WithMessage("Mật khẩu quản trị là bắt buộc");
        }
    }
}
