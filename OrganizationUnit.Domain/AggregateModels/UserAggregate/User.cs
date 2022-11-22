using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CsvHelper.Configuration.Attributes;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using OrganizationUnit.Domain.Commands.User;
using OrganizationUnit.Domain.Seed;

namespace OrganizationUnit.Domain.AggregateModels.UserAggregate
{
    [Table("Users")]
    public class User : Entity, IAggregateRoot
    {
        public User()
        {
            UserBankAccounts = new List<UserBankAccount>();
            ContactInfos = new List<ContactInfo>();
            OrganizationUnitUsers = new HashSet<OrganizationUnitUser>();
        }

        #region Properties
        [Required]
        public string IdentityGuid { get; set; }
        [StringLength(256)]
        public string AccountingCustomerCode { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(256)]
        public string FirstName { get; set; }
        [StringLength(256)]
        public string LastName { get; set; }
        [StringLength(512)]
        public string FullName { get; set; }
        [StringLength(512)]
        public string ShortName { get; set; }
        [StringLength(100)]
        public string MobilePhoneNo { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        public string Email { get; set; }
        [StringLength(12)]
        public string IdNo { get; set; } //Số CMT hoặc thẻ căn cước

        [Column(TypeName = "datetime")]
        public DateTime? DateOfIssueID { get; set; } //Ngày cấp CMT
        [StringLength(256)]
        public string PlaceOfIssueID { get; set; } // Nơi cấp CMT
        public string LastIpAddress { get; private set; }
        [Column(TypeName = "LONGTEXT")]
        public string Password { get; set; }
        //public string PasswordSalt { get; set; }
        [StringLength(68)]
        public string SecurityStamp { get; set; }
        public int? AvatarId { get; set; }
        public int? Status { get; set; }
        public int? Gender { get; set; }
        [StringLength(50)]
        public string FaxNo { get; set; } // Số Fax
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

        [StringLength(256)]
        public string JobPosition { get; set; }

        [StringLength(256)]
        public string JobTitle { get; set; }
        public bool IsLock { get; set; }
        public bool IsPartner { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsCustomer { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public bool IsCustomerInternational { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
        [StringLength(1000)]
        public string TradingAddress { get; set; }
        public List<UserBankAccount> UserBankAccounts { get; set; }

        public List<ContactInfo> ContactInfos { get; set; }
        [Ignore]
        public ConfigurationPersonalAccount ConfigurationAccount { get; set; }
        public virtual HashSet<OrganizationUnitUser> OrganizationUnitUsers { get; set; }
        #endregion

        public void Update(UpdateUserCommand updateCommand)
        {
            AccountingCustomerCode = updateCommand.AccountingCustomerCode;
            MobilePhoneNo = updateCommand.MobilePhoneNo;
            IsLock = updateCommand.IsLock;
            AvatarId = updateCommand.AvatarId;
            IsActive = updateCommand.IsActive;
            IsPartner = updateCommand.IsPartner;
            IsEnterprise = updateCommand.IsEnterprise;
            FirstName = updateCommand.FirstName;
            LastName = updateCommand.LastName;
            ShortName = updateCommand.ShortName;
            FullName = updateCommand.LastName != null && updateCommand.FirstName != null ? $"{updateCommand.LastName} {updateCommand.FirstName}" : updateCommand.FullName;
            IdNo = updateCommand.IdNo;
            Address = updateCommand.Address;
            Status = updateCommand.Status;
            Gender = updateCommand.Gender;
            Email = updateCommand.Email;
            JobPosition = updateCommand.JobPosition;
            JobTitle = updateCommand.JobTitle;
            FaxNo = updateCommand.FaxNo;
            RepresentativePersonName = updateCommand.RepresentativePersonName;
            RpPhoneNo = updateCommand.RpPhoneNo;
            RpDateOfBirth = updateCommand.RpDateOfBirth;
            RpJobPosition = updateCommand.RpJobPosition;
            BusinessRegCertificate = updateCommand.BusinessRegCertificate;
            BrcDateOfIssue = updateCommand.BrcDateOfIssue;
            BrcIssuedBy = updateCommand.BrcIssuedBy;
            IsCustomer = updateCommand.IsCustomer;
            ApplicationUserIdentityGuid = updateCommand.ApplicationUserIdentityGuid;
            Code = updateCommand.Code;
            PlaceOfIssueID = updateCommand.PlaceOfIssueID;
            TaxIdNo = updateCommand.TaxIdNo;
            DateOfIssueID = updateCommand.DateOfIssueID;
            UserName = updateCommand.UserName;
            IsCustomerInternational = updateCommand.IsCustomerInternational;
            Note = updateCommand.Note;
            TradingAddress = updateCommand.TradingAddress;


            DeleteUserBankAccounts();
            if (updateCommand.UserBankAccounts != null)
            {
                foreach (var userBankAccount in updateCommand.UserBankAccounts)
                {
                    AddUserBankAccounts(userBankAccount);
                }
            }

            DeleteUserContactInfos();
            if (updateCommand.UserContactInfos != null)
            {
                foreach (var contactInfo in updateCommand.UserContactInfos)
                {
                    AddUserContactInfos(contactInfo);
                }
            }

            if (updateCommand.OrganizationUnitIds?.Length > 0)
            {
                var toDeleteOrganizationUnitIds = OrganizationUnitUsers
                    .Where(o => !updateCommand.OrganizationUnitIds.Contains(o.OrganizationUnitId))
                    .Select(o => o.OrganizationUnitId)
                    .ToArray();

                var toAddNewOuIds = updateCommand.OrganizationUnitIds
                    .Where(c => OrganizationUnitUsers.All(o => o.OrganizationUnitId != c));

                if (toAddNewOuIds.Any())
                {
                    foreach (var ouId in toAddNewOuIds)
                    {
                        AddToOrganizaton(ouId);
                    }
                }

                if (toDeleteOrganizationUnitIds.Any())
                {
                    foreach (var toDeleteId in toDeleteOrganizationUnitIds)
                    {
                        OrganizationUnitUsers.RemoveWhere(p => p.OrganizationUnitId == toDeleteId);
                    }
                }
            }
            else
            {
                OrganizationUnitUsers.Clear();
            }
        }

        public void AddToOrganizaton(int organizationUnitId)
        {
            if (OrganizationUnitUsers.Any(o => o.OrganizationUnitId == organizationUnitId)) return;

            OrganizationUnitUsers.Add(new OrganizationUnitUser()
            {
                OrganizationUnitId = organizationUnitId
            });
        }

        public void AddUserBankAccounts(CUUserBankAccountCommand cUUserBankAccountCommand)
        {
            UserBankAccounts.Add(new UserBankAccount(cUUserBankAccountCommand));
        }

        public void DeleteUserBankAccounts()
        {
            foreach (var item in UserBankAccounts)
            {
                item.IsDeleted = true;
            }
        }

        public void AddUserContactInfos(CUUserContactInfoCommand cUUserContactInfoCommand)
        {
            ContactInfos.Add(new ContactInfo(cUUserContactInfoCommand));
        }

        public void DeleteUserContactInfos()
        {
            foreach (var item in ContactInfos)
            {
                item.IsDeleted = true;
            }
        }
    }
}
