using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    public enum ReasonType
    {
        Suspension = 1,
        Terminate
    }

    [Table("TransactionReasons")]
    public class TransactionReason : Entity
    {
        public static TransactionReason SuspensionMaintenanceSystem = new TransactionReason(1, "Bảo trì hệ thống HTC-ITC", ReasonType.Suspension);
        public static TransactionReason SuspensionSystemIssue = new TransactionReason(2, "Sự cố hệ thống HTC-ITC", ReasonType.Suspension);
        public static TransactionReason SuspensionTechnicalInfrastructureProblems = new TransactionReason(3, "Sự cố hạ tầng kỹ thuật nơi triển khai", ReasonType.Suspension);
        public static TransactionReason SuspensionCustomerRequest = new TransactionReason(4, "Yêu cầu của khách hàng", ReasonType.Suspension);
        public static TransactionReason SuspensionMoveLocation = new TransactionReason(5, "Dịch chuyển địa điểm", ReasonType.Suspension);
        public static TransactionReason SuspensionOtherReason = new TransactionReason(6, "Lý do khác", ReasonType.Suspension);

        public static TransactionReason TerminateServiceUnavailable = new TransactionReason(7, "HTC ITC dừng cung cấp dịch vụ", ReasonType.Terminate);
        public static TransactionReason TerminateCustomerRequested = new TransactionReason(8, "Yêu cầu của khách hàng", ReasonType.Terminate);
        public static TransactionReason TerminateBadDebt  = new TransactionReason(9, "Quá hạn thanh toán công nợ", ReasonType.Terminate);
        public static TransactionReason TerminateOtherReason = new TransactionReason(10, "Lý do khác", ReasonType.Terminate);
        public static IEnumerable<TransactionReason> Seeds() => new TransactionReason[] {
            SuspensionMaintenanceSystem,
            SuspensionSystemIssue,
            SuspensionTechnicalInfrastructureProblems,
            SuspensionCustomerRequest,
            SuspensionMoveLocation,
            SuspensionOtherReason,
            TerminateServiceUnavailable,
            TerminateCustomerRequested,
            TerminateBadDebt,
            TerminateOtherReason
        };
        public static IEnumerable<TransactionReason> SuspensionReasons => Seeds().Where(r => r.ReasonType == ReasonType.Suspension);
        public static IEnumerable<TransactionReason> TerminateReasons => Seeds().Where(r => r.ReasonType == ReasonType.Terminate);
        public TransactionReason()
        {
        }
        public TransactionReason(int id, string name, ReasonType type)
        {
            Id = id;
            Name = name;
            ReasonType = type;
        }
        public string Name { get; set; }
        public ReasonType ReasonType { get; set; }

        public static string GetTypeName(int id)
        {
            var ob = Seeds().Where(t => t.Id == id).FirstOrDefault();
            if (ob == null) return "";
            return ob.Name;
        }
    }
}
