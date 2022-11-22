using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractTransactionCommand
{
    public class CUOutContractTransactionCommand : IRequest<ActionResponse<OutContractTransactionDTO>>
    {
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public int TransactionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
