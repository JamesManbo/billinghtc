using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Models;

namespace ApplicationUserIdentity.API.Models.AccountViewModels
{
    public class RegisterViewModel : ILinkToEntity<ApplicationUser>
    {
        public string IdentityGuid { get; set; }
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9_]+$",
            ErrorMessage = "Tên tài khoản chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)")]
        [MinLength(6, ErrorMessage = "Tên tài khoản quá ngắn(tối thiểu 6 ký tự)")]
        [MaxLength(256, ErrorMessage = "Tên tài khoản quá dài(tối đa 256 ký tự)")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Họ người dùng là bắt buộc")]
        [MinLength(3, ErrorMessage = "Họ người dùng quá ngắn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [MinLength(2, ErrorMessage = "Tên người dùng quá ngắn")]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        [RegularExpression("^0[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required(ErrorMessage = "Số điện thoại người dùng là bắt buộc")]
        public string MobilePhoneNo { get; set; }
        [Required(ErrorMessage = "Mật khẩu người dùng là bắt buộc")]
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        [StringLength(1000, ErrorMessage = "Địa chỉ quá dài(tối đa 1000 ký tự)")]
        public string Address { get; set; }
        public string Ward { get; set; }
        public string WardIdentityGuid { get; set; }
        public string District { get; set; }
        public string DistrictIdentityGuid { get; set; }
        public string Province { get; set; }
        public string ProvinceIdentityGuid { get; set; }
        public bool IsEmailCertificated { get; set; }
        public bool IsPhoneNoCertificated { get; set; }

        public string Country { get; set; }

        public string CountryIdentityGuid { get; set; }
    }
}
