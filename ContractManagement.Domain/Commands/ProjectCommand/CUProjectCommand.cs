using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;

namespace ContractManagement.Domain.Commands.ProjectCommand
{
    public class CUProjectCommand : IRequest<ActionResponse<ProjectDTO>>
    {
        public CUProjectCommand()
        {
            Technicians = new List<TechnicianDTO>();
        }
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int? MarketAreaId { get; set; }
        public int? AgentContractCodeId { get; set; }
        public string NumberOfUnits { get; set; }
        public int? NumberOfRooms { get; set; }
        public int? ActualContractNumber { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<TechnicianDTO> Technicians { get; set; }
        public int NumberOfSupporters { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IdentityGuid { get; set; }
        public int BusinessBlockId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
    }
}
