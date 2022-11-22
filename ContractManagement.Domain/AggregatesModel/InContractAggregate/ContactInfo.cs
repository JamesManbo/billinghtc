using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("ContactInfos")]
    public class ContactInfo: Entity
    {
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        [StringLength(512)]
        public string Name { get; set; }
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        protected ContactInfo()
        {
        }

        public ContactInfo(CUContactInfoCommand cUContactInfoCommand)
        {


            //if (cUContactInfoCommand.InContractId.HasValue &&  cUContactInfoCommand.InContractId <= 0)
            //{
            //    throw new ContractDomainException("Id hợp đồng đầu vào không hợp lệ");
            //}

            InContractId = cUContactInfoCommand.InContractId;

            //if (cUContactInfoCommand.OutContractId.HasValue && cUContactInfoCommand.OutContractId <= 0)
            //{
            //    throw new ContractDomainException("Id hợp đồng đầu ra không hợp lệ");
            //}

            OutContractId = cUContactInfoCommand.OutContractId;

            Name = cUContactInfoCommand.Name;
            PhoneNumber = cUContactInfoCommand.PhoneNumber;
            Email = cUContactInfoCommand.Email;
            CreatedBy = cUContactInfoCommand.CreatedBy;
            UpdatedBy = cUContactInfoCommand.UpdatedBy;
            CreatedDate = cUContactInfoCommand.CreatedDate;
            UpdatedDate = cUContactInfoCommand.UpdatedDate;
        }
    }
}
