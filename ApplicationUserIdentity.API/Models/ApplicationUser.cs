using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ApplicationUserIdentity.API.Models
{
    [Table("ApplicationUsers")]
    public class ApplicationUser : Entity
    {
        public int? ClassId { get; set; }
        [StringLength(128)]
        public string IdentityGuid { get; set; }
        [StringLength(256)]
        public string AccountingCustomerCode { get; set; }
        public int? AvatarId { get; set; }
        [StringLength(256)]
        public string CustomerCode { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string FirstName { get; set; }
        [StringLength(256)]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        [StringLength(1000)]
        public string MobilePhoneNo { get; set; }
        [StringLength(1000)]
        public string FaxNo { get; set; }

        [Column(TypeName = "LONGTEXT")]
        public string Password { get; set; }

        [StringLength(128)]
        public string PasswordSalt { get; set; }
        [StringLength(68)]
        public string SecurityStamp { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(256)]
        public string IdNo { get; set; }

        public DateTime? IdDateOfIssue { get; set; }
        [StringLength(1000)]
        public string IdIssuedBy { get; set; }

        public string TaxIdNo { get; set; } // Mã số thuế doanh nghiệp
        [StringLength(256)]
        public string RepresentativePersonName { get; set; } // Tên người đại diện
        [StringLength(256)]
        public string RpPhoneNo { get; set; }
        public DateTime? RpDateOfBirth { get; set; }
        [StringLength(256)]
        public string RpJobPosition { get; set; }
        [StringLength(256)]
        public string BusinessRegCertificate { get; set; } // Số đăng ký kinh doanh
        public DateTime? BrcDateOfIssue { get; set; }
        [StringLength(1000)]
        public string BrcIssuedBy { get; set; }

        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(256)]
        public string Ward { get; set; }

        [StringLength(68)]
        public string WardIdentityGuid { get; set; }
        [StringLength(256)]
        public string District { get; set; }

        [StringLength(68)]
        public string DistrictIdentityGuid { get; set; }
        [StringLength(256)]
        public string Province { get; set; }

        [StringLength(68)]
        public string ProvinceIdentityGuid { get; set; }

        public int? CustomerCategoryId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CustomerStructureId { get; set; }

        [StringLength(256)]
        public string BankName { get; set; }
        [StringLength(256)]
        public string BankAccountNumber { get; set; }
        [StringLength(256)]
        public string BankBranch { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsEmailCertificated { get; set; }
        public bool IsPhoneNoCertificated { get; set; }
        public bool IsLocked { get; set; }
        public string CustomerReviews { get; set; }
        public bool IsPartner { get; set; }
        [StringLength(128)]
        public string UserIdentityGuid { get; set; }
        [StringLength(256)]
        public string ShortName { get; set; }
        [StringLength(1000)]
        public string TradingAddress { get; set; }
        [StringLength(512)]
        public string SwiftCode { get; set; } //Mã giao dịch ngân hàng của khách hàng
        [StringLength(128)]
        public string ProjectIdentityGuid { get; set; }
        public bool IsPromotion { get; set; }
        public int PromotionTypeId { get; set; }
        [IgnoreDataMember]
        public List<Industry> Industries { get; set; }

        public string ParentId { get; set; }

        public string Country { get; set; }

        [StringLength(68)]
        public string CountryIdentityGuid { get; set; }
    }
}
