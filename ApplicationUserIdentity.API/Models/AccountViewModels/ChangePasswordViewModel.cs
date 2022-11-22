using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.AccountViewModels
{
    public class ChangePasswordViewModel
    {

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
