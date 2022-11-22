using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;

namespace ContractManagement.Domain.Commands.MarketAreaCommand
{
    public class CUMarketAreaCommand : IRequest<ActionResponse<MarketAreaDTO>>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int TreeLevel { get; set; }
        public string TreePath { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
