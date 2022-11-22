using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TransactionChannelPointDTO
    {
        public int Id { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string LocationId { get; set; }
        public int PointType { get; set; }
        public string CurrencyUnitCode { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO MonthlyCost { get; set; }
        public MoneyDTO EquipmentAmount { get; set; }

        public List<TransactionEquipmentDTO> Equipments { get; set; }

        public TransactionChannelPointDTO()
        {
            Equipments = new List<TransactionEquipmentDTO>();
        }
    }
}
