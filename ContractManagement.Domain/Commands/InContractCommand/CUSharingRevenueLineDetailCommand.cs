using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.InContractCommand
{
    public class CUSharingRevenueLineDetailCommand
    {
        public int Id { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string SharingLineUid { get; set; }
        public int? SharingLineId { get; set; }

        public int SharingType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal SharingAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool HasStartPointSharing { get; set; }
    }
}
