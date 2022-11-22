using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using FluentValidation;

namespace ApplicationUserIdentity.API.Infrastructure.Validations
{
    public class ApplicationUserValidator : AbstractValidator<UserViewModel>
    {
        public ApplicationUserValidator(bool isEnterpriseValidation)
        {
            //RuleFor(x => x.Avatar).NotNull().WithMessage("Ảnh avatar là bắt buộc");

            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(isEnterpriseValidation
                    ? "Tên doanh nghiệp là bắt buộc"
                    : "Họ & tên khách hàng là bắt buộc")
                .MinimumLength(2)
                .WithMessage(isEnterpriseValidation ? "Tên doanh nghiệp quá ngắn" : "Họ & tên khách hàng quá ngắn")
                .MaximumLength(256)
                .WithMessage(isEnterpriseValidation ? "Tên doanh nghiệp quá dài" : "Họ & tên người dùng quá dài");

            RuleFor(x => x.CustomerCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(isEnterpriseValidation
                    ? "Mã doanh nghiệp là bắt buộc"
                    : "Mã khách hàng là bắt buộc")
                .MinimumLength(2)
                .WithMessage(isEnterpriseValidation ? "Mã doanh nghiệp quá ngắn" : "Mã khách hàng quá ngắn")
                .MaximumLength(256)
                .WithMessage(isEnterpriseValidation ? "Mã doanh nghiệp quá dài" : "Mã khách hàng quá dài");

            RuleFor(x => x.ShortName)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("Tên khách hàng/chi nhánh viết tắt là bắt buộc")
                    .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                    .WithMessage("Tên khách hàng/chi nhánh viết tắt chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)")
                    .MinimumLength(2).WithMessage("Tên khách hàng/chi nhánh viết tắt tối thiếu 2 ký tự")
                    .MaximumLength(256).WithMessage("Tên khách hàng/chi nhánh viết tắt tối đa 256 ký tự");

            RuleFor(x => x.MobilePhoneNo)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Số điện thoại liên hệ là bắt buộc")
                .MinimumLength(10).WithMessage("Số điện thoại liên hệ không hợp lệ");

            When(o => o.CustomerCategoryName != "QT", () => {
                RuleFor(x => x.ProvinceIdentityGuid)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Tỉnh/Thành Phố là bắt buộc")
                .NotEmpty().WithMessage("Tỉnh/Thành Phố là bắt buộc");

                RuleFor(x => x.DistrictIdentityGuid)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Quận/Huyện là bắt buộc")
                    .NotEmpty().WithMessage("Quận/Huyện là bắt buộc");
            });

            When(o => o.CustomerCategoryName == "QT", () => {
                RuleFor(x => x.CountryIdentityGuid)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Quốc gia là bắt buộc")
                .NotEmpty().WithMessage("Quốc gia là bắt buộc");
            });

            //RuleFor(x => x.ProvinceIdentityGuid)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotNull().WithMessage("Tỉnh/Thành Phố là bắt buộc")
            //    .NotEmpty().WithMessage("Tỉnh/Thành Phố là bắt buộc");

            //RuleFor(x => x.DistrictIdentityGuid)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotNull().WithMessage("Quận/Huyện là bắt buộc")
            //    .NotEmpty().WithMessage("Quận/Huyện là bắt buộc");

            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Địa chỉ là bắt buộc")
                .MaximumLength(100).WithMessage("Địa chỉ quá dài(tối đa 1000 ký tự)");

            ////RuleFor(x => x.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
            //RuleFor(x => x.Email)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("Địa chỉ mail liên hệ là bắt buộc");

            //When(x => x.IsPromotion, () =>
            //{
            //    RuleFor(o => o.PromotionTypeId).NotEmpty().WithMessage("Tên loại khuyến mãi là bắt buộc");
            //});

            //RuleFor(x => x.TradingAddress)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage(isEnterpriseValidation
            //        ? "Địa chỉ giao dịch của doanh nghiệp là bắt buộc"
            //        : "Địa chỉ giao dịch của khách hàng là bắt buộc");

            RuleFor(x => x.GroupIds).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Nhóm khách hàng là bắt buộc");

            //RuleFor(x => x.IndustryIds).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Lĩnh vực khách hàng là bắt buộc");

            RuleFor(x => x.CustomerCategoryId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("Danh mục khách hàng là bắt buộc");

            if (isEnterpriseValidation)
            {
                RuleFor(x => x.ShortName)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty().WithMessage("Tên doanh nghiệp/chi nhánh viết tắt là bắt buộc")
                    .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                    .WithMessage("Tên doanh nghiệp/chi nhánh viết tắt chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)")
                    .MinimumLength(2).WithMessage("Tên doanh nghiệp/chi nhánh viết tắt tối thiếu 2 ký tự")
                    .MaximumLength(256).WithMessage("Tên doanh nghiệp/chi nhánh viết tắt tối đa 256 ký tự");

                //RuleFor(x => x.TaxIdNo)
                //    .Cascade(CascadeMode.StopOnFirstFailure)
                //    .NotEmpty().WithMessage("Mã số thuế là bắt buộc")
                //    .MinimumLength(10).WithMessage("Mã số thuế không hợp lệ")
                //    .MaximumLength(15).WithMessage("Mã số thuế không hợp lệ");

                //RuleFor(x => x.BusinessRegCertificate)
                //    .NotEmpty().WithMessage("Số giấy phép đăng ký kinh doanh là bắt buộc");

                //RuleFor(x => x.BrcDateOfIssue)
                //    .NotEmpty().WithMessage("Ngày cấp giấy phép ĐKKD là bắt buộc");

                //RuleFor(x => x.BrcIssuedBy)
                //    .NotEmpty().WithMessage("Nơi cấp giấy phép ĐKKD là bắt buộc");

                RuleFor(x => x.RepresentativePersonName)
                    .NotEmpty().WithMessage("Người đại diện là bắt buộc");

                //RuleFor(x => x.RpPhoneNo)
                //    .NotEmpty().WithMessage("Số điện thoại của người đại diện là bắt buộc");

                When(o => o.IsPartner == true, () =>
                {
                    RuleFor(x => x.UserIdentityGuid).NotNull().WithMessage("Đối tác là bắt buộc")
                    .NotEmpty().WithMessage("Đối tác là bắt buộc");
                });

                RuleFor(x => x.TradingAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Địa chỉ giao dịch của doanh nghiệp là bắt buộc");
            }
            else
            {
                //RuleFor(x => x.ShortName)
                //    .Cascade(CascadeMode.StopOnFirstFailure)
                //    .NotEmpty().WithMessage("Tên khách hàng/chi nhánh viết tắt là bắt buộc")
                //    .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                //    .WithMessage("Tên khách hàng/chi nhánh viết tắt chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)")
                //    .MinimumLength(2).WithMessage("Tên khách hàng/chi nhánh viết tắt tối thiếu 2 ký tự")
                //    .MaximumLength(256).WithMessage("Tên khách hàngp/chi nhánh viết tắt tối đa 256 ký tự");

                //RuleFor(x => x.UserName)
                //    .Cascade(CascadeMode.StopOnFirstFailure)
                //    .NotEmpty().WithMessage("Tên tài khoản là bắt buộc")
                //    .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                //    .WithMessage("Tên tài khoản chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)")
                //    .MinimumLength(6).WithMessage("Tên tài khoản quá ngắn(tối thiểu 6 ký tự)")
                //    .MaximumLength(18).WithMessage("Tên tài khoản quá dài(tối đa 18 ký tự))");

                //RuleFor(x => x.Password)
                //    .Cascade(CascadeMode.StopOnFirstFailure)
                //    .NotEmpty().WithMessage("Mật khẩu người dùng là bắt buộc")
                //    .MinimumLength(6).WithMessage("Mật khẩu quá ngắn(tối thiểu 6 ký tự)")
                //    .MaximumLength(25).WithMessage("Mật khẩu quá dài(tối đa 25 ký tự)");

                //RuleFor(x => x.IdNo).NotEmpty().WithMessage("Số CMT/Thẻ căn cước/Hộ chiếu là bắt buộc");
                //RuleFor(x => x.IdNo).IsValidIdNo();
            }
        }
    }

    public static class IdNoValidator
    {
        public static IRuleBuilderOptions<T, string> IsValidIdNo<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must((root, idNo, context) =>
            {
                if (string.IsNullOrEmpty(idNo)) return true;

                if (Regex.IsMatch(idNo, "^\\d+$"))
                {
                    context.MessageFormatter.AppendArgument("IdType", "Số CMT/Thẻ căn cước");
                    return idNo.Length == 9 || idNo.Length == 12;
                }

                context.MessageFormatter.AppendArgument("IdType", "Số hộ chiếu");
                return Regex.IsMatch(idNo, "^\\w{1}\\d{7,}$");
            }).WithMessage("{IdType} không hợp lệ");
        }
    }
}