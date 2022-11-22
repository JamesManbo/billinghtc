using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StaticResource.API.Helper
{
    public static class StringHelper
    {

        public static string ResolveUrl(this string path)
        {
            var validUrl = Uri.EscapeUriString($"{path.ToLower().Replace(@"\", "/")}");
            return validUrl;
        }
        public static string ResolveFileName(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }

            var result = fileName.ToLower().Trim();
            result = Regex.Replace(result, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[đ]", "d", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ý|ỳ|ỷ|ỹ|ỵ]", "y", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–|\\^|\\[|\\]|\\{|\\}]", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"\s+", "-");
            result = Regex.Replace(result, @"-+", "-");
            return result;
        }

    }
}
