using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class SignInResultDTO
    {
        public bool Succeeded { get; protected set; }

        public bool IsLockedOut { get; protected set; }

        public bool IsNotExisted { get; protected set; }
        public bool IsNotAllowed { get; protected set; }
        public bool RequiresTwoFactor { get; protected set; }
        public string Token { get; set; }
    }
}
