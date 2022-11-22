using ContractManagement.Domain.AggregateModels.PictureAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models 
{ 
    public class ServiceDTO
    {
        public ServiceDTO()
        {
            ServicePrices = new List<ServicePackagePriceDTO>();
            ServiceLevelAgreements = new List<ServiceLevelAgreementDTO>();
        }
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasStartAndEndPoint { get; set; }
        public string StateLabel => !IsActive ? "Đã khóa" : "Đang hoạt động";

        public PictureDTO  Avatar { get; set; }
        public int AvatarId{ get; set; }
        public int[] DeletedSLAs { get; set; }
        public bool HasPackages { get; set; }
        public bool HasLineQuantity { get; set; }
        public bool HasCableKilometers { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public List<ServicePackagePriceDTO> ServicePrices { get; set; }
        public List<ServiceLevelAgreementDTO> ServiceLevelAgreements { get; set; }
    }
}
