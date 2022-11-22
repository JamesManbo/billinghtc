using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("ChannelGroups")]
    public class ChannelGroups : Entity
    {
        public ChannelGroups() { }

        public string ChannelGroupName { get; set; }
        public string ChannelGroupCode { get; set; }
        public string Description { get; set; }
        public int ChannelGroupType { get; set; }
        public string ContractorIdGuid { get; set; }
    }
}
