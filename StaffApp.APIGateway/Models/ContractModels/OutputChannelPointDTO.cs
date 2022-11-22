using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class OutputChannelPointDTO
    {
        public int Id { get; set; } = 0;
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string LocationId { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; } = OutputChannelPointTypeEnum.Input;
        public string CurrencyUnitCode { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
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
