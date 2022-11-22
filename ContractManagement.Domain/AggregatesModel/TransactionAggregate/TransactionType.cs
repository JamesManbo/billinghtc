using Global.Models.Response;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionType")]
    public class TransactionType
    {
        public static TransactionType AddNewServicePackage = new TransactionType((int)TransactionTypeEnums.AddNewServicePackage, "Thêm mới dịch vụ/gói cước", "APPROVED_ADD_NEW_SERVICE_PACKAGE_OUT_CONTRACT");
        public static TransactionType ChangeServicePackage = new TransactionType((int)TransactionTypeEnums.ChangeServicePackage, "Điều chỉnh gói cước", "APPROVED_CHANGE_SERVICE_PACKAGE_OUT_CONTRACT");
        public static TransactionType SuspendServicePackage = new TransactionType((int)TransactionTypeEnums.SuspendServicePackage, "Tạm ngưng dịch vụ", "APPROVED_SUSPEND_SERVICE_PACKAGE_OUT_CONTRACT");
        public static TransactionType TerminateServicePackage = new TransactionType((int)TransactionTypeEnums.TerminateServicePackage, "Hủy dịch vụ", "APPROVED_TERMINATE_SERVICE_PACKAGE_OUT_CONTRACT");
        public static TransactionType ChangeLocation = new TransactionType((int)TransactionTypeEnums.ChangeLocation, "Dịch chuyển địa điểm", "APPROVED_CHANGE_LOCATION_OUT_CONTRACT");
        public static TransactionType ChangeEquipment = new TransactionType((int)TransactionTypeEnums.ChangeEquipment, "Thay đổi thiết bị", "APPROVED_CHANGE_EQUIPMENT_OUT_CONTRACT");
        public static TransactionType ReclaimEquipment = new TransactionType((int)TransactionTypeEnums.ReclaimEquipment, "Thu hồi thiết bị", "APPROVED_RECLAIM_EQUIPMENT_OUT_CONTRACT");
        public static TransactionType TerminateContract = new TransactionType((int)TransactionTypeEnums.TerminateContract, "Thanh lý hợp đồng", "APPROVED_TERMINATE_OUT_CONTRACT");
        public static TransactionType UpgradeEquipments = new TransactionType((int)TransactionTypeEnums.UpgradeEquipments, "Nâng cấp thiết bị", "APPROVED_UPGRADE_EQUIPMENTS_OUT_CONTRACT");
        public static TransactionType UpgradeBandwidth = new TransactionType((int)TransactionTypeEnums.UpgradeBandwidth, "Nâng cấp băng thông", "APPROVED_UPGRADE_BANDWIDTH_OUT_CONTRACT");
        public static TransactionType RestoreServicePackage = new TransactionType((int)TransactionTypeEnums.RestoreServicePackage, "Khôi phục dịch vụ", "APPROVED_RESTORE_SERVICE_PACKAGE_OUT_CONTRACT");
        public static TransactionType DeployNewOutContract = new TransactionType((int)TransactionTypeEnums.DeployNewOutContract, "Triển khai hợp đồng mới", "APPROVED_DEPLOY_NEW_OUT_CONTRACT");
        public static TransactionType RenewContract = new TransactionType((int)TransactionTypeEnums.RenewContract, "Gia hạn hợp đồng", "RENEW_CONTRACT");

        public enum TransactionTypeEnums
        {
            AddNewServicePackage = 1
                , ChangeServicePackage
                , SuspendServicePackage
                , TerminateServicePackage
                , ChangeLocation
                , ChangeEquipment
                , ReclaimEquipment
                , TerminateContract
                , UpgradeEquipments
                , UpgradeBandwidth
                , RestoreServicePackage
                , DeployNewOutContract
                , RenewContract
        }

        public static IEnumerable<TransactionType> Seeds() => new TransactionType[] {
            AddNewServicePackage,
            ChangeServicePackage,
            SuspendServicePackage,
            TerminateServicePackage,
            ChangeLocation,
            ChangeEquipment,
            ReclaimEquipment,
            TerminateContract,
            UpgradeEquipments,
            UpgradeBandwidth,
            RestoreServicePackage,
            DeployNewOutContract,
            RenewContract
        };

        public static IEnumerable<SelectionItem> SelectionList() => new SelectionItem[]
            {
                new SelectionItem(){Value = AddNewServicePackage.Id, Text = AddNewServicePackage.Name },
                new SelectionItem(){Value = ChangeServicePackage.Id, Text = ChangeServicePackage.Name },
                new SelectionItem(){Value = SuspendServicePackage.Id, Text = SuspendServicePackage.Name },
                new SelectionItem(){Value = TerminateServicePackage.Id, Text = TerminateServicePackage.Name },
                new SelectionItem(){Value = ChangeLocation.Id, Text = ChangeLocation.Name },
                new SelectionItem(){Value = ChangeEquipment.Id, Text = ChangeEquipment.Name },
                new SelectionItem(){Value = ReclaimEquipment.Id, Text = TerminateServicePackage.Name },
                new SelectionItem(){Value = TerminateContract.Id, Text = TerminateContract.Name },
                new SelectionItem(){Value = UpgradeEquipments.Id, Text = UpgradeEquipments.Name },
                new SelectionItem(){Value = UpgradeBandwidth.Id, Text = UpgradeBandwidth.Name },
                new SelectionItem(){Value = RestoreServicePackage.Id, Text = RestoreServicePackage.Name },
                new SelectionItem(){Value = DeployNewOutContract.Id, Text = DeployNewOutContract.Name },
                new SelectionItem(){Value = RenewContract.Id, Text = RenewContract.Name }
            };

        public TransactionType() { }
        public TransactionType(int id, string name, string permission)
        {
            Id = id;
            Name = name;
            Permission = permission;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Permission { get; set; }

        public static string GetTypeName(int type)
        {
            var ob = Seeds().Where(t => t.Id == type).FirstOrDefault();
            if (ob == null) return "";
            return ob.Name;
        }

        public static string GetTypePermission(int type)
        {
            var ob = Seeds().Where(t => t.Id == type).FirstOrDefault();
            if (ob == null) return "";
            return ob.Permission;
        }
    }
}
