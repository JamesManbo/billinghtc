using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Application.Commands.OrganizationUnit
{
    public class CreateOrganizationUnitValidator : AbstractValidator<CreateOrganizationUnitCommand>
    {
        public CreateOrganizationUnitValidator()
        {
            RuleFor(organizationUnit => organizationUnit.Name)
                .NotNull().WithMessage("Tên đơn vị là bắt buộc")
                .NotEmpty().WithMessage("Tên đơn vị là bắt buộc ");
            RuleFor(organizationUnit => organizationUnit.Name)
                .MaximumLength(256).WithMessage("Tên đơn vị quá dài (tối đa 256 ký tự)");
            RuleFor(organizationUnit => organizationUnit.Code)
                .NotNull().WithMessage("Mã đơn vị là bắt buộc")
                .NotEmpty().WithMessage("Mã đơn vị là bắt buộc");
            RuleFor(organizationUnit => organizationUnit.Code)
                .MaximumLength(256).WithMessage("Mã đơn vị quá dài (tối đa 256 ký tự)");
            RuleFor(organizationUnit => organizationUnit.Code)
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                .WithMessage("Mã đơn vị chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");

            When(organizationUnit => organizationUnit.NumberPhone != null && organizationUnit.NumberPhone.Length > 0, () =>
            {
                RuleFor(organizationUnit => organizationUnit.NumberPhone).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
            });

            When(organizationUnit => organizationUnit.Email != null && organizationUnit.Email.Length > 0, () =>
            {
                RuleFor(organizationUnit => organizationUnit.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
            });

            //RuleFor(organizationUnit => organizationUnit.NumberPhone).NotNull().WithMessage("Số điện thoại là bắt buộc").NotEmpty().WithMessage("Số điện thoại là bắt buộc");
            //RuleFor(organizationUnit => organizationUnit.NumberPhone).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
            //RuleFor(organizationUnit => organizationUnit.Email).NotNull().WithMessage("Địa chỉ email là bắt buộc").NotEmpty().WithMessage("Địa chỉ email là bắt buộc");
            //RuleFor(organizationUnit => organizationUnit.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
        }
    }
    public class UpdateOrganizationUnitValidator : AbstractValidator<UpdateOrganizationUnitCommand>
    {
        public UpdateOrganizationUnitValidator()
        {
            RuleFor(organizationUnit => organizationUnit.Id)
                .NotNull().NotEmpty()
                .GreaterThan(0).WithMessage("Đơn vị không tồn tại");

            RuleFor(organizationUnit => organizationUnit.Name)
                .NotNull().WithMessage("Tên đơn vị là bắt buộc")
                .NotEmpty().WithMessage("Tên đơn vị là bắt buộc ");

            RuleFor(organizationUnit => organizationUnit.Name)
                .MaximumLength(256).WithMessage("Tên đơn vị quá dài (tối đa 256 ký tự)");
            RuleFor(organizationUnit => organizationUnit.Code)
                .NotNull().WithMessage("Mã đơn vị là bắt buộc")
                .NotEmpty().WithMessage("Mã đơn vị là bắt buộc");
            RuleFor(organizationUnit => organizationUnit.Code)
                .MaximumLength(256).WithMessage("Mã đơn vị quá dài (tối đa 256 ký tự)");
            RuleFor(organizationUnit => organizationUnit.Code)
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                .WithMessage("Mã đơn vị chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");

            When(organizationUnit => organizationUnit.NumberPhone != null && organizationUnit.NumberPhone.Length > 0, () =>
            {
                RuleFor(organizationUnit => organizationUnit.NumberPhone).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
            });

            When(organizationUnit => organizationUnit.Email != null && organizationUnit.Email.Length > 0, () =>
            {
                RuleFor(organizationUnit => organizationUnit.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
            });

            //RuleFor(organizationUnit => organizationUnit.NumberPhone).NotNull().WithMessage("Số điện thoại là bắt buộc").NotEmpty().WithMessage("Số điện thoại là bắt buộc");
            //RuleFor(organizationUnit => organizationUnit.NumberPhone).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
            //RuleFor(organizationUnit => organizationUnit.Email).NotNull().WithMessage("Địa chỉ email là bắt buộc").NotEmpty().WithMessage("Địa chỉ email là bắt buộc");
            //RuleFor(organizationUnit => organizationUnit.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
        }
    }
}
