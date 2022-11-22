using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Configs
{
    public class TokenProvideOption
    {
        public TokenProvideOption()
        {
        }

        public string JWTSecretKey { get; set; }

        public string JWTIssuer { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(+1);

        public SigningCredentials SigningCredentials
        {
            get
            {
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTSecretKey));
                return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            }
        }
    }
}
