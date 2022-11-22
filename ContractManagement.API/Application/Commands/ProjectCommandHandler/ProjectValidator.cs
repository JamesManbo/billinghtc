using ContractManagement.Domain.Commands.ProjectCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.ProjectCommandHandler
{
    public class ProjectValidator : AbstractValidator<CUProjectCommand>
    {
        public ProjectValidator()
        {
            RuleFor(project => project.ProjectName).NotEmpty().WithMessage("Tên dự án là bắt buộc");
            RuleFor(project => project.ProjectName).MinimumLength(3).WithMessage("Tên dự án quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(project => project.ProjectName).MaximumLength(256).WithMessage("Tên dự án quá dài(tối đa 256 ký tự)");
            RuleFor(project => project.ProjectCode).NotEmpty().WithMessage("Mã dự án là bắt buộc");
            RuleFor(project => project.ProjectCode).MinimumLength(3).WithMessage("Mã dự án quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(project => project.ProjectCode).MaximumLength(256).WithMessage("Mã dự án quá dài(tối đa 256 ký tự)");
            RuleFor(project => project.ProjectCode).Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(project => project.CityId).NotEmpty().WithMessage("Tỉnh/TP là bắt buộc");
            RuleFor(project => project.DistrictId).NotEmpty().WithMessage("Quận/Huyện/Thị xã là bắt buộc");
        }
    }
}
