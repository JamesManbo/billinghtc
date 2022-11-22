using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.UnitOfMeasurementCommand
{
    public class CUUnitOfMeasurementCommand : IRequest<ActionResponse<UnitOfMeasurementDTO>>
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
