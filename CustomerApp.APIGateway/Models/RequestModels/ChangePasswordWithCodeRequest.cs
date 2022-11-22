using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.RequestModels
{
    public class ChangePasswordWithCodeRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
