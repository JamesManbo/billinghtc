using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.User
{
    public class UserBankAccountDTO
    {
        public int Id { get; set; }
        public string BankName { get; set; } // Tên ngân hàng
        public string BankAccountNumber { get; set; } // STK ngân hàng
        public string BankBranch { get; set; } // Chi nhánh ngân hàng
        public int UserId { get; set; }
    }
}
