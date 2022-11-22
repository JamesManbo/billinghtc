using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("PointTypes")]
    public class PointType : Enumeration
    {
        public static PointType NullPoint = new PointType(-1, string.Empty);
        public static PointType StartPoint = new PointType(1, "Điểm đầu");
        public static PointType EndPoint = new PointType(2, "Điểm cuối");

        public PointType(int id, string name) : base(id, name)
        {
        }

        public static PointType[] List()
        {
            return new[] { NullPoint, StartPoint, EndPoint };
        }
    }
}
