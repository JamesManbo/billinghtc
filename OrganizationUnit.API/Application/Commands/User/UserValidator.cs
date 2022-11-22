using System;
using System.Text.RegularExpressions;
using FluentValidation;
using OrganizationUnit.Domain.Commands.User;

namespace OrganizationUnit.API.Application.Commands.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {

            When(user => !user.IsPartner, () =>
            {
                RuleFor(user => user.UserName).NotNull().WithMessage("Tên tài khoản là bắt buộc").NotEmpty()
                    .WithMessage("Tên tài khoản là bắt buộc")
                    .MinimumLength(6).WithMessage("Tên tài khoản quá ngắn(tối thiểu 6 ký tự)")
                    .MaximumLength(18).WithMessage("Tên tài khoản quá dài(tối đa 18 ký tự)")
                    .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$")
                    .WithMessage("Tên tài khoản chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");

                RuleFor(user => user.Password)
                    .NotNull().WithMessage("Mật khẩu người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Mật khẩu người dùng là bắt buộc");

                RuleFor(user => user.FirstName).NotNull().WithMessage("Họ người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Họ người dùng là bắt buộc")
                    .MinimumLength(2).WithMessage("Họ người dùng quá ngắn(tối thiểu 2 ký tự)")
                    .MaximumLength(256).WithMessage("Họ người dùng quá dài(tối đa 256 ký tự)");

                RuleFor(user => user.LastName).NotNull().WithMessage("Tên người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Tên người dùng là bắt buộc")
                    .WithMessage("Tên người dùng quá ngắn(tối thiểu 2 ký tự)")
                    .WithMessage("Tên người dùng quá dài(tối đa 256 ký tự)");

                //RuleFor(user => user.OrganizationUnitId).NotNull().WithMessage("Phòng ban là bắt buộc").GreaterThan(0).WithMessage("Phòng ban là bắt buộc");

                //RuleFor(user => user.MobilePhoneNo).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
                //RuleFor(user => user.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");

                //When(u => !u.IsEnterprise, () =>
                //{
                //    RuleFor(x => x.IdNo).NotEmpty().WithMessage("Số CMT/Thẻ căn cước/Hộ chiếu là bắt buộc");
                //    RuleFor(x => x.IdNo).IsValidIdNo();
                //});

                //RuleFor(user => user.Email).NotNull().WithMessage("Email người dùng là bắt buộc").NotEmpty()
                //.WithMessage("Email người dùng là bắt buộc");
            });

            When(user => user.IsPartner, () =>
            {
                RuleFor(user => user.Code).NotNull().WithMessage("Mã là bắt buộc")
                    .NotEmpty().WithMessage("Mã là bắt buộc");

                RuleFor(user => user.FullName)
                    .NotEmpty()
                    .WithMessage("Tên đối tác/đại lý là bắt buộc")
                    .MinimumLength(2)
                    .WithMessage("Tên đối tác/đại lý quá ngắn(tối thiểu 2 ký tự)");

                RuleFor(user => user.ShortName)
                    .NotEmpty()
                    .WithMessage("Tên viết tắt đối tác/đại lý là bắt buộc");

                RuleFor(user => user.FullName).MaximumLength(256)
                    .WithMessage("Tên đối tác/đại lý quá dài(tối đa 256 ký tự)");

                When(u => !u.IsEnterprise, () =>
                {
                    RuleFor(x => x.IdNo).NotEmpty().WithMessage("Số CMT/Thẻ căn cước/Hộ chiếu là bắt buộc");
                    RuleFor(x => x.IdNo).IsValidIdNo();
                });

                When(u => u.IsEnterprise, () =>
                {
                    RuleFor(x => x.TaxIdNo)
                        .NotEmpty().WithMessage("Mã số thuế là bắt buộc")
                        .MinimumLength(10).WithMessage("Mã số thuế không hợp lệ")
                        .MaximumLength(15).WithMessage("Mã số thuế không hợp lệ");

                    RuleFor(x => x.RepresentativePersonName)
                        .NotEmpty().WithMessage("Tên người đại diện là bắt buộc")
                        .MinimumLength(2).WithMessage("Tên người đại diện quá ngắn")
                        .MaximumLength(256).WithMessage("Tên người đại diện quá dài");

                    RuleFor(user => user.RpPhoneNo)
                        .NotEmpty().WithMessage("SĐT người đại diện là bắt buộc");
                    //.Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
                });

                When(u => u.IsCustomer, () =>
                {
                    RuleFor(x => x.ApplicationUserIdentityGuid)
                    .NotEmpty().WithMessage("Khách hàng là bắt buộc")
                    .NotNull().WithMessage("Khách hàng là bắt buộc");
                });

                RuleFor(user => user.MobilePhoneNo).NotNull().WithMessage("Số điện thoại người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Số điện thoại người dùng là bắt buộc");

                RuleFor(user => user.Address).NotNull().WithMessage("Địa chỉ người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Địa chỉ người dùng là bắt buộc");

                RuleFor(user => user.Address).MaximumLength(1000).WithMessage("Địa chỉ quá dài(tối đa 1000 ký tự)");

                RuleFor(user => user.JobPosition).MaximumLength(256)
                    .WithMessage("Vị trí công việc quá dài(tối đa 256 ký tự)");
                RuleFor(user => user.JobTitle).MaximumLength(256).WithMessage("Chức vụ quá dài(tối đa 256 ký tự)");
            });

