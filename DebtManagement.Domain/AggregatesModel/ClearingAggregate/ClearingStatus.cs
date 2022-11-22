using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.ClearingAggregate
{
    [Table("ClearingStatuses")]
    public class ClearingStatus : Enumeration
    {
        public static ClearingStatus New = new ClearingStatus(1, "Mới tạo");
        public static ClearingStatus Pending = new ClearingStatus(2, "Đang chờ xử lý");
        public static ClearingStatus Success = new ClearingStatus(3, "Đã hoàn thành");
        public static ClearingStatus Cancelled = new ClearingStatus(4, "Đã hủy");
        public ClearingStatus(int id, string name) : base(id, name)
        {
        }

        public static List<ClearingStatus> List()
        {
            return new List<ClearingStatus>()
            {
                New,
                Pending,
                Success,
                Cancelled
            };
        }
        public static ClearingStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new DebtDomainException($"Possible values for ClearingStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
