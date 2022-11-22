using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.RadiusAndBrasCommandHandler
{
    public class RadiusServerInfoValidator : AbstractValidator<CuRadiusServerInfoCommand>
    {
        public RadiusServerInfoValidator()
        {
            RuleFor(r => r.ServerName).NotNull().WithMessage("Tên server Radius là bắt buộc")
                .NotEmpty().WithMessage("Tên server Radius là bắt buộc");
            RuleFor(r => r.IP).NotNull().WithMessage("Địa chỉ IP là bắt buộc")
                .NotEmpty().WithMessage("Địa chỉ IP là bắt buộc");
            RuleFor(r => r.DatabasePort).LessThan(65535).WithMessage("Số port không hợp lệ")
                .GreaterThan(0).WithMessage("Số port không hợp lệ");
            RuleFor(r => r.DatabaseUserName).NotNull().WithMessage("Tài khoản quản trị database là bắt buộc")
                .NotEmpty().WithMessage("Tài khoản quản trị database là bắt buộc");
            RuleFor(r => r.DatabasePassword).NotNull().WithMessage("Mật khẩu quản trị database là bắt buộc")
                .NotEmpty().WithMessage("Mật khẩu quản trị database là bắt buộc");
        }
    }
}
