using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public class OutContractServicePackageStatus : Enumeration
    {
        public static OutContractServicePackageStatus Developed = new OutContractServicePackageStatus(0, "Đang hoạt động");
        public static OutContractServicePackageStatus Suspend = new OutContractServicePackageStatus(1, "Tạm ngưng");
        public static OutContractServicePackageStatus Terminate = new OutContractServicePackageStatus(2, "Hủy dịch vụ");
        public static OutContractServicePackageStatus Replaced = new OutContractServicePackageStatus(3, "Đã được thay thế");
        public static OutContractServicePackageStatus UpgradeBandwidths = new OutContractServicePackageStatus(4, "Nâng cấp băng thông");
        public static OutContractServicePackageStatus Undeveloped = new OutContractServicePackageStatus(5, "Chưa triển khai");

        public OutContractServicePackageStatus(int id, string name) : base(id, name)
        {
        }

        public static int[] CanBeListedStatuses()
        {
            return new int[]
            {
                Undeveloped.Id,
                Developed.Id,
                Suspend.Id,
                Terminate.Id
            };
        }

        public static int[] ValidStatuses()
        {
            return new int[]
            {
                Undeveloped.Id,
                Developed.Id,
                Suspend.Id,
            };
        }

        public static IEnumerable<OutContractServicePackageStatus> List() => new[]
            {Developed, Suspend, Terminate, Replaced, UpgradeBandwidths, Undeveloped};

        public static OutContractServicePackageStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (state == null)
            {
                throw new ContractDomainException($"Possible values for TransactionStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }


        public static OutContractServicePackageStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ContractDomainException($"Possible values for TransactionStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
