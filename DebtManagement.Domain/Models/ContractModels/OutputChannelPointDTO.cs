using DebtManagement.Domain.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ContractModels
{
    public class OutputChannelPointDTO
    {
        public int Id { get; set; } = 0;
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string LocationId { get; set; }
        public int PointType { get; set; }
        public string CurrencyUnitCode { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal EquipmentAmount { get; set; }
        public string InstallationPointSpliter { get; set; }
        public bool ApplyFeeToChannel { get; set; }

        public OutputChannelPointDTO()
        {
        }
    }
}
