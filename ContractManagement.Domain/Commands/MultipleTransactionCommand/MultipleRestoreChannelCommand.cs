﻿using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.MultipleTransactionCommand
{
    public class MultipleRestoreChannelCommand : BaseMultipleTransactionCommand, IRequest<ActionResponse>
    {
        public MultipleRestoreChannelCommand()
        {
        }
    }
}
