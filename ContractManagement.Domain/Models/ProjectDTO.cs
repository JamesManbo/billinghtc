using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public class ProjectDTO
    {
        public ProjectDTO()
        {
            Technicians = new List<string>();
        }
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? MarketAreaId { get; set; }
        public int? AgentContractCodeId { get; set; }
        public string NumberOfUnits { get; set; }
        public int? NumberOfRooms { get; set; }
        public int? NumberOfOutContracts { get; set; }
        public string MarketName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string StateLabel => !IsActive ? "Không hoạt động" : "Đang hoạt động";
        public DateTime UpdatedDate { get; set; }
        public List<string> Technicians { get; set; }
        public string IdentityGuid { get; set; }
        public int BusinessBlockId { get; set; }
        public string BusinessBlockName { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
    }

    public class ProjectRaw
    {       
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? MarketAreaId { get; set; }
        public int? AgentContractCodeId { get; set; }
        public string NumberOfUnits { get; set; }
        public int? NumberOfRooms { get; set; }
        public int? NumberOfOutContracts { get; set; }
        public string MarketName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string StateLabel => !IsActive ? "Không hoạt động" : "Đang hoạt động";
        public DateTime UpdatedDate { get; set; }
        public string UserTechnicianId { get; set; }
        public string IdentityGuid { get; set; }
        public int BusinessBlockId { get; set; }
        public string BusinessBlockName { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
    }
}
