using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContractManagement.Utility
{
    public static class StringExtensions
    {
        public static int[] SplitToInt(this string src, char splitter)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return new int[0];
            }

            return src.Split(splitter).Select(int.Parse).ToArray();
        }

        public static string JoinUnique(this string src, string element, char splitter = ',')
        {
            if (string.IsNullOrWhiteSpace(element)) return src;
            if (string.IsNullOrEmpty(src))
            {
                src = element;
                return src;
            }

            var paths = src.Split(splitter).ToList();
            if (paths.Any(c => c.EqualsIgnoreCase(element))) return src;
            paths.Add(element);
            src = string.Join(splitter, paths);
            return src;
        }

        public static IEnumerable<string> SplitByLength(this string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public static string DeepTrim(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            str = str.Trim();
            str = Regex.Replace(str, @"\s+", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return str;
        }

        public static string RemoveSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            str = str.Trim();
            str = Regex.Replace(str, @"\s+", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = Regex.Replace(str, @"\n+", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = Regex.Replace(str, @"\r+", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = Regex.Replace(str, @"\t+", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return str;
        }

        public static string ToAscii(this string str, string joiner = "-")
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
            result = Regex.Replace(result, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–|-]", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"\s+", joiner);
            result = Regex.Replace(result, $@"{joiner}+", joiner);

            return result;
        }

        public static string GetAcronym(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            var wordParts = Regex.Split(input, @"\b", RegexOptions.Multiline);
            return string.Join(string.Empty, wordParts.Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.GetFirstLetter().ToAscii().ToUpper()));
        }

        public static string GetFirstLetter(this string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return string.Empty;

            return src[0].ToString();
        }

        public static string ToUpperFirstLetterOnly(this string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return string.Empty;

            return new string(src.Select((c, i) => i == 0 ? Char.ToUpper(c) : Char.ToLower(c)).ToArray());
        }

        public static bool EqualsIgnoreCase(this string src, string compareStr)
        {
            if (string.IsNullOrWhiteSpace(src) && string.IsNullOrWhiteSpace(compareStr)) return true;

            if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(compareStr)) return false;

            return src.Trim().Equals(compareStr.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string src, string compareStr)
        {
            if (string.IsNullOrWhiteSpace(src) && string.IsNullOrWhiteSpace(compareStr)) return true;

            if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(compareStr)) return false;

            return src.Contains(compareStr, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}