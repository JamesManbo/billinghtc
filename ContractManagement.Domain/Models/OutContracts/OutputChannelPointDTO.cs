using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Seed;
using GenericRepository.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class OutputChannelPointDTO: IRequest
    {
        public int Id { get; set; } = 0;
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string LocationId { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; } = OutputChannelPointTypeEnum.Input;
        public string CurrencyUnitCode { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal EquipmentAmount { get; set; }
        public string InstallationPointSpliter { get; set; }
        public bool ApplyFeeToChannel { get; set; }
        public int ConnectionPoint { get; set; }

        public List<ContractEquipmentDTO> Equipments { get; set; }

        public OutputChannelPointDTO()
        {
            InstallationAddress = new InstallationAddress();
            Equipments = new List<ContractEquipmentDTO>();
        }
    }
}