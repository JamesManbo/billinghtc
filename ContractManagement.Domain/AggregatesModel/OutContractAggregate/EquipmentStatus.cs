using System;
using System.Collections.Generic;
using System.Linq;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    public class EquipmentStatus : Enumeration
    {
        public static EquipmentStatus Examined = new EquipmentStatus(1, "Trong kế hoạch triển khai");
        //public static EquipmentStatus Confirmed = new EquipmentStatus(2, "Đã được duyệt, chờ triển khai");
        public static EquipmentStatus Deployed = new EquipmentStatus(3, "Đã triển khai");
        public static EquipmentStatus HasToBeReclaim = new EquipmentStatus(4, "Đang chờ thu hồi");
        public static EquipmentStatus Reclaimed = new EquipmentStatus(5, "Đã thu hồi");
        public static EquipmentStatus Cancelled = new EquipmentStatus(6, "Đã hủy");
        public static EquipmentStatus Terminated = new EquipmentStatus(7, "Không thể thu hồi");
        public static EquipmentStatus SupporterHolding = new EquipmentStatus(8, "Tạm giữ");
        public EquipmentStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<EquipmentStatus> Seeds() => new[]
            {Examined, Deployed, HasToBeReclaim, Reclaimed, Cancelled, Terminated, SupporterHolding};

        public static EquipmentStatus FromName(string name)
        {
            var state = Seeds()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            
            if (state == null)
            {
                throw new ContractDomainException($"Possible values for EquipmentStatus: {String.Join(",", Seeds().Select(s => s.Name))}");
            }

            return state;
        }


        public static EquipmentStatus From(int id)
        {
            var state = Seeds().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ContractDomainException($"Possible values for EquipmentStatus: {String.Join(",", Seeds().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
