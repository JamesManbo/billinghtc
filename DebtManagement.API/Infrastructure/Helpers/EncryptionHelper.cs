using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.Helpers
{
    public static class EncryptionHelper
    {
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

        public static string Sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
