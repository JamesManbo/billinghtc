using OrganizationUnit.Domain.Commands.User;
using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.Domain.AggregateModels.UserAggregate
{
    [Table("UserBankAccounts")]
    public class UserBankAccount : Entity
    {
        public UserBankAccount()
        {

        }

        public UserBankAccount(CUUserBankAccountCommand cUUserBankAccountCommand)
        {
            BankName = cUUserBankAccountCommand.BankName;
            BankAccountNumber = cUUserBankAccountCommand.BankAccountNumber;
            BankBranch = cUUserBankAccountCommand.BankBranch;
            CreatedDate = DateTime.Now;
            CreatedBy = cUUserBankAccountCommand.CreatedBy;
            DisplayOrder = 0;
        }

        [StringLength(256)]
        public string BankName { get; set; }
        [StringLength(256)]
        public string BankAccountNumber { get; set; }
        [StringLength(256)]
        public string BankBranch { get; set; }
        public int UserId { get; set; }
    }
}
