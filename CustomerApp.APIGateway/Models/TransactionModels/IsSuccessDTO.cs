using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.TransactionModels
{
    public class IsSuccessDTO
    {
        public bool IsSuccess { get; set; }
        public List<ErrorModel> Errors { get; set; }

    }
}
