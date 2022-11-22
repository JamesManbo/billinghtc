using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionEquipments")]
    public class TransactionEquipment : DeploymentEquipment, IBind, IRequest
    {
        public int? TransactionId { get; private set; }
        public int? TransactionServicePackageId { get; private set; }
        public int? ContractEquipmentId { get; set; }
        public int OldEquipmentId { get; set; }
        public bool? IsOld { get; set; }
        public float WillBeReclaimUnit { get; set; }
        public float WillBeHoldUnit { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool? IsAcceptanced { get; set; }

        public TransactionEquipment()
        {
        }      


        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            switch (command)
            {
                case CUTransactionEquipmentCommand transEquipment:
                    TransactionId = transEquipment.TransactionId;
                    TransactionServicePackageId = transEquipment.TransactionServicePackageId;
                    ContractEquipmentId = transEquipment.ContractEquipmentId;
                    OldEquipmentId = transEquipment.OldEquipmentId;
                    IsOld = transEquipment.IsOld;
                    PackageName = transEquipment.PackageName;
                    ServiceName = transEquipment.ServiceName;
                    IsAcceptanced = transEquipment.IsAcceptanced;
                    WillBeReclaimUnit = transEquipment.WillBeReclaimUnit;
                    WillBeHoldUnit = transEquipment.WillBeHoldUnit;
                    base.Binding(transEquipment);
                    break;
                default:
                    throw new ContractDomainException($"The {nameof(CUTransactionEquipmentCommand)} command is not supported in binding handling");
            }
        }

        public override void CalculateTotal()
        {
            this.ExaminedSubTotal = this.RealSubTotal = this.ReclaimedSubTotal = this.SubTotal = 0;
            this.ExaminedGrandTotal = this.RealGrandTotal = this.ReclaimedGrandTotal = this.GrandTotal = 0;

            if (this.IsFree)
            {
                return;
            }

            if (this.RealUnit == 0)
            {
                this.ExaminedSubTotal = this.UnitPrice * (decimal)this.ExaminedUnit;
                this.SubTotal = this.ExaminedSubTotal;
            }
            else
            {
                this.RealSubTotal = this.UnitPrice * (decimal)this.RealUnit;
                this.SubTotal = this.RealSubTotal;
            }

            this.ReclaimedSubTotal = this.UnitPrice * (decimal)this.ReclaimedUnit;

            this.ExaminedGrandTotal = this.ExaminedSubTotal;
            this.RealGrandTotal = this.RealSubTotal;
            this.ReclaimedGrandTotal = this.ReclaimedSubTotal;

            this.GrandTotal = this.SubTotal;
        }
    }
}
