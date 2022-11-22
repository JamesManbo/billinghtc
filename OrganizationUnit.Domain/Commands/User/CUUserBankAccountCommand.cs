using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Commands.User
{
    public class CUUserBankAccountCommand
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankBranch { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
    }
}
