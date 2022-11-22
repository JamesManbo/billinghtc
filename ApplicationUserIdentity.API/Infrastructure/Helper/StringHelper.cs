using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Helper
{
    public static class StringHelper
    {
        public static string RemoveUnicode(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            var result = src.ToLower().Trim();
            result = Regex.Replace(result, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[đ]", "d", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ý|ỳ|ỷ|ỹ|ỵ]", "y", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–|-]", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"\s+", "");

            return result;
        }

        public static string GetFirstName(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            if (!Regex.IsMatch(src, @"\b+[a-zA-Z]+"))
            {
                return src;
            }

            var nameParts = Regex.Split(src, @"\b+");
            return nameParts.Where(n => Regex.IsMatch(n, @"\w+")).FirstOrDefault();
        }

        public static string GetLastName(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            if (!Regex.IsMatch(src, @"\b+[a-zA-Z]+"))
            {
                return string.Empty;
            }

            var nameParts = Regex.Split(src, @"\b+");
            if (nameParts.Length < 4) return string.Empty;

            var lastNameArr = new List<string>();
            for (int i = 2; i < nameParts.Length - 1; i++)
            {
                if (Regex.IsMatch(nameParts.ElementAt(i), @"\w+"))
                {
                    lastNameArr.Add(nameParts.ElementAt(i));
                }
            }
            return string.Join(" ", lastNameArr);
        }
    }
}
