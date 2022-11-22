using System;
using System.Security.Cryptography;
using System.Text;

namespace CMS.APIGateway.Infrastructure.Helpers
{

    public static class EncryptionHelper
    {
        /// <summary>
        /// Encrypts provided string parameter
        /// </summary>
        public static string EncryptSHA256(this string s, string cryptoKey)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(cryptoKey);
            byte[] messageBytes = encoding.GetBytes(s);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
