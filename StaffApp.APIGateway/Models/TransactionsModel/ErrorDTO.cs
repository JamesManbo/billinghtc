using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class ErrorDTO
    {
        public string ErrorMessage { get; set; }
        public string MemberName { get; set; }
    }
}
