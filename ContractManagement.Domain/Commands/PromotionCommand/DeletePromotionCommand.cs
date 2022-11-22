using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;

namespace ContractManagement.Domain.Commands.PromotionCommand
{
    public class DeletePromotionCommand : IRequest<ActionResponse>
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
