using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using Global.Models.StateChangedResponse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public abstract class DeploymentEquipment : Entity
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        [Required]
        public int OutputChannelPointId { get; set; }
        [Required]
        public int OutContractPackageId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentPictureUrl { get; set; }
        public string EquipmentUom { get; set; } // Đơn vị số lượng
        public decimal UnitPrice { get; set; }
        public float ExaminedUnit { get; private set; } // Số lượng khảo sát
        public float ActivatedUnit { get; private set; } // Số lượng thiết bị đang hoạt động
        public float RealUnit { get; private set; } // Số lượng triển khai thực tế
        public float ReclaimedUnit { get; private set; } // Số lượng đã thu hồi
        public float SupporterHoldedUnit { get; private set; } // Số lượng đang tạm giữ
        public bool IsInSurveyPlan { get; set; }
        public bool IsFree { get; set; }
        public bool HasToReclaim { get; set; }

        public string SerialCode { get; set; }
        public string MacAddressCode { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public int StatusId { get; protected set; }
        [Required] public int EquipmentId { get; set; }
        public EquipmentStatus EquipmentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal ExaminedSubTotal { get; set; }
        public decimal ExaminedGrandTotal { get; set; }

        public decimal RealSubTotal { get; set; }
        public decimal RealGrandTotal { get; set; }

        public decimal ReclaimedSubTotal { get; set; }
        public decimal ReclaimedGrandTotal { get; set; }
        public void UpdateSerialCode(string newSerialCodes)
        {
            if (string.IsNullOrWhiteSpace(SerialCode))
            {
                SerialCode = newSerialCodes?.Trim();
            }
            else if (!string.IsNullOrWhiteSpace(newSerialCodes))
            {
                var splitedExistSerials = SerialCode.Split(",").ToList();
                var updateSerials = newSerialCodes.Split(",");
                foreach (var newSerialCode in updateSerials)
                {
                    if (splitedExistSerials.All(c => !c.Equals(newSerialCode.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        splitedExistSerials.Add(newSerialCode.Trim());
                    }
                }
                SerialCode = string.Join(",", splitedExistSerials);
            }
        }

        public void UpdateMacCodes(string newMacAddressCodes)
        {
            if (string.IsNullOrWhiteSpace(MacAddressCode))
            {
                MacAddressCode = newMacAddressCodes?.Trim();
            }
            else if (!string.IsNullOrWhiteSpace(newMacAddressCodes))
            {
                var splitedExistMacCodes = MacAddressCode.Split(",").ToList();
                var updateMacCodes = newMacAddressCodes.Split(",");
                foreach (var newMacCode in updateMacCodes)
                {
                    if (splitedExistMacCodes.All(c => !c.Equals(newMacCode.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        splitedExistMacCodes.Add(newMacCode.Trim());
                    }
                }
                MacAddressCode = string.Join(",", splitedExistMacCodes);
            }
        }

        public virtual void Binding(CUDeploymentEquipmentCommand equipment)
        {
            CurrencyUnitId = equipment.CurrencyUnitId;
            CurrencyUnitCode = equipment.CurrencyUnitCode;
            StatusId = equipment.StatusId;
            EquipmentId = equipment.EquipmentId;
            EquipmentName = equipment.EquipmentName;
            EquipmentUom = equipment.EquipmentUom;
            Specifications = equipment.Specifications;
            DeviceCode = equipment.DeviceCode;
            Manufacturer = equipment.Manufacturer;

            HasToReclaim = equipment.HasToReClaim;
            IsFree = equipment.IsFree;

            SerialCode = equipment.SerialCode;
            MacAddressCode = equipment.MacAddressCode;

            SetUnitPrice(equipment.UnitPrice);
            SetExaminedUnits(equipment.ExaminedUnit);
            SetRealUnits(equipment.RealUnit);
            SetReclaimedUnits(equipment.ReclaimedUnit);
            SetSupporterHoldedUnits(equipment.SupporterHoldedUnit);
        }

        public void SetStatusId(int statusId)
        {
            this.StatusId = statusId;
        }

        public void SetUnitPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ContractDomainException($"Equipment price is not valid");
            }

            this.UnitPrice = price;
        }
        private void CalculateActivatedUnits()
        {
            this.ActivatedUnit = this.RealUnit - this.ReclaimedUnit - this.SupporterHoldedUnit;
        }

        public void SetExaminedUnits(float unit)
        {
            if (unit < 0) throw new ContractDomainException("Examined unit is not valid");

            this.ExaminedUnit = unit;
        }

        public void SetRealUnits(float unit)
        {
            if (unit < 0) throw new ContractDomainException("Real unit is not valid");

            this.RealUnit = unit;
            this.ActivatedUnit = this.RealUnit - this.ReclaimedUnit;
        }

        public void SetReclaimedUnits(float unit)
        {
            if (unit < 0) throw new ContractDomainException("Reclaimed unit is not valid");
            if (unit > RealUnit)
                throw new ContractDomainException("Reclaimed unit value can not greater than real unit value");

            this.ReclaimedUnit = unit;
            this.CalculateActivatedUnits();
        }

        public void SetSupporterHoldedUnits(float unit)
        {
            if (unit < 0) throw new ContractDomainException("Supporter holded unit is not valid");
            if (unit > RealUnit)
                throw new ContractDomainException("Supporter holded unit value can not greater than real unit value");

            this.SupporterHoldedUnit = unit;
            this.CalculateActivatedUnits();
        }

        public IActionResponse AddExaminedUnits(float units)
        {
            if (units < 0)
            {
                return Failed("Số lượng thiết bị không hợp lệ");
            }

            ExaminedUnit += units;
            return Ok();
        }

        public IActionResponse AddRealUnits(float units)
        {
            if (units < 0)
            {
                return Failed("Số lượng thiết bị không hợp lệ");
            }

            RealUnit += units;
            this.CalculateActivatedUnits();
            return Ok();
        }

        public IActionResponse AddReclaimedUnits(float units)
        {
            if (units < 0)
            {
                return Failed("Số lượng thu hồi không hợp lệ");
            }

            if (units > ActivatedUnit)
            {
                return Failed("Số lượng thu hồi không thể vượt quá số lượng thiết bị đang");
            }

            ReclaimedUnit += units;
            this.CalculateActivatedUnits();
            return Ok();
        }

        public IActionResponse AddSupporterHoldedUnits(float units)
        {
            if (units < 0)
            {
                return Failed("Số lượng tạm giữ không hợp lệ");
            }

            if (SupporterHoldedUnit > ActivatedUnit)
            {
                return Failed("Số lượng tạm giữ không thể vượt quá số lượng thiết bị đang hoạt động");
            }

            SupporterHoldedUnit += units;
            this.CalculateActivatedUnits();
            return Ok();
        }
        public abstract void CalculateTotal();
    }
}
