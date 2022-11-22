using News.API.Models.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Validations
{
    public class ArticleCategoryValidator : AbstractValidator<ArticleCategoryDto>
    {
        public ArticleCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên danh mục là bắt buộc");
            RuleFor(x => x.ASCII)
                .NotEmpty().WithMessage("Mã Ascii là bắt buộc");
        }
    }
}
