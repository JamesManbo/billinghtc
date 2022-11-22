using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("InContractServices")]
    public class InContractService : Entity
    {
        public int InContractId { get; set; }
        public int ServiceId { get; set; }
        public int ShareType { get; set; }
        public int PointType { get; set; }
        public string ServiceName { get; set; }
        public float SharedPackagePercent { get; set; }
        public float SharedInstallFeePercent { get; set; }

        public InContractService()
        {
        }

        public InContractService(CUInContractServiceCommand cmd)
        {
            this.InContractId = cmd.InContractId;
            this.ServiceId = cmd.ServiceId;
            this.ShareType = cmd.ShareType;
            this.ServiceName = cmd.ServiceName;
            this.PointType = cmd.PointType;
            this.SharedInstallFeePercent = cmd.SharedInstallFeePercent;
            this.SharedPackagePercent = cmd.SharedPackagePercent;
        }
    }
}
