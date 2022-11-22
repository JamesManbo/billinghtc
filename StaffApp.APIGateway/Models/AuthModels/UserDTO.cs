using StaffApp.APIGateway.Models.ProjectModels;
using System;
using System.Collections.Generic;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class UserDTO
    {
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
        public string BankName { get; set; } // Tên ngân hàng
        public string BankAccountNumber { get; set; } // STK ngân hàng
        public string BankBranch { get; set; } // Chi nhánh ngân hàng
        public string OrganizationUnit { get; set; }
        public int[]? OrganizationUnitId { get; set; } //Đơn vị tổ chức
        public DateTime? StartDate { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int DisplayOrder { get; set; }
        public PictureDTO Avatar { get; set; }
        public string JobPosition { get; set; }
        public string JobTitle { get; set; }
        //public string Password { get; set; }
        public bool IsLock { get; set; }
        public bool IsEnterprise { get; set; }
        public bool? IsLeader { get; set; }
        public bool IsActive { get; set; }
        public bool IsPartner { get; set; }
        public int[] ProjectIds { get; set; }
        public List<ProjectDTO> Projects { get; set; }
        public ConfigurationUserDto ConfigurationAccount { get; set; }
        public string[] Permissions { get; set; }
        public string[] RoleCodes { get; set; }
    }
}
