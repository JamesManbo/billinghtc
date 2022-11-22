using News.API.Models.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Validations
{
    public class ArticleValidator : AbstractValidator<ArticleDto>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề bài viết là bắt buộc");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Nội dung bài viết là bắt buộc");
            RuleFor(x => x.ArticleCategories)
                .NotEmpty().WithMessage("Danh mục bài viết là bắt buộc");
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Danh mục bài viết nhỏ hơn 2000 ký tự");
        }
    }
}
