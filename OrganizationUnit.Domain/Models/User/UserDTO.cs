using System;
using System.Collections.Generic;
using System.Text;
using OrganizationUnit.Domain.Models.Common;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;

namespace OrganizationUnit.Domain.Models.User
{
    public class UserDTO
    {
        public UserDTO()
        {
            UserBankAccounts = new List<UserBankAccountDTO>();
            UserContactInfos = new List<UserContactInfoDTO>();
            OrganizationUnitPaths = new List<string>();
            OrganizationUnits = new List<string>();
            OrganizationUnitIds = new HashSet<int>();
            MngOrganizationUnitPaths = new List<string>();
        }
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string AccountingCustomerCode { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string MobilePhoneNo { get; set; }
        public int? AvatarId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; } //Số CMT
        public DateTime? DateOfIssueID { get; set; } //Ngày cấp CMT
        public string PlaceOfIssueID { get; set; } // Nơi cấp CMT
        public string TaxIdNo { get; set; } = null; // Mã số thuế
        public string PermanentAddress { get; set; } // Thường trú
        public string TemporaryAddress { get; set; } // Tạm trú
        public string Ethnic { get; set; } // Dân tộc
        public string Nationality { get; set; } // Quốc tịch
        public int? RegionId { get; set; } // Vùng miền
        public int? ProvinceId { get; set; } // Tỉnh/Thành phố
        public int MaritalStatus { get; set; } // Tình trạng hôn nhân
        public DateTime? StartDate { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int DisplayOrder { get; set; }
        public PictureDto Avatar { get; set; }
        public string JobPosition { get; set; }
        public string JobTitle { get; set; }
        public string[] Permissions { get; set; }
        public string[] Roles { get; set; }
        public string[] RoleCodes { get; set; }
        public string[] PermissionPages { get; set; }
        public bool IsLock { get; set; }
        public bool IsEnterprise { get; set; }
        public bool? IsLeader { get; set; }
        public bool IsActive { get; set; }
        public bool IsPartner { get; set; }
        public HashSet<int> OrganizationUnitIds { get; set; } //Đơn vị tổ chức
        public bool IsCustomer { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }

        public string RepresentativePersonName { get; set; } // Tên người đại diện
        public string RpPhoneNo { get; set; }
        public DateTime? RpDateOfBirth { get; set; }
        public string RpJobPosition { get; set; }
        public string BusinessRegCertificate { get; set; } // Số đăng ký kinh doanh
        public DateTime? BrcDateOfIssue { get; set; }
        public string BrcIssuedBy { get; set; }
        public List<UserBankAccountDTO> UserBankAccounts { get; set; }
        public int[] ProjectIds { get; set; }
        public List<ProjectDTO> Projects { get; set; }
        public List<string> OrganizationUnitPaths { get; set; }
        public List<string> MngOrganizationUnitPaths { get; set; }
        public List<string> OrganizationUnits { get; set; }
        public string FaxNo { get; set; }
        public bool IsCustomerInternational { get; set; }
        public string Note { get; set; }
        public List<UserContactInfoDTO> UserContactInfos { get; set; }
        public string ShortName { get; set; }
        public string TradingAddress { get; set; }
        public ConfigurationSettingUserDto ConfigurationAccount { get; set; }
    }

    public class UserOrganizationSimpleModel
    {
        public int Id { get; set; }
        public string TreePath { get; set; }
        public string Code { get; set; }
        public int PositionLevel { get; set; }
    }
}
