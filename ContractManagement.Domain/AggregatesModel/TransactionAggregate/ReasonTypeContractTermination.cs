using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("ReasonTypeContractTerminations")]
    public class ReasonTypeContractTermination : Entity
    {
        public static ReasonTypeContractTermination ServiceUnavailable = new ReasonTypeContractTermination(1, "HTC ITC dừng cung cấp dịch vụ");
        public static ReasonTypeContractTermination CustomerRequested = new ReasonTypeContractTermination(2, "Yêu cầu của khách hàng");
        public static ReasonTypeContractTermination BadDebt = new ReasonTypeContractTermination(3, "Quá hạn thanh toán công nợ");
        public static ReasonTypeContractTermination Other = new ReasonTypeContractTermination(6, "Lý do khác");
        public static IEnumerable<ReasonTypeContractTermination> Seeds() => new ReasonTypeContractTermination[] {
            ServiceUnavailable,
            CustomerRequested,
            BadDebt,
            Other
        };
        public ReasonTypeContractTermination() { }
        public ReasonTypeContractTermination(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public string Name { get; set; }

        public static string GetTypeName(int id)
        {
            var ob = Seeds().Where(t => t.Id == id).FirstOrDefault();
            if (ob == null) return "";
            return ob.Name;
        }
    }
}
