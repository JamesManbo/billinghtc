using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.Abstractions
{
    public abstract class ServiceLevelAgreementAbstraction : Entity
    {
        public int ServiceId { get; set; }
        public bool IsDefault { get; set; }
        [StringLength(68)]
        public string Uid { get; set; }
        [StringLength(256)]
        public string Label { get; set; }
        [StringLength(2000)]
        public string Content { get; set; }

        public ServiceLevelAgreementAbstraction DeepClone()
        {
            ServiceLevelAgreementAbstraction other = (ServiceLevelAgreementAbstraction)this.MemberwiseClone();
            other.Id = 0;
            return other;
        }
    }
}
