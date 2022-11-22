using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Global.Models.Auth
{
    public class TokenProvideOptions
    {
        public string JWTSecretKey { get; set; }
        public string JWTIssuer { get; set; }
        public string[] JWTIssuers
        {
            get {
                return string.IsNullOrWhiteSpace(JWTIssuer) ? new string[] { } : JWTIssuer.Split(',');
            }
        }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1);

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
