using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.API.Grpc
{
    public class ConnectionSettings
    {
        public string DefaultConnection { get; set; }
        public string ContractDbConnection { get; set; }
    }
}
