using System.ComponentModel.DataAnnotations;

namespace SystemUserIdentity.API.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordUserViewModel : ChangePasswordViewModel
    {
        public string UserName { get; set; }
        public string Permission { get; set; }
    }
}
