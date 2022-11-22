using System.Collections.Generic;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class ChangePasswordResult
    {
        public bool Succeeded { get; set; }
        public List<DraftUserError> Errors { get; set; }
    }
}