            //RuleFor(user => user.MobilePhoneNo).NotNull().WithMessage("Số điện thoại người dùng là bắt buộc").NotEmpty()
            //    .WithMessage("Số điện thoại người dùng là bắt buộc");
            ////RuleFor(user => user.MobilePhoneNo).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
            //RuleFor(user => user.Address).NotNull().WithMessage("Địa chỉ người dùng là bắt buộc").NotEmpty()
            //    .WithMessage("Địa chỉ người dùng là bắt buộc");
            //RuleFor(user => user.Address).MaximumLength(1000).WithMessage("Địa chỉ quá dài(tối đa 1000 ký tự)");
            
            ////RuleFor(user => user.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
            //RuleFor(user => user.JobPosition).MaximumLength(256)
            //    .WithMessage("Vị trí công việc quá dài(tối đa 256 ký tự)");
            //RuleFor(user => user.JobTitle).MaximumLength(256).WithMessage("Chức vụ quá dài(tối đa 256 ký tự)");
            ////RuleFor(x => x.IdNo).NotEmpty().WithMessage("Số CMT/Thẻ căn cước/Hộ chiếu là bắt buộc");
            ////RuleFor(x => x.IdNo).IsValidIdNo();
        }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            When(user => !user.IsPartner, () =>
            {
                RuleFor(user => user.FirstName).NotNull().WithMessage("Họ người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Họ người dùng là bắt buộc")
                    .MinimumLength(2).WithMessage("Họ người dùng quá ngắn(tối thiểu 2 ký tự)")
                    .MaximumLength(256).WithMessage("Họ người dùng quá dài(tối đa 256 ký tự)");

                RuleFor(user => user.LastName).NotNull().WithMessage("Tên người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Tên người dùng là bắt buộc")
                    .WithMessage("Tên người dùng quá ngắn(tối thiểu 2 ký tự)")
                    .WithMessage("Tên người dùng quá dài(tối đa 256 ký tự)");

                //RuleFor(user => user.MobilePhoneNo).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
                //RuleFor(user => user.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");

                //When(c => !c.IsSelfUpdate, () =>
                //{
                //    RuleFor(user => user.OrganizationUnitId)
                //    .NotNull().WithMessage("Phòng ban là bắt buộc")
                //    .GreaterThan(0).WithMessage("Phòng ban là bắt buộc");

                //    RuleFor(x => x.IdNo).NotEmpty().WithMessage("Số CMT/Thẻ căn cước/Hộ chiếu là bắt buộc");
                //    RuleFor(x => x.IdNo).IsValidIdNo();
                //});

            });

            When(user => user.IsPartner, () =>
            {
                RuleFor(user => user.FullName)
                    .NotEmpty()
                    .WithMessage("Tên đối tác/đại lý là bắt buộc")
                    .MinimumLength(2)
                    .WithMessage("Tên đối tác/đại lý quá ngắn(tối thiểu 2 ký tự)");
                RuleFor(user => user.FullName).MaximumLength(256)
                    .WithMessage("Tên đối tác/đại lý quá dài(tối đa 256 ký tự)");

                When(u => u.IsEnterprise, () =>
                {
                    RuleFor(x => x.TaxIdNo)
                        .NotEmpty().WithMessage("Mã số thuế là bắt buộc")
                        .MinimumLength(10).WithMessage("Mã số thuế không hợp lệ")
                        .MaximumLength(15).WithMessage("Mã số thuế không hợp lệ");

                    RuleFor(x => x.RepresentativePersonName)
                        .NotEmpty().WithMessage("Tên người đại diện là bắt buộc")
                        .MinimumLength(2).WithMessage("Tên người đại diện quá ngắn")
                        .MaximumLength(256).WithMessage("Tên người đại diện quá dài");

                    RuleFor(user => user.RpPhoneNo)
                        .NotEmpty().WithMessage("SĐT người đại diện là bắt buộc");
                    //.Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
                });

                When(u => !u.IsEnterprise, () =>
                {
                    RuleFor(x => x.IdNo).NotEmpty().WithMessage("Số CMT/Thẻ căn cước/Hộ chiếu là bắt buộc");
                    RuleFor(x => x.IdNo).IsValidIdNo();
                });

                When(u => u.IsCustomer, () =>
                {
                    RuleFor(x => x.ApplicationUserIdentityGuid)
                    .NotEmpty().WithMessage("Khách hàng là bắt buộc")
                    .NotNull().WithMessage("Khách hàng là bắt buộc");
                });

                RuleFor(user => user.MobilePhoneNo).NotNull().WithMessage("Số điện thoại người dùng là bắt buộc").NotEmpty()
                .WithMessage("Số điện thoại người dùng là bắt buộc");
                //RuleFor(user => user.MobilePhoneNo).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
                RuleFor(user => user.Address).NotNull().WithMessage("Địa chỉ người dùng là bắt buộc").NotEmpty()
                    .WithMessage("Địa chỉ người dùng là bắt buộc");
                RuleFor(user => user.Address).MaximumLength(1000).WithMessage("Địa chỉ quá dài(tối đa 1000 ký tự)");
                //RuleFor(user => user.Email).NotNull().WithMessage("Email người dùng là bắt buộc").NotEmpty()
                //    .WithMessage("Email người dùng là bắt buộc");
                //RuleFor(user => user.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
                RuleFor(user => user.JobPosition).MaximumLength(256)
                    .WithMessage("Vị trí công việc quá dài(tối đa 256 ký tự)");
                RuleFor(user => user.JobTitle).MaximumLength(256).WithMessage("Chức vụ quá dài(tối đa 256 ký tự)");
            });


            //RuleFor(user => user.MobilePhoneNo).NotNull().WithMessage("Số điện thoại người dùng là bắt buộc").NotEmpty()
            //    .WithMessage("Số điện thoại người dùng là bắt buộc");
            ////RuleFor(user => user.MobilePhoneNo).Matches("^0[0-9]{9}$").WithMessage("Số điện thoại không hợp lệ");
            //RuleFor(user => user.Address).NotNull().WithMessage("Địa chỉ người dùng là bắt buộc").NotEmpty()
            //    .WithMessage("Địa chỉ người dùng là bắt buộc");
            //RuleFor(user => user.Address).MaximumLength(1000).WithMessage("Địa chỉ quá dài(tối đa 1000 ký tự)");
            //RuleFor(user => user.Email).NotNull().WithMessage("Email người dùng là bắt buộc").NotEmpty()
            //    .WithMessage("Email người dùng là bắt buộc");
            ////RuleFor(user => user.Email).EmailAddress().WithMessage("Địa chỉ email không hợp lệ");
            //RuleFor(user => user.JobPosition).MaximumLength(256)
            //    .WithMessage("Vị trí công việc quá dài(tối đa 256 ký tự)");
            //RuleFor(user => user.JobTitle).MaximumLength(256).WithMessage("Chức vụ quá dài(tối đa 256 ký tự)");
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