using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CURenewContractCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CURenewContractCommand() : base()
        {

        }

        public decimal? RenewFee { get; set; }
        public DateTime ContractExpiredDate { get; set; }
        public int ContractRenewMonths { get; set; }
        public DateTime ContractNewExpirationDate { get; set; }
    }
}
