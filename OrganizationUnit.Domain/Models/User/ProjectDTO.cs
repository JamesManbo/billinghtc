using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.User
{
    public class ProjectDTO
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

    }
}
