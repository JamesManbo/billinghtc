using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{    
    public class InContractType : Enumeration
    {
        public static InContractType InChannelRental = new InContractType(1, "Thuê kênh");
        public static InContractType InCommission = new InContractType(2, "Phân chia hoa hồng");
        public static InContractType InSharingRevenue = new InContractType(3, "Phân chia doanh thu");
        public static InContractType InMaintenance = new InContractType(4, "Bảo trì, bảo dưỡng");

        public static List<InContractType> List()
        {
            return new List<InContractType>()
            {
                InChannelRental,
                InCommission,
                InSharingRevenue,
                InMaintenance
            };
        }
        
        public InContractType(int id, string name) : base(id, name)
        {
        }
    }
}
