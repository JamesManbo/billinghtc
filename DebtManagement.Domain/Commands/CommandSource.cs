using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands
{
    public enum CommandSource
    {
        CMS,
        IntegrationEvent,
        MobileApplication
    }
}
