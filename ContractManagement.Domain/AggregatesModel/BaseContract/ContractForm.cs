using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    [Table("ContractForms")]
    public class ContractForm : Entity
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int ServiceId { get; set; }
        public int? DigitalSignatureId { get; set; }
        public Picture DigitalSignature { get; set; }
    }
}
