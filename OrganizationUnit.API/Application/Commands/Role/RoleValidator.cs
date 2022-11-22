using FluentValidation;

namespace OrganizationUnit.API.Application.Commands.Role
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(role => role.RoleName).NotNull().WithMessage("Tên vai trò người dùng là bắt buộc").NotEmpty().WithMessage("Tên vai trò người dùng là bắt buộc");
            RuleFor(role => role.RoleName).MinimumLength(3).WithMessage("Tên vai trò quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(role => role.RoleName).MaximumLength(256).WithMessage("Tên vai trò quá dài(tối đa 256 ký tự)");
            RuleFor(role => role.RoleCode).NotNull().WithMessage("Mã vai trò người dùng là bắt buộc").NotEmpty().WithMessage("Mã vai trò người dùng là bắt buộc");
            RuleFor(role => role.RoleCode).MinimumLength(3).WithMessage("Mã vai trò quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(role => role.RoleCode).MaximumLength(256).WithMessage("Mã vai trò quá dài(tối đa 256 ký tự)");
            RuleFor(role => role.RoleCode).Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(role => role.RoleDescription).MaximumLength(1000).WithMessage("Mô tả vai trò quá dài(tối đa 1000 ký tự)");
        }
    }
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator()
        {
            RuleFor(role => role.Id).NotNull().NotEmpty().GreaterThan(0).WithMessage("Vai trò người dùng không tồn tại"); 
            RuleFor(role => role.RoleName).NotNull().NotEmpty().WithMessage("Tên vai trò người dùng là bắt buộc");
            RuleFor(role => role.RoleName).MinimumLength(3).WithMessage("Tên vai trò quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(role => role.RoleName).MaximumLength(256).WithMessage("Tên vai trò quá dài(tối đa 256 ký tự)");
            RuleFor(role => role.RoleCode).NotNull().NotEmpty().WithMessage("Mã vai trò người dùng là bắt buộc");
            RuleFor(role => role.RoleCode).MinimumLength(3).WithMessage("Mã vai trò quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(role => role.RoleCode).MaximumLength(256).WithMessage("Mã vai trò quá dài(tối đa 256 ký tự)");
            RuleFor(role => role.RoleCode).Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(role => role.RoleDescription).MaximumLength(1000).WithMessage("Mô tả vai trò quá dài(tối đa 1000 ký tự)");
        }
    }
}
