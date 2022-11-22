using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.AspNetCore.StaticFiles;

namespace Feedback.API.Infrastructure.Helpers
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
                return new List<int>();
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
            result = Regex.Replace(result, @"-+", " ");

            return result;
        }
        public static string ResolveSqlParameter(this string str)
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
            result = Regex.Replace(result, @"\s+", "_");
            result = Regex.Replace(result, @"-+", "_");

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

        public static string GetFirstLetter(this string src)
        {
            if (string.IsNullOrWhiteSpace(src)) return string.Empty;

            return src[0].ToString();
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
        public static string ConvertMoneyNumberToString(this string moneyNumber)
        {
            return new DocSo().ConvertMoneyNumberToString(moneyNumber.Split('.')[0]);
        }
    }

    public class Nhom3So
    {
        //Các Property ứng với các chữ số hàng đơn vị, hàng chục và hàng trăm
        public int hd { get; set; }
        public int hc { get; set; }
        public int ht { get; set; }

        //Phương thức khởi tạo: Nhập nhóm 3 chữ số, đưa từng chữ số trong nhóm
        //vào các thuộc tính
        public Nhom3So() { }

        public Nhom3So(int nhom)
        {
            this.hd = nhom % 10;
            this.hc = nhom / 10 % 10;
            this.ht = nhom / 100;
        }

        public Nhom3So(string stringNhom)
        {
            if (int.TryParse(stringNhom, out int nhom))
            {
                this.hd = nhom % 10;
                this.hc = nhom / 10 % 10;
                this.ht = nhom / 100;
            }
        }
    }

    public class DocSo
    {
        public DocSo()
        {
            DocSo_Load();
        }

        //Khởi tạo Hashtable chứa các key và value dùng để đọc từng con số
        int[] kyso = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        string[] kytu1 = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        string[] kytu2 = { "", "mốt", "hai", "ba", "bốn", "lăm", "sáu", "bảy", "tám", "chín" };

        public Dictionary<int, string> h1 = new Dictionary<int, string>();
        public Dictionary<int, string> hdv = new Dictionary<int, string>();

        public void KhoiTao_h1_hdv()
        {
            for (int i = 0; i <= 9; i++)
            {
                h1.Add(kyso[i], kytu1[i]);
                hdv.Add(kyso[i], kytu2[i]);
            }
        }

        //Kiểm tra hai số liên tiếp có bằng 0 hay không
        private int Ktrazero(Nhom3So nhom)
        {
            if (nhom.ht == nhom.hc && nhom.ht == 0 && nhom.hd != 0)
                return 0;
            if (nhom.hc == nhom.hd && nhom.hc == 0 && nhom.ht != 0)
                return 1;
            if (nhom.ht == nhom.hc && nhom.hc == nhom.hd && nhom.ht == 0)
                return 2;
            else return 3;
        }

        //Đọc hàng đơn vị
        private string Docdv(Nhom3So nhom)
        {
            if (nhom.hc != 0)
                return hdv[nhom.hd].ToString();
            else
                return h1[nhom.hd].ToString();
        }

        private string Doc1so(int so)
        {
            return h1[so].ToString();
        }

        private string Doc3so(Nhom3So nhom, bool lopdautien, bool lopdonvi) //Kiểm tra xem lớp đang đọc có phải lớp bên trái cùng
        {
            BaSo0 = false;
            if (Ktrazero(nhom) == 0)
                if (lopdautien)
                    return Docdv(nhom);
                else
                    return "không trăm linh " + Docdv(nhom);
            else
                if (Ktrazero(nhom) == 1)
                return Doc1so(nhom.ht) + " trăm";
            else
                    if (Ktrazero(nhom) == 2)
            {
                BaSo0 = true;
                return "";
            }
            else
                        if (nhom.ht == 0)
            {
                if (lopdautien)
                {
                    if (nhom.hc == 1)
                        if (nhom.hd != 1)
                            return " mười " + Docdv(nhom);
                        else
                            return " mười một ";
                    else
                        if (nhom.hc == 0)
                        return " linh " + Docdv(nhom);
                    else
                        return Doc1so(nhom.hc) + " mươi " + Docdv(nhom);
                }
                else
                {
                    if (nhom.hc == 1)
                        if (nhom.hd != 1)
                            return " mười " + Docdv(nhom);
                        else
                            if (!lopdonvi)
                            return " không trăm mười một";
                        else
                            return "mười một";
                    else
                        if (nhom.hc == 0)
                        return " không trăm linh " + Docdv(nhom);
                    else
                        return " không trăm " + Doc1so(nhom.hc) + " mươi " + Docdv(nhom);
                }
            }
            else
                            if (nhom.hc == 0)
                return Doc1so(nhom.ht) + " trăm linh " + Docdv(nhom);
            else
                                if (nhom.hc == 1)
                return Doc1so(nhom.ht) + " trăm mười một";
            else
                return Doc1so(nhom.ht) + " trăm " + Doc1so(nhom.hc) + " mươi " + Docdv(nhom);


        }

        Nhom3So[] nhomso = new Nhom3So[0];

        //Đếm số nhóm của số cần đọc và TRỪ ĐI 1
        private int DemSoNhom(string so)
        {
            if (so.Length % 3 == 0)
                return so.Length / 3 - 1;
            else
                return so.Length / 3;
        }

        //Đảo nhóm 3 chữ số
        //private string DaoNhom(string nhom)
        //{
        //    string kq = "";
        //    for (int i = nhom.Length - 1; i >= 0; i--)
        //        kq += nhom[i];
        //    return kq;
        //}

        //Tách số thành từng nhóm 3 số, đưa các nhóm 3 số này vào mảng nhomso
        private void TachNhom(string so)
        {
            if (DemSoNhom(so) < 1)
            {
                Stack<char> temp2 = new Stack<char>();
                for (int i = so.Length - 1; i >= 0; i--)
                    temp2.Push(so[i]);
                Array.Resize<Nhom3So>(ref nhomso, nhomso.Length + 1);
                StringBuilder sb = new StringBuilder();
                while (temp2.Count > 0)
                    sb.Append(temp2.Pop());
                nhomso[nhomso.Length - 1] = new Nhom3So(sb.ToString());
                Array.Reverse(nhomso);
            }
            else
            {
                for (int i = 0; i < DemSoNhom(so); i++)
                {
                    Stack<char> temp = new Stack<char>();
                    for (int j = so.Length - (1 + i * 3); j >= so.Length - (3 + i * 3); j--)
                        temp.Push(so[j]);
                    Array.Resize<Nhom3So>(ref nhomso, nhomso.Length + 1);
                    StringBuilder sb = new StringBuilder();
                    while (temp.Count > 0)
                        sb.Append(temp.Pop());
                    nhomso[nhomso.Length - 1] = new Nhom3So(sb.ToString());
                }
                Stack<char> temp2 = new Stack<char>();
                for (int i = so.Length - (1 + DemSoNhom(so) * 3); i >= 0; i--)
                    temp2.Push(so[i]);
                Array.Resize<Nhom3So>(ref nhomso, nhomso.Length + 1);
                StringBuilder sb2 = new StringBuilder();
                while (temp2.Count > 0)
                    sb2.Append(temp2.Pop());
                nhomso[nhomso.Length - 1] = new Nhom3So(sb2.ToString());
                Array.Reverse(nhomso);
            }
        }

        //Khởi tạo Hashtable dùng để đọc tên lớp nghìn, triệu, tỉ...
        int[] solop = { 0, 1, 2, 3 }; //Số nhóm đã đếm với DemSoNhom()
        string[] tenlop = { "", " nghìn ", " triệu ", " tỉ " };

        public Dictionary<int, string> hlop = new Dictionary<int, string>();
        private void KhoiTao_hlop()
        {
            for (int i = 0; i <= solop.Length - 1; i++)
                hlop.Add(solop[i], tenlop[i]);
        }

        //Cách đọc lớp
        private string DocLop(int so) //Nhập vào CHỈ SỐ (INDEX) của nhóm (tính từ phải sang trái)
        {
            if (so <= 3)
                return hlop[so].ToString();
            else
                if (so % 3 == 0)
                return hlop[3].ToString();
            else
                return hlop[so % 3].ToString();
        }

        public bool BaSo0 = false;

        private void DocSo_Load()
        {
            KhoiTao_h1_hdv();
            KhoiTao_hlop();
        }

        //Bỏ các số 0 vô nghĩa bên trái cùng
        private string Boso0(string so)
        {
            while (so[0] == '0')
            {
                string temp = "";
                for (int i = 1; i < so.Length; i++)
                    temp += so[i];

                if (so.Length > 1)
                    so = temp;
            }
            return so;
        }

        //Định dạng chuỗi "không có khoảng trắng thừa, viết hoa kí tự đầu tiên"
        private string DinhDang(string s)
        {
            while (s[0] == ' ')
            {
                string temp = "";
                for (int i = 1; i < s.Length; i++)
                    temp += s[i];
                s = temp;
            }
            //while (s.IndexOf("  ") !=-1)
            //    s.Replace("  ", " ");

            string temp2 = "";
            temp2 += char.ToUpper(s[0]);
            for (int i = 1; i < s.Length; i++)
                temp2 += s[i];
            s = temp2;
            return s + " đồng";
        }

        public string ConvertMoneyNumberToString(string moneyNumber)
        {
            if (string.IsNullOrEmpty(moneyNumber)) return "";

            Array.Resize<Nhom3So>(ref nhomso, 0);
            string stringSo = Boso0(moneyNumber);
            TachNhom(stringSo);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nhomso.Length; i++)
            {
                if (i == 0 && i == DemSoNhom(stringSo))
                    sb.Append(Doc3so(nhomso[i], true, true));
                else
                    if (i == 0 && i != DemSoNhom(stringSo))
                    sb.Append(Doc3so(nhomso[i], true, false));
                else
                        if (i != 0 && i == DemSoNhom(stringSo))
                    sb.Append(Doc3so(nhomso[i], false, true));
                else
                    sb.Append(Doc3so(nhomso[i], false, false));
                if (!BaSo0)
                    sb.Append(DocLop(DemSoNhom(stringSo) - i));
            }
            return DinhDang(sb.ToString());
        }

    }
}
