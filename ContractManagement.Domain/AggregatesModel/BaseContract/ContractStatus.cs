using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    [Table("ContractStatus")]
    public class ContractStatus : Enumeration
    {
        public static ContractStatus Draft = new ContractStatus(1, "Chờ ký");
        public static ContractStatus Signed = new ContractStatus(2, "Đã ký");
        // public static ContractStatus Accepted = new ContractStatus(3, "Đã nghiệm thu");
        // public static ContractStatus Paused = new ContractStatus(4, "Tạm ngưng");
        public static ContractStatus Liquidated = new ContractStatus(5, "Đã thanh lý");
        // public static ContractStatus SubmissionProcess = new ContractStatus(6, "Trình ký");
        public static ContractStatus Cancelled = new ContractStatus(9, "Hủy");
        // public static ContractStatus Other = new ContractStatus(10, "Khác");

        public ContractStatus(int id, string name) : base(id, name)
        {
        }

        public static bool IsActiveStatus(int statusId)
        {
            return Signed.Id == statusId;
        }

        public static IEnumerable<ContractStatus> List() => new[]
            {Draft, Signed, Liquidated, Cancelled};

        public static ContractStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (state == null)
            {
                throw new ContractDomainException($"Possible values for ContractStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static ContractStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ContractDomainException($"Possible values for ContractStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
