﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.ApplicationUsers
{
    public class ApplicationUserDTO
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string AccountingCustomerCode { get; set; }
        public string CustomerCode { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public int? Gender { get; set; }
        public string MobilePhoneNo { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string IdNo { get; set; }
        public int? AvatarId { get; set; }
        public string Email { get; set; }
        public string FaxNo { get; set; }

        public string Address { get; set; }
        public string Ward { get; set; }

        public string WardIdentityGuid { get; set; }
        public string District { get; set; }

        public string DistrictIdentityGuid { get; set; }
        public string Province { get; set; }

        public string ProvinceIdentityGuid { get; set; }
        public string IdIssuedBy { get; set; }
        public DateTime? IdDateOfIssue { get; set; }

        public string TaxIdNo { get; set; } // Mã số thuế doanh nghiệp
        public string BusinessRegCertificate { get; set; } // Số đăng ký kinh doanh
        public DateTime? BrcDateOfIssue { get; set; }
        public string BrcIssuedBy { get; set; }
        public string RepresentativePersonName { get; set; }
        public string RpPhoneNo { get; set; }
        public DateTime? RpDateOfBirth { get; set; }
        public string RpJobPosition { get; set; }

        public bool IsEmailCertificated { get; set; }
        public bool IsPhoneNoCertificated { get; set; }
        public bool IsLocked { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ClassName { get; set; }
        public bool IsEnterprise { get; set; }
        public int? CustomerCategoryId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CustomerStructureId { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankBranch { get; set; }
        public int? ClassId { get; set; }
        public string CustomerReviews { get; set; }
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public List<int> GroupIds { get; set; }
        public List<string> GroupNames { get; set; }
        public List<int> IndustryIds { get; set; }
        public List<string> CustomerIndustryNames { get; set; }
        public string CustomerCategoryName { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerStructureName { get; set; }
    }
}
