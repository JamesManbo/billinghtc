using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Transactions
{
    public class TransactionChannelPointDTO : BaseDTO
    {
        public int Id { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string LocationId { get; set; }
        public int PointType { get; set; }
        public string CurrencyUnitCode { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal EquipmentAmount { get; set; }

        public List<TransactionEquipmentDTO> Equipments { get; set; }

        public TransactionChannelPointDTO()
        {
            Equipments = new List<TransactionEquipmentDTO>();
        }
    }
}
