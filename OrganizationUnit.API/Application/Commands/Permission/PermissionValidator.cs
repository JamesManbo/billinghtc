using FluentValidation;

namespace OrganizationUnit.API.Application.Commands.Permission
{
    public class CreatePermissionValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionValidator()
        {
            RuleFor(permission => permission.PermissionName).NotNull().WithMessage("Tên quyền quản trị là bắt buộc").NotEmpty().WithMessage("Tên quyền quản trị là bắt buộc ");
            RuleFor(permission => permission.PermissionName).MaximumLength(256).WithMessage("Tên quyền quản trị quá dài (tối đa 256 ký tự)");
            RuleFor(permission => permission.PermissionCode).NotNull().WithMessage("Mã quyền quản trị là bắt buộc").NotEmpty().WithMessage("Mã quyền quản trị là bắt buộc");
            RuleFor(permission => permission.PermissionCode).MaximumLength(256).WithMessage("Mã quyền quản trị quá dài (tối đa 256 ký tự)");
            RuleFor(permission => permission.PermissionCode).Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã quyền chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(permission => permission.Description).MaximumLength(1000).WithMessage("Mô tả quyền quản trị quá dài (tối đa 1000 ký tự)");
        }
    }
    public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionValidator()
        {
            RuleFor(permission => permission.Id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Quyền quản trị không tồn tại");
            RuleFor(permission => permission.PermissionName).NotNull().NotEmpty().WithMessage("Tên quyền quản trị là bắt buộc");
            RuleFor(permission => permission.PermissionName).MaximumLength(256).WithMessage("Tên quyền quản trị quá dài (tối đa 256 ký tự)");
            RuleFor(permission => permission.PermissionCode).NotNull().NotEmpty().WithMessage("Mã quyền quản trị là bắt buộc");
            RuleFor(permission => permission.PermissionCode).MaximumLength(256).WithMessage("Mã quyền quản trị quá dài (tối đa 256 ký tự)");
            RuleFor(permission => permission.PermissionCode).Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã quyền chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(permission => permission.Description).MaximumLength(1000).WithMessage("Mô tả quyền quản trị quá dài (tối đa 1000 ký tự)");
        }
    }
}
