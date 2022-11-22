using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ChannelGroupDTO
    {
        public int Id { get; set; }
        public string ChannelGroupName { get; set; }
        public string ChannelGroupCode { get; set; }
        public string Description { get; set; }
        public int ChannelGroupType { get; set; }
        public string ContractorIdGuid { get; set; }
    }
}
