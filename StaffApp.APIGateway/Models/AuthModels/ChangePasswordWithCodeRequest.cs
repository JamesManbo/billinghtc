using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StaffApp.APIGateway.Models.AuthModels
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
