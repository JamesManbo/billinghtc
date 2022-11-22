using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class TransactionPendingTaskDTO
    {
        public string UserTechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public string ProjectName { get; set; }
        public int PendingTasks { get; set; }
        public decimal TaskPerSupporterAVG { get; set; }
        public decimal TaskPerProjectAVG { get; set; }
        
    }
}
