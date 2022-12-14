using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Helper
{

    public static class EncryptionHelper
    {
        // The Initialization Vector for the DES encryption routine
        private static readonly byte[] IV =
            new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };

        /// <summary>
        /// Encrypts provided string parameter
        /// </summary>
        public static string EncryptMD5(this string s, string cryptoKey)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            string result;

            byte[] buffer = Encoding.ASCII.GetBytes(s);

            var des = new TripleDESCryptoServiceProvider();
            var md5 = new MD5CryptoServiceProvider();

            des.Key =
                md5.ComputeHash(Encoding.ASCII.GetBytes(cryptoKey));

            des.IV = IV;
            result = Convert.ToBase64String(
                des.CreateEncryptor().TransformFinalBlock(
                    buffer, 0, buffer.Length));

            return result;
        }

        /// <summary>
        /// Decrypts provided string parameter
        /// </summary>
        public static string DecryptMD5(this string s, string cryptoKey)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            string result;

            try
            {
                byte[] buffer = Convert.FromBase64String(s);

                TripleDESCryptoServiceProvider des =
                    new TripleDESCryptoServiceProvider();

                MD5CryptoServiceProvider MD5 =
                    new MD5CryptoServiceProvider();

                des.Key =
                    MD5.ComputeHash(Encoding.ASCII.GetBytes(cryptoKey));

                des.IV = IV;

                result = Encoding.ASCII.GetString(
                    des.CreateDecryptor().TransformFinalBlock(
                    buffer, 0, buffer.Length));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

            return result;
        }
    }
}
