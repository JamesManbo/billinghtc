using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace News.API.Common
{
    public static class StringHelperExtension
    {
        public const string UniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";

        public const string AsciiChars =
            //"aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyaaaaaaaaaaaaaaaaaeeeeeeeeeeediiiooooooooooooooooooouuuuuuuuuuuyyyyyaadoou";

        public const string KeyBoardChars = " `1234567890-=~!@#$%^&*()_+qwertyuiop[]{}|asdfghjkl;':zxcvbnm,./<>?*-+";
        /// <summary>
        /// Split string to an int array
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<int> SplitToArray(this string s, char seperator)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var result = new List<int>();
            var strArr = s.Split(seperator);
            foreach (var numberAsStr in strArr)
            {
                if (int.TryParse(numberAsStr, out var number))
                {
                    result.Add(number);
                }
            }

            return result;
        }

        public static T DeserializeObject<T>(this string source) where T : class
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            return JsonConvert.DeserializeObject<T>(source);
        }

        public static int? ToNullableInt(this string source)
        {
            if (string.IsNullOrWhiteSpace(source) || !int.TryParse(source, out var result))
            {
                return null;
            }

            return result;
        }

        public static string ToAscii(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var result = str.ToLower().Trim();
            result = Regex.Replace(result, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[đ]", "d", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[ý|ỳ|ỷ|ỹ|ỵ]", "y", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–]", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"\s+", "-");
            result = Regex.Replace(result, @"-+", "-");

            return result;
        }

        public static string GenerateRandomString(int length = 8)
        {
            var random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        private static readonly Regex Reg = new Regex("([a-z,0-9](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", RegexOptions.Compiled);

        /// <summary>
        /// This splits up a string based on capital letters
        /// e.g. "MyAction" would become "My Action" and "My10Action" would become "My10 Action"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SplitPascalCase(this string str)
        {
            return Reg.Replace(str, "$1 ");
        }

        public static string ToUpperFirstLetter(this string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return string.Empty;

            var chars = src
                .Select((c, i) => i == 0 ? c.ToString().ToUpper() : c.ToString())
                .ToArray();

            return string.Join("", chars);
        }

        public static string ToLowerFirstLetter(this string src)
        {
            var chars = src
                .Select((c, i) => i == 0 ? c.ToString().ToLower() : c.ToString())
                .ToArray();

            return string.Join("", chars);
        }
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

        public static string GetPlainTextFromHtml(this string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(source);
            return doc.DocumentNode.InnerText;
        }
        public static string ResolveExcelValue(this object s)
        {
            if (s == null) return string.Empty;
            var result = s.ToString().Trim();
            if (string.IsNullOrEmpty(result)) return string.Empty;
            //            if (result.Contains("Intentional self-poisoning by and"))
            //            {
            //                Console.Write("Abc");
            //            }
            return Regex.Replace(result, @"\r\n?|\n|\t", string.Empty);
        }
    }
}
