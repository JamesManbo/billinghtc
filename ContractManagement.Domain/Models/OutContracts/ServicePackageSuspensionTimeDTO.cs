using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.OutContracts
{
    public class ServicePackageSuspensionTimeDTO
    {
        public int Id { get; set; }
        public int OutContractServicePackageId { get; set; }
        public DateTime SuspensionStartDate { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public int DaysSuspended { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Description { get; set; }
    }
}
