using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.Abstraction
{
    public abstract class ServiceLevelAgreementCommandAbstraction
    {
        public int? ServiceId { get; set; }
        public bool IsDefault { get; set; }
        public string Uid { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
        public int Id { get; set; }
    }
}
