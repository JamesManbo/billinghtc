using CustomerApp.APIGateway.Models.CommonModels;
using CustomerApp.APIGateway.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class OutputChannelPointDTO
    {
        public int Id { get; set; } = 0;
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string LocationId { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; } = OutputChannelPointTypeEnum.Input;
        public string CurrencyUnitCode { get; set; }
        public InstallationAddressDTO InstallationAddress { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO MonthlyCost { get; set; }
        public MoneyDTO EquipmentAmount { get; set; }
        public string InstallationPointSpliter { get; set; }
        public bool ApplyFeeToChannel { get; set; }

        public List<OutContractEquipmentDTO> Equipments { get; set; }

        public OutputChannelPointDTO()
        {
            Equipments = new List<OutContractEquipmentDTO>();
        }
    }
}
