using OrganizationUnit.Domain.Commands.User;
using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrganizationUnit.Domain.AggregateModels.UserAggregate
{
    [Table("ContactInfos")]
    public class ContactInfo : Entity
    {
        public int UserId { get; set; }
        [StringLength(512)]
        public string Name { get; set; }
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }

        public ContactInfo()
        {

        }

        public ContactInfo(CUUserContactInfoCommand cUUserContactInfoCommand)
        {
            Name = cUUserContactInfoCommand.Name;
            PhoneNumber = cUUserContactInfoCommand.PhoneNumber;
            Email = cUUserContactInfoCommand.Email;
            CreatedDate = DateTime.Now;
            CreatedBy = cUUserContactInfoCommand.CreatedBy;
            DisplayOrder = 0;
            UserId = cUUserContactInfoCommand.UserId;
            Note = cUUserContactInfoCommand.Note;
        }
    }

}
