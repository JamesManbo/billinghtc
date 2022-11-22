using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.UserGrpc
{
    public class UserGrpcDto
    {
        public int Id { get; set; }
        public Guid IdentityGuid { get; set; }
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
        public DateTime? StartDate { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int DisplayOrder { get; set; }
        public string JobPosition { get; set; }
        public string JobTitle { get; set; }
        //public string Password { get; set; }
        public bool IsLock { get; set; }
        public bool IsEnterprise { get; set; }
        public bool? IsLeader { get; set; }
        public bool IsActive { get; set; }
        public bool IsPartner { get; set; }
    }
}
