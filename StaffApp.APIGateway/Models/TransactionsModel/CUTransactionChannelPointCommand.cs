using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ContractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class CUTransactionChannelPointCommand
    {
        public CUTransactionChannelPointCommand()
        {
            Equipments = new List<CUTransactionEquipmentCommand>();
        }
        public int Id { get; set; }
        public string LocationId { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal EquipmentAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<CUTransactionEquipmentCommand> Equipments { get; set; }
    }
}
