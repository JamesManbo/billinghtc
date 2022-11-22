﻿using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUTerminateContractCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
    }
}
