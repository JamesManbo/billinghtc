using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Dashboard.DashboardSupporter
{
    public class CurrentWorkStatusDTO
    {
        public int DoneWorkQuantity { get; set; }
        public int PendingWorkQuantity { get; set; }
        public int CancelWorkQuantity { get; set; }
        public int TotalQuantity { get; set; }
        public int MaketAreaId { get; set; }
    }
}
