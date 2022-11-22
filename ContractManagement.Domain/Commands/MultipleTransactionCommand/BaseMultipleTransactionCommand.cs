using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Commands.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.MultipleTransactionCommand
{
    public class BaseMultipleTransactionCommand
    {
        public string Note { get; set; }
        public bool IsSupplierConfirmation { get; set; }
        public bool IsTechnicalConfirmation { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int[] ChannelIds { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorUserId { get; set; }
        public bool AutoConfirmation { get; set; } = false;
    }
}
