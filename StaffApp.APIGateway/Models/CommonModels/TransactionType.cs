using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StaffApp.APIGateway.Models.CommonModels
{
    
    public class TransactionType
    {
        public static TransactionType AddNewServicePackage = new TransactionType((int)TransactionTypeEnums.AddNewServicePackage, "Thêm mới dịch vụ/gói cước");
        public static TransactionType ChangeServicePackage = new TransactionType((int)TransactionTypeEnums.ChangeServicePackage, "Thay đổi gói cước/dịch vụ");
        public static TransactionType SuspendServicePackage = new TransactionType((int)TransactionTypeEnums.SuspendServicePackage, "Tạm ngưng dịch vụ");
        public static TransactionType TerminateServicePackage = new TransactionType((int)TransactionTypeEnums.TerminateServicePackage, "Hủy dịch vụ");
        public static TransactionType ChangeLocation = new TransactionType((int)TransactionTypeEnums.ChangeLocation, "Dịch chuyển địa điểm");
        public static TransactionType ChangeEquipment = new TransactionType((int)TransactionTypeEnums.ChangeEquipment, "Thay đổi thiết bị");
        public static TransactionType ReclaimEquipment = new TransactionType((int)TransactionTypeEnums.ReclaimEquipment, "Thu hồi thiết bị");
        public static TransactionType TerminateContract = new TransactionType((int)TransactionTypeEnums.TerminateContract, "Hủy hợp đồng");
        public static TransactionType UpgradeEquipments = new TransactionType((int)TransactionTypeEnums.UpgradeEquipments, "Nâng cấp thiết bị");
        public static TransactionType UpgradeBandwidth = new TransactionType((int)TransactionTypeEnums.UpgradeBandwidth, "Nâng cấp băng thông");
        public static TransactionType RestoreServicePackage = new TransactionType((int)TransactionTypeEnums.RestoreServicePackage, "Khôi phục dịch vụ");

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
            RestoreServicePackage
        };
        public TransactionType() { }
        public TransactionType(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public static string GetTypeName(int type)
        {
            var ob = Seeds().Where(t => t.Id == type).FirstOrDefault();
            if (ob == null) return "";
            return ob.Name;
        }
    }
}
