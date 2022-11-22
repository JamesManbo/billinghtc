using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Models.OutContracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUDeploymentChannelPointCommand<TEquipment> : CUDeploymentChannelPointCommand
        where TEquipment : CUDeploymentEquipmentCommand
    {
        public List<TEquipment> Equipments { get; set; }
        public CUDeploymentChannelPointCommand()
        {
            Equipments = new List<TEquipment>();
        }
    }

    public class CUDeploymentChannelPointCommand : IRequest
    {
        public int Id { get; set; }
        public string LocationId { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public bool ApplyFeeToChannel { get; set; }
        public decimal EquipmentAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int[] DeletedEquipmentIds { get; set; }
        public int ConnectionPoint { get; set; }
        public bool PreventRemoveEquipmentIfNotUpdate { get; set; }
    }
}