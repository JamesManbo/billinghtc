using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CustomerModels
{
    public class CreateCustomerDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public CustomerDTO CustomerModel { get; set; }
    }
}
