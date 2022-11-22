using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class IsSuccessDTO
    {
        public bool IsSuccess { get; set; }
        public List<ErrorDTO> Errors { get; set; }
    }
}
