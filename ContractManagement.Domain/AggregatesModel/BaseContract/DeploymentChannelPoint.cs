using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Seed;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public abstract class DeploymentChannelPoint : Entity
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        [StringLength(68)]
        public string LocationId { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal EquipmentAmount { get; set; }
        public bool ApplyFeeToChannel { get; set; }
        public abstract void CalculateTotal();
        public int ConnectionPoint { get; set; }
    }

    public abstract class DeploymentChannelPoint<TEquipment> : DeploymentChannelPoint
        where TEquipment : DeploymentEquipment, IBind, new()
    {
        protected readonly List<TEquipment> _equipments;
        public virtual IReadOnlyCollection<TEquipment> Equipments => _equipments;
        public DeploymentChannelPoint()
        {
            _equipments = new List<TEquipment>();
        }

        public virtual void AddOrUpdateEquipment<TEquipmentCommand>(TEquipmentCommand equipmentCmd)
            where TEquipmentCommand : CUDeploymentEquipmentCommand, IBaseRequest
        {
            // Thiết bị nhập lên là thiết bị mới
            if (equipmentCmd.Id == 0)
            {
                // kiểm tra thiết bị mới có trùng loại thiết bị đã có bên trong kênh.
                // nếu trùng thì cộng dồn số lượng, append các giá trị như địa chỉ MAC, Serial Code
                var existingEquipment = Equipments
                    .SingleOrDefault(o => o.EquipmentId == equipmentCmd.EquipmentId);

                if (existingEquipment != null)
                {
                    existingEquipment.CurrencyUnitId = this.CurrencyUnitId;
                    existingEquipment.CurrencyUnitCode = this.CurrencyUnitCode;
                    existingEquipment.EquipmentName = equipmentCmd.EquipmentName;
                    existingEquipment.EquipmentUom = equipmentCmd.EquipmentUom;

                    existingEquipment.Specifications = equipmentCmd.Specifications;
                    existingEquipment.DeviceCode = equipmentCmd.DeviceCode;
                    existingEquipment.Manufacturer = equipmentCmd.Manufacturer;

                    existingEquipment.UpdateSerialCode(equipmentCmd.SerialCode);
                    existingEquipment.UpdateMacCodes(equipmentCmd.MacAddressCode);
                    //if previous line exist modify it with higher units..
                    existingEquipment.AddExaminedUnits(equipmentCmd.ExaminedUnit);
                    existingEquipment.AddRealUnits(equipmentCmd.RealUnit);
                    existingEquipment.AddReclaimedUnits(equipmentCmd.ReclaimedUnit);
                    existingEquipment.AddSupporterHoldedUnits(equipmentCmd.SupporterHoldedUnit);
                    existingEquipment.CalculateTotal();
                }
                else
                {
                    //add validated new contract equipment
                    var equipmentFactory = new BindingModelFactory<TEquipment, TEquipmentCommand>();
                    var equipmentItem = equipmentFactory.CreateInstance(equipmentCmd);
                    equipmentItem.CreatedDate = DateTime.Now;
                    equipmentItem.CreatedBy = CreatedBy;
                    equipmentItem.CalculateTotal();
                    _equipments.Add(equipmentItem);
                }
            }
            else
            {
                var existEquipment = Equipments.First(e => e.Id == equipmentCmd.Id);
                existEquipment.CreatedDate = DateTime.Now;
                existEquipment.UpdatedBy = UpdatedBy;
                existEquipment.Binding(equipmentCmd);
                existEquipment.CalculateTotal();
            }
            this.CalculateTotal();
        }

        public void RemoveEquipments(IEnumerable<int> removeIds)
        {
            this._equipments.RemoveAll(e => removeIds.Contains(e.Id));
            this.CalculateTotal();
        }

        /// <summary>
        /// Reclaim equipment handler
        /// </summary>
        /// <param name="contractEquipmentId"></param>
        /// <param name="reclaimedUnits"></param>
        /// <returns></returns>
        public IActionResponse ReclaimEquipment(int contractEquipmentId, int reclaimedUnits)
        {
            var contractEquipment = _equipments.SingleOrDefault(e => e.Id == contractEquipmentId);
            if (contractEquipment == null)
            {
                return Failed("The equipment to be reclaimed cannot be found in the deployed list");
            }

            return contractEquipment.AddReclaimedUnits(reclaimedUnits);
        }

        public void UpdateInstallationAddress(InstallationAddress installationAddress)
        {
            this.InstallationAddress = installationAddress.GetCopy();
        }
    }
}
