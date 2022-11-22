using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.ContractContentCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    [Table("ContractContents")]
    public class ContractContent : Entity
    {
        public ContractContent() { }

        public ContractContent(CUContractContentCommand contractContentCommand)
        {
            Content = contractContentCommand.Content;
            ContractFormId = contractContentCommand.ContractFormId;
            if (contractContentCommand.DigitalSignatureId > 0)
            {
                DigitalSignatureId = contractContentCommand.DigitalSignatureId;
            }

            if (contractContentCommand.ContractFormSignatureId > 0)
            {
                ContractFormSignatureId = contractContentCommand.ContractFormSignatureId;
            }
        }

        public int? ContractFormId { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public string Content { get; set; }
        public int? DigitalSignatureId { get; set; }
        public int? ContractFormSignatureId { get; set; }
        public Picture DigitalSignature { get; set; }
        public Picture ContractFormSignature { get; set; }
        public OutContract OutContract { get; set; }
        public InContract InContract { get; set; }
    }
}
