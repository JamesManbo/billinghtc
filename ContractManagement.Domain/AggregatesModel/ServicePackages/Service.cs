using System.ComponentModel.DataAnnotations.Schema;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("Services")]
    public class Service : Entity
    {
        public int? GroupId { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        /// <summary>
        /// Dịch vụ có điểm đầu điểm cuối
        /// </summary>
        public bool HasStartAndEndPoint { get; set; }
        /// <summary>
        /// Dịch vụ có gói cước
        /// </summary>
        public bool HasPackages { get; set; }
        /// <summary>
        /// Dịch vụ có khai báo số lượng tuyến
        /// </summary>
        public bool HasLineQuantity { get; set; }
        /// <summary>
        /// Dịch vụ có khai báo số kilomet cáp
        /// </summary>
        public bool HasCableKilometers { get; set; }
        /// <summary>
        /// Dịch vụ có phân biệt 2 loại băng thông trong nước và quốc tế
        /// </summary>
        public bool HasDistinguishBandwidth { get; set; }
        public int AvatarId { get; set; }
        public decimal ServicePrice { get; set; }
    }
}
