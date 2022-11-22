using ContractManagement.Domain.Commands.TaxCategoryCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.TaxCategoryCommandHandler
{
    public class CreateTaxCategoryValidator : AbstractValidator<CUTaxCategoryCommand>
    {
        public CreateTaxCategoryValidator()
        {
            RuleFor(tax => tax.TaxName).NotEmpty().WithMessage("Tên loại thuế là bắt buộc");
            RuleFor(tax => tax.TaxName).MinimumLength(3).WithMessage("Tên loại thuế quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(tax => tax.TaxName).MaximumLength(256).WithMessage("Tên loại thuế quá dài(tối đa 256 ký tự)");
            RuleFor(tax => tax.TaxCode).NotEmpty().WithMessage("Mã loại thuế là bắt buộc");
            RuleFor(tax => tax.TaxCode).MinimumLength(3).WithMessage("Mã loại thuế quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(tax => tax.TaxCode).MaximumLength(256).WithMessage("Mã loại thuế quá dài(tối đa 256 ký tự)");
            RuleFor(tax => tax.TaxCode).Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            //RuleFor(tax => tax.TaxValue).NotEmpty().WithMessage("Giá trị thuế là bắt buộc");
            RuleFor(tax => tax.TaxValue).GreaterThan(0).WithMessage("Giá trị thuế phải lớn hơn 0");
            RuleFor(tax => tax.TaxValue).LessThan(100).WithMessage("Giá trị thuế phải nhỏ hơn 100");
        }
    }
}
