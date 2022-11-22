using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels.AppModel
{
    public class CUOutputChannelPointCommandApp
    {
        CUOutputChannelPointCommandApp()
        {
            Equipments = new List<CUContractEquipmentApp>();
        }
        public int Id { get; set; }
        public string LocationId { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public MoneyDTO MonthlyCost { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO EquipmentAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<CUContractEquipmentApp> Equipments { get; set; }
    }
}
