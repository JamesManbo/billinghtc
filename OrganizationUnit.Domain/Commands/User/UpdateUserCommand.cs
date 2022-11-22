using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.Domain.Commands.User
{
    public class UpdateUserCommand : IRequest<ActionResponse<string>>
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string AccountingCustomerCode { get; set; }
        public string Password { get; set; }
        public string MobilePhoneNo { get; set; }
        public bool IsLock { get; set; }
        public int? AvatarId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPartner { get; set; }
        public bool IsEnterprise { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string IdNo { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public int? RegionId { get; set; } // Vùng miền
        public int? ProvinceId { get; set; } // Tỉnh/Thành phố
        public bool? IsLeader { get; set; }
        public DateTime? StartDate { get; set; }
        public PictureDto Avatar { get; set; }
        public string JobPosition { get; set; }
        public string JobTitle { get; set; }
        public string FaxNo { get; set; } // Số Fax
        public string TaxIdNo { get; set; } // Mã số thuế doanh nghiệp
        public string RepresentativePersonName { get; set; } // Tên người đại diện
        public string RpPhoneNo { get; set; }
        public DateTime? RpDateOfBirth { get; set; }
        public string RpJobPosition { get; set; }
        public string BusinessRegCertificate { get; set; } // Số đăng ký kinh doanh
        public DateTime? BrcDateOfIssue { get; set; }
        public string BrcIssuedBy { get; set; }
        public int[] OrganizationUnitIds { get; set; } //Đơn vị tổ chức
        public bool IsCustomer { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string PlaceOfIssueID { get; set; } // Nơi cấp CMT
        public DateTime? DateOfIssueID { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public List<CUUserBankAccountCommand> UserBankAccounts { get; set; }
        public List<CUUserContactInfoCommand> UserContactInfos { get; set; }
        public bool IsCustomerInternational { get; set; }
        public string Note { get; set; }
        public string TradingAddress { get; set; }
        public bool IsSelfUpdate { get; set; }
    }
}
