using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Security.Cryptography;

namespace Eagle.Common.Utilities
{
    public static class StringUtils
    {
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Convert string "on" to Bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ConvertOnOffToBool(object obj)
        {
            if (obj == null)
                return false;

            if ((obj.ToString().ToLower() == "on") || (obj.ToString().ToLower() == "true"))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Replace last occurance of string in a main string
        /// </summary>
        /// <param name="source">Main String</param>
        /// <param name="find">What To Search</param>
        /// <param name="replace">What To Replace</param>
        /// <returns>Replaced String </returns>
        public static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find, StringComparison.Ordinal);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string FormatDays(double mDays)
        {
            string ret = string.Empty;
            int year = 0;
            int month = 0;
            int weeak = 0;
            int days = 0;
            int reminder;
            if (mDays > 360 || (int)mDays == 360)
            {
                reminder = (int)mDays % 360;
                year = (int)mDays / 360;
                if (reminder > 30 || reminder == 30)
                {

                    month = reminder / 30;
                    reminder = reminder % 30;
                    if (reminder > 7)
                    {
                        weeak = reminder / 7;
                        reminder = reminder % 7;
                        if (reminder > 0)
                        {
                            days = reminder;
                        }
                    }
                    else
                    {
                        days = reminder;
                    }
                }
                else
                {
                    if (reminder > 7)
                    {
                        weeak = reminder / 7;
                        reminder = reminder % 7;
                        if (reminder > 0)
                        {
                            days = reminder;
                        }
                    }
                    else
                    {
                        days = reminder;
                    }
                }
            }
            else if (mDays > 30 || (int)mDays == 30)
            {
                reminder = (int)mDays % 30;
                month = (int)mDays / 30;
                if (reminder > 7 || reminder == 7)
                {
                    weeak = reminder / 7;
                    reminder = reminder % 7;
                    if (reminder > 0)
                    {
                        days = reminder;
                    }
                }
                else
                {
                    days = reminder;
                }
            }
            else if (mDays > 7 || (int)mDays == 7)
            {
                reminder = (int)mDays % 7;
                weeak = (int)mDays / 7;
                if (reminder > 0)
                {
                    days = reminder;
                }
            }
            else
            {
                days = (int)mDays;
            }

            if (year > 0)
            {
                if (year > 1)
                    ret += year + " Years  ";
                else
                    ret += year + " Year  ";

            }
            if (month > 0)
            {
                if (month > 1)
                    ret += month + " Months  ";
                else
                    ret += month + " Month  ";
            }
            if (weeak > 0)
            {
                if (weeak > 1)
                    ret += weeak + " Weeaks  ";
                else
                    ret += weeak + " Weeak  ";
            }
            if (days > 0)
            {
                if (days > 1)
                    ret += days + " Days  ";
                else
                    ret += days + " Day  ";
            }
            return ret;
        }

        public static string FormatRating(int rateValue)
        {
            string ret = string.Empty;
            switch (rateValue)
            {
                case 1:
                    ret = "Basic level";
                    break;
                case 2:
                    ret = "Normal";
                    break;
                case 3:
                    ret = "Good";
                    break;
                case 4:
                    ret = "Advance";
                    break;
                case 5:
                    ret = "Fluient";
                    break;
            }
            return ret;
        }

        public static string FormatbitValue(string bitValue)
        {
            string ret = "No";
            if (bitValue == "1")
            {
                ret = "Yes";
            }
            return ret;
        }

        public static bool CheckIgnorWords(string searchString)
        {
            var IgnorWords = "^ , ; : [] ] [ {} () } { ) ( _ = < > . + - \\ / \" \"\" ' ! % * @~ @# @& &? & # ? about 1 after 2 all also 3 an 4 and 5 another 6 any 7 are 8 as 9 at 0 be $ because been before being between both but by came can come could did do each for from get got has had he have her here him himself his how if in into is it like make many me might more most much must my never now of on only or other our out over said same see should since some still such take than that the their them then there these they this those through to too under up very was way we well were what where which while who with would you your a b c d e f g h i j k l m n o p q r s t u v w x y z";
            searchString = RemoveSpecialSymbols(searchString);
            if (IgnorWords.Contains(searchString) || searchString.Trim() == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string RemoveUnnessaryKeywords(string searchString)
        {
            searchString = RemoveSpecialSymbols(searchString);
            return searchString;
        }

        public static string RemoveSpecialSymbols(string searchString)
        {
            searchString = searchString.Replace("\"", "");
            searchString = searchString.Replace("@", "");
            searchString = searchString.Replace("?", "");
            searchString = searchString.Replace(":", "");
            searchString = searchString.Replace(";", "");
            searchString = searchString.Replace("_", "");
            searchString = searchString.Replace("=", "");
            searchString = searchString.Replace("<", "");
            searchString = searchString.Replace(">", "");
            searchString = searchString.Replace("[", "");
            searchString = searchString.Replace("]", "");
            searchString = searchString.Replace("{", "");
            searchString = searchString.Replace("}", "");
            searchString = searchString.Replace("!", "");
            searchString = searchString.Replace("#", "");
            searchString = searchString.Replace(",", "");
            searchString = searchString.Replace("-", "");
            searchString = searchString.Replace(".", "");
            searchString = searchString.Replace("^", "");
            searchString = searchString.Replace("(", "");
            searchString = searchString.Replace(")", "");
            searchString = searchString.Replace("/", "");
            searchString = searchString.Replace("~", "");
            searchString = searchString.Replace("|", "");
            searchString = searchString.Replace("$", "");
            searchString = searchString.Replace("%", "");
            searchString = searchString.Replace("&", "");
            searchString = searchString.Replace("*", "");
            searchString = searchString.Replace("and", "");
            return searchString;
        }

        public static string Trim(string info)
        {
            if (String.IsNullOrEmpty(info) || String.IsNullOrWhiteSpace(info))
            {
                return null;
            }
            return info.Trim();
        }
        public static string EncodePassword(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }
            byte[] binarySource = Encoding.UTF8.GetBytes(source);
            var rijn = SymmetricAlgorithm.Create();
            MemoryStream ms = new MemoryStream();
            byte[] rgbIv = Encoding.ASCII.GetBytes("lkjhasdfyuiwhcnt");
            byte[] key = Encoding.ASCII.GetBytes("tkw123aaaa");
            CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIv), CryptoStreamMode.Write);
            cs.Write(binarySource, 0, binarySource.Length);
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecodePassword(string source)
        {
            byte[] binarySource = Convert.FromBase64String(source);
            MemoryStream ms = new MemoryStream();
            SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();
            byte[] rgbIv = Encoding.ASCII.GetBytes("lkjhasdfyuiwhcnt");
            byte[] key = Encoding.ASCII.GetBytes("tkw123aaaa");
            CryptoStream cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIv),
            CryptoStreamMode.Write);
            cs.Write(binarySource, 0, binarySource.Length);
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        //Get MD5
        public static string GetMd5Sum(string str)
        {
            // First we need to convert the string into bytes, which
            // means using a text encoder.

            Encoder enc = Encoding.Unicode.GetEncoder();
            // Create a buffer large enough to hold the string

            byte[] unicodeText = new byte[str.Length * 2];

            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it

            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte

            // into hex and appending it to a StringBuilder

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {

                sb.Append(result[i].ToString("X2"));

            }
            // And return it
            return sb.ToString();
        }

        public static string[] ConvertStringToArray(string strInput)
        {
            string[] arrayString = strInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return arrayString;
        }

        public static string ConvertToNoMarkString(string text)
        {
            //Ky tu dac biet

            for (int i = 32; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "-");
            }
            text = text.Replace(".", "");
            text = text.Replace("?", "");
            text = text.Replace("\"", "");
            text = text.Replace(" ", "-");
            text = text.Replace(",", "-");
            text = text.Replace(";", "-");
            text = text.Replace(":", "-");

            text = text.Replace("–", "");
            text = text.Replace("“", "");
            text = text.Replace("”", "");

            text = text.Replace("(", "-");
            text = text.Replace(")", "-");
            text = text.Replace("@", "-");
            text = text.Replace("&", "-");
            text = text.Replace("*", "-");
            text = text.Replace("\\", "-");
            text = text.Replace("+", "-");
            text = text.Replace("/", "-");
            text = text.Replace("#", "-");
            text = text.Replace("$", "-");
            text = text.Replace("%", "-");
            text = text.Replace("^", "-");
            text = text.Replace("--", "-");
            text = text.Replace("--", "-");
            if (text.Substring(0, 1) == "-")
            {
                text = text.Substring(1);
            }
            if (text.Substring(text.Length - 1) == "-")
            {
                text = text.Substring(0, text.Length - 1);
            }
            //'Dấu Ngang
            text = text.Replace("A", "A");
            text = text.Replace("a", "a");
            text = text.Replace("Ă", "A");
            text = text.Replace("ă", "a");
            text = text.Replace("Â", "A");
            text = text.Replace("â", "a");
            text = text.Replace("E", "E");
            text = text.Replace("e", "e");
            text = text.Replace("Ê", "E");
            text = text.Replace("ê", "e");
            text = text.Replace("I", "I");
            text = text.Replace("i", "i");
            text = text.Replace("O", "O");
            text = text.Replace("o", "o");
            text = text.Replace("Ô", "O");
            text = text.Replace("ô", "o");
            text = text.Replace("Ơ", "O");
            text = text.Replace("ơ", "o");
            text = text.Replace("U", "U");
            text = text.Replace("u", "u");
            text = text.Replace("Ư", "U");
            text = text.Replace("ư", "u");
            text = text.Replace("Y", "Y");
            text = text.Replace("y", "y");

            //    'Dấu Huyền
            text = text.Replace("À", "A");
            text = text.Replace("à", "a");
            text = text.Replace("Ằ", "A");
            text = text.Replace("ằ", "a");
            text = text.Replace("Ầ", "A");
            text = text.Replace("ầ", "a");
            text = text.Replace("È", "E");
            text = text.Replace("è", "e");
            text = text.Replace("Ề", "E");
            text = text.Replace("ề", "e");
            text = text.Replace("Ì", "I");
            text = text.Replace("ì", "i");
            text = text.Replace("Ò", "O");
            text = text.Replace("ò", "o");
            text = text.Replace("Ồ", "O");
            text = text.Replace("ồ", "o");
            text = text.Replace("Ờ", "O");
            text = text.Replace("ờ", "o");
            text = text.Replace("Ù", "U");
            text = text.Replace("ù", "u");
            text = text.Replace("Ừ", "U");
            text = text.Replace("ừ", "u");
            text = text.Replace("Ỳ", "Y");
            text = text.Replace("ỳ", "y");

            //'Dấu Sắc
            text = text.Replace("Á", "A");
            text = text.Replace("á", "a");
            text = text.Replace("Ắ", "A");
            text = text.Replace("ắ", "a");
            text = text.Replace("Ấ", "A");
            text = text.Replace("ấ", "a");
            text = text.Replace("É", "E");
            text = text.Replace("é", "e");
            text = text.Replace("Ế", "E");
            text = text.Replace("ế", "e");
            text = text.Replace("Í", "I");
            text = text.Replace("í", "i");
            text = text.Replace("Ó", "O");
            text = text.Replace("ó", "o");
            text = text.Replace("Ố", "O");
            text = text.Replace("ố", "o");
            text = text.Replace("Ớ", "O");
            text = text.Replace("ớ", "o");
            text = text.Replace("Ú", "U");
            text = text.Replace("ú", "u");
            text = text.Replace("Ứ", "U");
            text = text.Replace("ứ", "u");
            text = text.Replace("Ý", "Y");
            text = text.Replace("ý", "y");

            //'Dấu Hỏi
            text = text.Replace("Ả", "A");
            text = text.Replace("ả", "a");
            text = text.Replace("Ẳ", "A");
            text = text.Replace("ẳ", "a");
            text = text.Replace("Ẩ", "A");
            text = text.Replace("ẩ", "a");
            text = text.Replace("Ẻ", "E");
            text = text.Replace("ẻ", "e");
            text = text.Replace("Ể", "E");
            text = text.Replace("ể", "e");
            text = text.Replace("Ỉ", "I");
            text = text.Replace("ỉ", "i");
            text = text.Replace("Ỏ", "O");
            text = text.Replace("ỏ", "o");
            text = text.Replace("Ổ", "O");
            text = text.Replace("ổ", "o");
            text = text.Replace("Ở", "O");
            text = text.Replace("ở", "o");
            text = text.Replace("Ủ", "U");
            text = text.Replace("ủ", "u");
            text = text.Replace("Ử", "U");
            text = text.Replace("ử", "u");
            text = text.Replace("Ỷ", "Y");
            text = text.Replace("ỷ", "y");

            //'Dấu Ngã   
            text = text.Replace("Ã", "A");
            text = text.Replace("ã", "a");
            text = text.Replace("Ẵ", "A");
            text = text.Replace("ẵ", "a");
            text = text.Replace("Ẫ", "A");
            text = text.Replace("ẫ", "a");
            text = text.Replace("Ẽ", "E");
            text = text.Replace("ẽ", "e");
            text = text.Replace("Ễ", "E");
            text = text.Replace("ễ", "e");
            text = text.Replace("Ĩ", "I");
            text = text.Replace("ĩ", "i");
            text = text.Replace("Õ", "O");
            text = text.Replace("õ", "o");
            text = text.Replace("Ỗ", "O");
            text = text.Replace("ỗ", "o");
            text = text.Replace("Ỡ", "O");
            text = text.Replace("ỡ", "o");
            text = text.Replace("Ũ", "U");
            text = text.Replace("ũ", "u");
            text = text.Replace("Ữ", "U");
            text = text.Replace("ữ", "u");
            text = text.Replace("Ỹ", "Y");
            text = text.Replace("ỹ", "y");

            //'Dẫu Nặng
            text = text.Replace("Ạ", "A");
            text = text.Replace("ạ", "a");
            text = text.Replace("Ặ", "A");
            text = text.Replace("ặ", "a");
            text = text.Replace("Ậ", "A");
            text = text.Replace("ậ", "a");
            text = text.Replace("Ẹ", "E");
            text = text.Replace("ẹ", "e");
            text = text.Replace("Ệ", "E");
            text = text.Replace("ệ", "e");
            text = text.Replace("Ị", "I");
            text = text.Replace("ị", "i");
            text = text.Replace("Ọ", "O");
            text = text.Replace("ọ", "o");
            text = text.Replace("Ộ", "O");
            text = text.Replace("ộ", "o");
            text = text.Replace("Ợ", "O");
            text = text.Replace("ợ", "o");
            text = text.Replace("Ụ", "U");
            text = text.Replace("ụ", "u");
            text = text.Replace("Ự", "U");
            text = text.Replace("ự", "u");
            text = text.Replace("Ỵ", "Y");
            text = text.Replace("ỵ", "y");
            text = text.Replace("Đ", "D");
            text = text.Replace("đ", "d");
        
            return text.ToLower();

        }

        public static bool CheckValidName(string strInput){
	        string pattern = @"[^a-zA-Z\s]$";
	        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
	        Match match = regex.Match(strInput);
	        int matchCount = 0;
            if (match.Success) 
	           ++matchCount;
            var isValid = matchCount <= 0;
	         return isValid;
        }
        public static string ConvertToUnSign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');

            sb = sb.Replace('á', 'a');
            sb = sb.Replace('à', 'a');
            sb = sb.Replace('ả', 'a');
            sb = sb.Replace('ã', 'a');
            sb = sb.Replace('ạ', 'a');

            sb = sb.Replace('ă', 'a');
            sb = sb.Replace('ắ', 'a');
            sb = sb.Replace('ằ', 'a');
            sb = sb.Replace('ẳ', 'a');
            sb = sb.Replace('ẵ', 'a');
            sb = sb.Replace('ặ', 'a');

            sb = sb.Replace('é', 'e');
            sb = sb.Replace('è', 'e');
            sb = sb.Replace('ẻ', 'e');
            sb = sb.Replace('ẽ', 'e');
            sb = sb.Replace('ẹ', 'e');

            sb = sb.Replace('ê', 'e');
            sb = sb.Replace('ế', 'e');
            sb = sb.Replace('ề', 'e');
            sb = sb.Replace('ể', 'e');
            sb = sb.Replace('ễ', 'e');
            sb = sb.Replace('ệ', 'e');


            sb = sb.Replace('í', 'i');
            sb = sb.Replace('ì', 'i');
            sb = sb.Replace('ỉ', 'i');
            sb = sb.Replace('ĩ', 'i');
            sb = sb.Replace('ị', 'i');

            sb = sb.Replace('ó', 'o');
            sb = sb.Replace('ò', 'o');
            sb = sb.Replace('ỏ', 'o');
            sb = sb.Replace('õ', 'o');
            sb = sb.Replace('ọ', 'o');

            sb = sb.Replace('ô', 'o');
            sb = sb.Replace('ố', 'o');
            sb = sb.Replace('ồ', 'o');
            sb = sb.Replace('ổ', 'o');
            sb = sb.Replace('ỗ', 'o');
            sb = sb.Replace('ộ', 'o');

            sb = sb.Replace('ú', 'u');
            sb = sb.Replace('ù', 'u');
            sb = sb.Replace('ủ', 'u');
            sb = sb.Replace('ũ', 'u');
            sb = sb.Replace('ụ', 'u');

            sb = sb.Replace('ý', 'y');
            sb = sb.Replace('ỳ', 'y');
            sb = sb.Replace('ỷ', 'y');
            sb = sb.Replace('ỹ', 'y');
            sb = sb.Replace('ỵ', 'y');

            //Capital letter
            sb = sb.Replace('Á', 'A');
            sb = sb.Replace('À', 'A');
            sb = sb.Replace('Ả', 'A');
            sb = sb.Replace('Ã', 'A');
            sb = sb.Replace('Ạ', 'A');

            sb = sb.Replace('Ă', 'A');
            sb = sb.Replace('Ắ', 'A');
            sb = sb.Replace('Ằ', 'A');
            sb = sb.Replace('Ẳ', 'A');
            sb = sb.Replace('Ẵ', 'A');
            sb = sb.Replace('Ặ', 'A');

            sb = sb.Replace('É', 'E');
            sb = sb.Replace('È', 'E');
            sb = sb.Replace('Ẻ', 'E');
            sb = sb.Replace('Ẽ', 'E');
            sb = sb.Replace('Ẹ', 'E');

            sb = sb.Replace('Ê', 'E');
            sb = sb.Replace('Ế', 'E');
            sb = sb.Replace('Ề', 'E');
            sb = sb.Replace('Ể', 'E');
            sb = sb.Replace('Ễ', 'E');
            sb = sb.Replace('Ệ', 'E');

            sb = sb.Replace('Í', 'I');
            sb = sb.Replace('Ì', 'I');
            sb = sb.Replace('Ỉ', 'I');
            sb = sb.Replace('Ĩ', 'I');
            sb = sb.Replace('Ị', 'I');

            sb = sb.Replace('Ó', 'O');
            sb = sb.Replace('Ò', 'O');
            sb = sb.Replace('Ỏ', 'O');
            sb = sb.Replace('Õ', 'O');
            sb = sb.Replace('Ọ', 'O');

            sb = sb.Replace('Ô', 'O');
            sb = sb.Replace('Ố', 'O');
            sb = sb.Replace('Ồ', 'O');
            sb = sb.Replace('Ổ', 'O');
            sb = sb.Replace('Ỗ', 'O');
            sb = sb.Replace('Ộ', 'O');


            sb = sb.Replace('Ú', 'U');
            sb = sb.Replace('Ù', 'U');
            sb = sb.Replace('Ủ', 'U');
            sb = sb.Replace('Ũ', 'U');
            sb = sb.Replace('Ụ', 'U');

            sb = sb.Replace('Ý', 'Y');
            sb = sb.Replace('Ỳ', 'Y');
            sb = sb.Replace('Ỷ', 'Y');
            sb = sb.Replace('Ỹ', 'Y');
            sb = sb.Replace('Ỵ', 'Y');

            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static string CutSubString(string content, int num)
        {
            content = RemoveHtmlTag(content, "");
            content = CutLimitText(content, num);
            return content;
        }

        public static string CreateTags(string tags)
        {
            var sb = new StringBuilder(tags.ToLower());
            sb.Replace("\"", " ");
            sb.Replace(",", " ");
            sb.Replace("&amp", " ");
            sb.Replace("&", " ");
            string result = UTF8_Encode(RemoveHtmlTag(sb.ToString(), ""));
            return result;
        }

        public static string ConvertTitle2Alias(string title)
        {
            string strSource = RemoveHtmlTag(title, "");

            StringBuilder sb = new StringBuilder(strSource.ToLower().TrimStart());
            sb.Replace(",", "-");
            sb.Replace(' ', '-');
            sb.Replace(":", "");
            sb.Replace("\"", "");
            sb.Replace("%", "");
            sb.Replace("?", "");
            sb.Replace("&amp", "-");
            sb.Replace("&", "-");
            sb.Replace("--", "-");
            var result = RemoveSign4VietnameseString(sb.ToString());
            return result;
        }

        public static string ConvertTitle2Link(string title)
        {
            string strSource = RemoveHtmlTag(title, "");

            StringBuilder sb = new StringBuilder(strSource.ToLower().TrimStart());
            sb.Replace("-", "");
            sb.Replace(' ', '-');
            sb.Replace(":", "");
            sb.Replace("\"", "");
            sb.Replace("%", "");
            sb.Replace("?", "");
            sb.Replace("&amp", "-");
            sb.Replace("&", "-");
            sb.Replace("--", "-");
            var result = RemoveSign4VietnameseString(sb.ToString());
            return result;
        }

        public static string ConvertName2Link(string title)
        {
            string strSource = RemoveHtmlTag(title, "");
            StringBuilder sb = new StringBuilder(strSource.ToLower());
            sb.Replace(' ', '_');
            sb.Replace("\"", "");
            sb.Replace(",", "_");
            sb.Replace("&amp", "_");
            sb.Replace("&", "_");
            var result = RemoveSign4VietnameseString(sb.ToString());
            return result;
        }

        public static string ConvertVniToUnicode(string vni)
        {
            string sUnicode = "";
            if (vni.Length == 0) return "";
            var iChieuDai = vni.Length - 1;
            int i = 0;
            while (i <= iChieuDai)
            {
                switch ((byte)vni[i])
                {
                    case 97: //, 101, 105, 111, 117 'a,e,i,o,u
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "á"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "à"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ả"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "ã"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ạ"; i += 2; //dấu nặng
                                    break;
                                case 226: sUnicode += "â"; i += 2; //^
                                    break;
                                case 225: sUnicode += "ấ"; i += 2; //ấ
                                    break;
                                case 224: sUnicode += "ầ"; i += 2; //ầ
                                    break;
                                case 229: sUnicode += "ẩ"; i += 2; //ẩ
                                    break;
                                case 227: sUnicode += "ẫ"; i += 2; //ẫ
                                    break;
                                case 228: sUnicode += "ậ"; i += 2; //ậ
                                    break;
                                case 234: sUnicode += "ă"; i += 2; //ă
                                    break;
                                case 233: sUnicode += "ắ"; i += 2; //ắ
                                    break;
                                case 232: sUnicode += "ằ"; i += 2; //ằ
                                    break;
                                case 250: sUnicode += "ẳ"; i += 2; //ẳ
                                    break;
                                case 252: sUnicode += "ẵ"; i += 2; //ẵ
                                    break;
                                case 235: sUnicode += "ặ"; i += 2; //ặ
                                    break;
                                default: sUnicode += "a"; i += 1;
                                    break;
                            }
                        }
                        else
                        {
                            sUnicode += "a"; i += 1;
                        }
                        break;
                    case 65: //A
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "Á"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "À"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ả"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Ã"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ạ"; i += 2; //dấu nặng
                                    break;
                                case 194: sUnicode += "Â"; i += 2; //^
                                    break;
                                case 193: sUnicode += "Ấ"; i += 2; //ấ
                                    break;
                                case 192: sUnicode += "Ầ"; i += 2; //ầ
                                    break;
                                case 197: sUnicode += "Ẩ"; i += 2; //ẩ
                                    break;
                                case 195: sUnicode += "Ẫ"; i += 2; //ẫ
                                    break;
                                case 196: sUnicode += "Ậ"; i += 2; //ậ
                                    break;
                                case 202: sUnicode += "Ă"; i += 2; //ă
                                    break;
                                case 201: sUnicode += "Ắ"; i += 2; //ắ
                                    break;
                                case 200: sUnicode += "Ằ"; i += 2; //ằ
                                    break;
                                case 218: sUnicode += "Ẳ"; i += 2; //ẳ
                                    break;
                                case 220: sUnicode += "Ẵ"; i += 2; //ẵ
                                    break;
                                case 203: sUnicode += "Ặ"; i += 2; //ặ
                                    break;
                                //Trường hợp bị lỗi
                                case 249: sUnicode += "Á"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "À"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "Ả"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "Ã"; i += 2;  //dấu ngã
                                    break;
                                case 239: sUnicode += "Ạ"; i += 2; //dấu nặng
                                    break;
                                case 226: sUnicode += "Â"; i += 2; //^
                                    break;
                                case 225: sUnicode += "Ấ"; i += 2; //ấ
                                    break;
                                case 224: sUnicode += "Ầ"; i += 2; //ầ
                                    break;
                                case 229: sUnicode += "Ẩ"; i += 2; //ẩ
                                    break;
                                case 227: sUnicode += "Ẫ"; i += 2; //ẫ
                                    break;
                                case 228: sUnicode += "Ậ"; i += 2; //ậ
                                    break;
                                case 234: sUnicode += "Ă"; i += 2; //ă
                                    break;
                                case 233: sUnicode += "Ắ"; i += 2; //ắ
                                    break;
                                case 232: sUnicode += "Ằ"; i += 2; //ằ
                                    break;
                                case 250: sUnicode += "Ẳ"; i += 2; //ẳ
                                    break;
                                case 252: sUnicode += "Ẵ"; i += 2; //ẵ
                                    break;
                                case 235: sUnicode += "Ặ"; i += 2; //ặ
                                    break;
                                default: sUnicode += "A"; i += 1; break;
                            }
                        }
                        else { sUnicode += "A"; i += 1; } break;
                    case 101: //e
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "é"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "è"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ẻ"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "ẽ"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ẹ"; i += 2; //dấu nặng
                                    break;
                                case 226: sUnicode += "ê"; i += 2; //^
                                    break;
                                case 225: sUnicode += "ế"; i += 2; //ấ
                                    break;
                                case 224: sUnicode += "ề"; i += 2; //ầ
                                    break;
                                case 229: sUnicode += "ể"; i += 2; //ẩ
                                    break;
                                case 227: sUnicode += "ễ"; i += 2; //ẫ
                                    break;
                                case 228: sUnicode += "ệ"; i += 2; //ậ
                                    break;
                                default: sUnicode += "e"; i += 1;
                                    break;
                            }
                        }
                        else { sUnicode += "e"; i += 1; } break;
                    case 69:
                        //E
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "É"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "È"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ẻ"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Ẽ"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ẹ"; i += 2; //dấu nặng
                                    break;
                                case 194: sUnicode += "Ê"; i += 2; //^
                                    break;
                                case 193: sUnicode += "Ế"; i += 2; //ấ
                                    break;
                                case 192: sUnicode += "Ề"; i += 2; //ầ
                                    break;
                                case 197: sUnicode += "Ể"; i += 2; //ẩ
                                    break;
                                case 195: sUnicode += "Ễ"; i += 2; //ẫ
                                    break;
                                case 196: sUnicode += "Ệ"; i += 2; //ậ
                                    break;
                                default: sUnicode += "E"; i += 1;
                                    break;
                            }
                        }
                        else { sUnicode += "E"; i += 1; } break;
                    case 111:
                        //o
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "ó"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "ò"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ỏ"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "õ"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ọ"; i += 2; //dấu nặng
                                    break;
                                case 226: sUnicode += "ô"; i += 2; //^
                                    break;
                                case 225: sUnicode += "ố"; i += 2; //ấ
                                    break;
                                case 224: sUnicode += "ồ"; i += 2; //ầ
                                    break;
                                case 229: sUnicode += "ổ"; i += 2; //ẩ
                                    break;
                                case 227: sUnicode += "ỗ"; i += 2; //ẫ
                                    break;
                                case 228: sUnicode += "ộ"; i += 2; //ậ
                                    break;
                                default: sUnicode += "o"; i += 1;
                                    break;
                            }
                        }
                        else { sUnicode += "o"; i += 1; } break;
                    case 79: //O
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "Ó"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "Ò"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ỏ"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Õ"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ọ"; i += 2; //dấu nặng
                                    break;
                                case 194: sUnicode += "Ô"; i += 2; //^
                                    break;
                                case 193: sUnicode += "Ố"; i += 2; //ấ
                                    break;
                                case 192: sUnicode += "Ồ"; i += 2; //ầ
                                    break;
                                case 197: sUnicode += "Ổ"; i += 2; //ẩ
                                    break;
                                case 195: sUnicode += "Ỗ"; i += 2; //ẫ
                                    break;
                                case 196: sUnicode += "Ộ"; i += 2; //ậ
                                    break;
                                default: sUnicode += "O"; i += 1; break;
                            }
                        }
                        else { sUnicode += "O"; i += 1; } break;
                    case 117: //u
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "ú"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "ù"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ủ"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "ũ"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ụ"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "u"; i += 1; break;
                            }
                        }
                        else { sUnicode += "u"; i += 1; } break;
                    case 85: //U
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "Ú"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "Ù"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ủ"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Ũ"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ụ"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "U"; i += 1; break;
                            }
                        }
                        else { sUnicode += "U"; i += 1; } break;
                    case 244: //ơ
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "ớ"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "ờ"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ở"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "ỡ"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ợ"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "ơ"; i += 1; break;
                            }
                        }
                        else
                        {
                            sUnicode += "ơ";
                            i += 1;
                        }
                        break;
                    case 212: //Ơ
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "Ớ"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "Ờ"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ở"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Ỡ"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ợ"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "Ơ"; i += 1; break;
                            }
                        }
                        else { sUnicode += "Ơ"; i += 1; } break;
                    case 246: //ư
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "ứ"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "ừ"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ử"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "ữ"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ự"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "ư"; i += 1;
                                    break;
                            }
                        }
                        else { sUnicode += "ư"; i += 1; } break;
                    case 214: //Ư
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "Ứ"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "Ừ"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ử"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Ữ"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ự"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "Ư"; i += 1; break;
                            }
                        }
                        else { sUnicode += "Ư"; i += 1; } break;
                    case 121: //y
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 249: sUnicode += "ý"; i += 2; //dấu sắc
                                    break;
                                case 248: sUnicode += "ỳ"; i += 2; //dấu huyền
                                    break;
                                case 251: sUnicode += "ỷ"; i += 2; //dấu hỏi
                                    break;
                                case 245: sUnicode += "ỹ"; i += 2; //dấu ngã
                                    break;
                                case 239: sUnicode += "ỵ"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "y"; i += 1; break;
                            }
                        }
                        else { sUnicode += "y"; i += 1; }
                        break;
                    case 89: //Y
                        if (i < iChieuDai)
                        {
                            switch ((byte)vni[i + 1])
                            {
                                case 217: sUnicode += "Ý"; i += 2; //dấu sắc
                                    break;
                                case 216: sUnicode += "Ỳ"; i += 2; //dấu huyền
                                    break;
                                case 219: sUnicode += "Ỷ"; i += 2; //dấu hỏi
                                    break;
                                case 213: sUnicode += "Ỹ"; i += 2; //dấu ngã
                                    break;
                                case 207: sUnicode += "Ỵ"; i += 2; //dấu nặng
                                    break;
                                default: sUnicode += "Y"; i += 1; break;
                            }
                        }
                        else { sUnicode += "Y"; i += 1; } break;
                    case 237: sUnicode += "í"; i += 1; break;
                    case 236: sUnicode += "ì"; i += 1; break;
                    case 230: sUnicode += "ỉ"; i += 1; break;
                    case 243: sUnicode += "ĩ"; i += 1; break;
                    case 242: sUnicode += "ị"; i += 1; break;
                    case 205: sUnicode += "Í"; i += 1; break;
                    case 204: sUnicode += "Ì"; i += 1; break;
                    case 198: sUnicode += "Ỉ"; i += 1; break;
                    case 211: sUnicode += "Ĩ"; i += 1; break;
                    case 210: sUnicode += "Ị"; i += 1; break;
                    case 241: sUnicode += "đ"; i += 1; break;
                    case 209: sUnicode += "Đ"; i += 1; break;
                    case 238:
                    case 255: sUnicode += "ỵ"; i += 1; break;
                    case 159:
                    case 206: sUnicode += "Ỵ"; i += 1; break;
                    default:
                        sUnicode += vni[i]; i += 1; break;
                }
            }
            return sUnicode;
        }

        public static string CutLimitText(string source, int limit)
        {
            if (source.Length < limit)
            {
                return source;
            }
            else
            {
                var endtext2 = source.IndexOf(" ", limit, StringComparison.Ordinal);

                if (endtext2 > 0)
                {
                    source = source.Substring(0, endtext2);
                }
                else
                {
                    source = source + " ...";
                }
                return source;
            }
        }

        public static string FixCode(string html)
        {
            html = html.Replace("  ", "&nbsp; ");
            html = html.Replace("  ", " &nbsp;");
            html = html.Replace("\t", "&nbsp; &nbsp;&nbsp;");
            html = html.Replace("[", "&#91;");
            html = html.Replace("]", "&#93;");
            html = html.Replace("<", "&lt;");
            html = html.Replace(">", "&gt;");
            html = html.Replace("\r\n", "<br/>");
            return html;
        }

        public static string FormatDate(string strDate)
        {
            int pos = strDate.IndexOf(" ", StringComparison.Ordinal);
            string strRe = "";
            string strChar = "";
            if (pos > 0)
            {
                string temp = strDate.Substring(0, pos);
                string[] arrDate = temp.Split('/');

                for (int i = 0; i < arrDate.Length; i++)
                {
                    if (i != 0)
                    {
                        strChar = "-";
                    }
                    if (arrDate[i].Length == 1)
                    {
                        strRe += strChar + "0" + arrDate[i];
                    }
                    else if (arrDate[i].Length == 2)
                    {
                        strRe += strChar + arrDate[i];
                    }
                    else if (arrDate[i].Length >= 4)
                    {
                        strRe += strChar + arrDate[i].Substring(2, 2);
                    }
                }
            }
            else
            {
                strRe = strDate;
            }

            string[] fm = strRe.Split('-');
            if (fm.Length > 1)
                strRe = fm[1] + "-" + fm[0] + "-" + fm[2];
            return strRe;
        }

        public static string GetAlphanumericString(string s)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in s)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string GenerateCode(string strInput, int maxNumber)
        {
            if (strInput.Length <= maxNumber)
            {
                var total = maxNumber - strInput.Length;
                for (int i = 0; i < total; i++)
                {
                    strInput = "0" + strInput;
                }
            }
            return strInput;
        }

        public static string GenerateCodeWithMillisecond(string strInput, int maxNumber)
        {
            //strInput = String.Concat(strInput, "-", DateTime.UtcNow.Millisecond.ToString());
            if (string.IsNullOrEmpty(strInput)) return null;
            strInput = String.Concat(strInput, DateTime.UtcNow.Millisecond.ToString());
            int strLength = strInput.Length;
            if (strLength > maxNumber)
            {
                strInput = strInput.Substring(0, maxNumber);
            }
            else
            {
                var total = maxNumber - strInput.Length;
                for (int i = 0; i < total; i++)
                {
                    strInput = "0" + strInput;
                }
            }
            return strInput;
        }

        public static string GetEncodeString(string strInput)
        {
            //Random String 
            Random rnd = new Random();
            string strRandom = rnd.Next(10000, 99999) + GetRandomString();

            strInput = new Regex(@"[\s+\\\/:\*\?\&""\''<>|]").Replace(ConvertToUnSign(strInput), string.Empty);
            string encodeResult = strRandom + "_" + strInput;
            return encodeResult;
        }

        public static string GetEncodeStringWithDate(string strInput)
        {
            //Current Date
            CultureInfo culture = new CultureInfo("en-US")
            {
                DateTimeFormat =
                {
                    DateSeparator = string.Empty,
                    ShortDatePattern = "yyyyMMdd",
                    LongDatePattern = "yyyyMMdd"
                }
            };
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            string currentDateYyyymmddHhmmssMmm = DateTime.UtcNow.ToString("yyyyMMdd_hhmmss") + DateTime.UtcNow.Millisecond.ToString();

            //Random String 
            Random rnd = new Random();
            string strRandom = rnd.Next(10000, 99999) + GetRandomString();

            strInput = new Regex(@"[\s+\\\/:\*\?\&""\''<>|]").Replace(ConvertToUnSign(strInput), string.Empty);
            string encodeResult = currentDateYyyymmddHhmmssMmm + "_" + strRandom + "_" + strInput;
            return encodeResult;
        }

        public static string CreateEncodedStringWithDateGuid(string strInput)
        {
            //Current Date
            CultureInfo culture = new CultureInfo("en-US")
            {
                DateTimeFormat =
                {
                    DateSeparator = string.Empty,
                    ShortDatePattern = "yyyyMMdd",
                    LongDatePattern = "yyyyMMdd"
                }
            };
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            string currentDateYyyymmddHhmmssMmm = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss") + DateTime.UtcNow.Millisecond.ToString();

            strInput = new Regex(@"[\s+\\\/:\*\?\@\&\#\$\^\%\--\+\;\,\“\”\""\''<>|]").Replace(ConvertToUnSign(strInput), string.Empty);
            string encodeResult = currentDateYyyymmddHhmmssMmm + "-" + Guid.NewGuid() + "-" + strInput;
            return encodeResult;
        }

        public static string GetFirstCapitalizedLetterOfWords(string strInput)
        {
            string result = null;
            string[] words = strInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                //first_capitialized_letter_of_words
                result += word.Substring(0, 1).ToUpper();

            }
            return result;
        }

        public static string GeneratePageTitle(string title, string rootTitle, string pathSeparator)
        {
            List<string> path = new List<string>();

            var currentNode = SiteMap.CurrentNode;

            if (currentNode == null)
            {
                if (String.IsNullOrEmpty(title))
                {
                    return rootTitle;
                }

                return title + pathSeparator + rootTitle;
            }

            if (!Equals(currentNode, SiteMap.RootNode))
            {
                if (!String.IsNullOrEmpty(title) && title != rootTitle)
                {
                    path.Add(title);
                }
                else
                {
                    path.Add(currentNode.Title);
                }
            }
            else
            {
                path.Add(rootTitle);
            }

            currentNode = currentNode.ParentNode;

            while (!(currentNode == null))
            {

                // Use our own root title for the <title> tag
                if (currentNode.Title != SiteMap.RootNode.Title)
                {
                    path.Add(currentNode.Title);
                }
                else
                {
                    path.Add(rootTitle);
                }

                currentNode = currentNode.ParentNode;
            }

            string[] paths = path.ToArray();

            return string.Join(pathSeparator, paths);

        }

        public static string GetRandom(int min, int max)
        {
            Random random = new Random();
            int result = random.Next(min, max);
            var builder = new StringBuilder();
            builder.Append(result);
            return builder.ToString();
        }

        public static string GenerateRandomString(int size)
        {

            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString().ToLower();
        }

        public static string RandomString(int size)
        {
            string strCaptcha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string strTemp = "";
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                int iRan = random.Next(0, 61);
                strTemp += strCaptcha.Substring(iRan, 1);
            }
            return strTemp;
        }

        /// <summary>
        /// Creates a securely random string of any length
        /// </summary>
        /// <param name="length"></param>
        /// <param name="allowedChars"></param>
        /// <returns></returns>
        public static string RandomString(int length, string allowedChars)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars))
            {
                allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            }

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(string.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
        
        public static string GetRandomString()
        {
            //use the following string to control your set of alphabetic characters to choose from
            //for example, you could include uppercase too
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            // Random is not truly random, 
            // so we try to encourage better randomness by always changing the seed value
            Random rnd = new Random(DateTime.UtcNow.Millisecond);

            // basic 5 digit random number
            string result = rnd.Next(10000, 99999).ToString();

            // single random character in ascii range a-z
            string alphaChar = alphabet.Substring(rnd.Next(0, alphabet.Length - 1), 1);

            // random position to put the alpha character
            int replacementIndex = rnd.Next(0, (result.Length - 1));
            result = result.Remove(replacementIndex, 1).Insert(replacementIndex, alphaChar);

            return result;
        }

        public static string GetRandomString(int length)
        {
            int intCount = 0;
            Random rRandom = new Random(DateTime.UtcNow.Millisecond);

            var intZero = Convert.ToInt32('0');
            var intNine = Convert.ToInt32('9');

            var intA = Convert.ToInt32('A');
            var intZ = Convert.ToInt32('Z');
            var strRandomString = "";

            while (intCount < length)
            {
                var intRandomNumber = rRandom.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) & (intRandomNumber <= intNine)) | ((intRandomNumber >= intA) & (intRandomNumber <= intZ)))
                {
                    strRandomString = strRandomString + ((char)(intRandomNumber)).ToString();
                }
                else
                {
                    strRandomString = strRandomString + ((char)(rRandom.Next(intZero, intNine))).ToString();
                }
                intCount = intCount + 1;
            }

            return strRandomString;

        }

        public static string GenerateFriendlyString(string phrase)
        {
            string str = RemoveSign4VietnameseString(phrase.ToLower());

            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // trim it
            str = str.Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }

        public static string GenerateFriendlyString(string phrase, int maxLength)
        {
            string str = RemoveSign4VietnameseString(phrase.ToLower());

            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // cut and trim it
            int selectedLength = (str.Length <= maxLength) ? str.Length : maxLength;
            str = str.Substring(0, selectedLength).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;

            //string title = @"A bunch of ()/*++\'#@$&*^!%     invalid URL characters  ";
            //title.Slugify();
            // outputs : a-bunch-of-invalid-url-characters
        }

        public static string GetMd5Hash(string input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }

        public static string GetUniqueKey()
        {
            int maxSize = 8;
            //int minSize = 5 ;
            var a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var chars = a.ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            var size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        //public string HighlightKeyWords(string text, string keywords, string cssClass)
        //{
        //    if (text == String.Empty || keywords == String.Empty || cssClass == String.Empty)
        //        return text;
        //    var words = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //    //return words.Select(word => word.Trim()).Aggregate(text,(current, pattern) => Regex.Replace(current,pattern,string.Format("<span style=\"background-color:{0}\">{1}</span>",cssClass,"$0"),RegexOptions.IgnoreCase));
        //    //return words.Select(word => "\\b" + word.Trim() + "\\b").Aggregate(text, (current, pattern) => Regex.Replace(current, pattern, string.Format("<span style=\"background-color:{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase));
        //    return words.Select(word => word.Trim()).Aggregate(text, (current, pattern) => Regex.Replace(current, pattern, string.Format("<span class=\"{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase));
        //    //return words.Select(word => "\\b" + word.Trim() + "\\b").Aggregate(text, (current, pattern) => Regex.Replace(current, pattern, string.Format("<span class=\"{0}\">{1}</span>", cssClass, "$0"), RegexOptions.IgnoreCase));
        //}       
        public static string RemoveExtraText(string value)
        {
            var allowedChars = "01234567890.,";
            return new string(value.Where(c => allowedChars.Contains(c)).ToArray());
        }

        public static string RemoveExtraTextWithoutPointOrComma(string value)
        {
            var allowedChars = "01234567890";
            return new string(value.Where(c => allowedChars.Contains(c)).ToArray());
        }

        public static readonly string[] VietnameseSigns = new string[]    
        {   "aAeEoOuUiIdDyY",        
            "áàạảãâấầậẩẫăắằặẳẵ",        
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",        
            "éèẹẻẽêếềệểễ",        
            "ÉÈẸẺẼÊẾỀỆỂỄ",        
            "óòọỏõôốồộổỗơớờợởỡ",        
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",        
            "úùụủũưứừựửữ",        
            "ÚÙỤỦŨƯỨỪỰỬỮ",        
            "íìịỉĩ",        
            "ÍÌỊỈĨ",        
            "đ",        
            "Đ",        
            "ýỳỵỷỹ",        
            "ÝỲỴỶỸ",            
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi        
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
        
        //meta characters : \ | ( ) [  {  ^ $ * + ? .
        public static string RemoveDuplicateWhiteSpace(string input)
        {   
            return Regex.Replace(input, @"[\s]+", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        /*** Remove white space*/
        public static string RemoveWhiteSpace(string strIput)
        {
            string result = null;
            if (strIput != null)
            {
                string pattern = @"\s+";
                var regex = new Regex(pattern);
                result = regex.Replace(strIput, String.Empty);
            }
            return result;
        }        

        public static string RemoveWhitespaceWithSplit(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            string[] parts = inputString.Split(new[] { ' ', '\n', '\t', '\r', '\f', '\v' }, StringSplitOptions.RemoveEmptyEntries);
            int size = parts.Length;
            for (int i = 0; i < size; i++)
            sb.AppendFormat("{0} ", parts[i]);
            return sb.ToString();
        }

        public static String ReverseString(String inString)
        {
            // Check null String
            if (inString == null) return null;

            Int32 intSize = inString.Length;
            char[] arrayInString = inString.ToCharArray();
            char[] arrayOutString = new char[intSize];

            for (Int32 i = 0; i < intSize; ++i)
                arrayOutString[i] = arrayInString[intSize - i - 1];

            return new String(arrayOutString);
        }      

        /* SQL INJECTION ==============================================
            /((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))/i 
            /\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix 
            /((\%27)|(\'))union/ix
            /exec(\s|\+)+(s|x)p\w+/ix 
            /((\%3C)|<)((\%2F)|\/)*[a-z0-9\%]+((\%3E)|>)/ix
         =============================================================== */
        public static string RemoveFixedMetaCharacters(string strIput)
        {
            string pattern = @"&quot;|union|exec|['!~=+<>()""\[\]^&@$?%\.*:#/\\_;]|[-+]";
            string q = Regex.Replace(strIput, pattern, " ").Trim();
            string result = Regex.Replace(q, @"\s+", "-").Trim();
            return result;
        }


        public static string[] RemoveDuplicates(string[] items)
        {
            ArrayList noDupsArrList = new ArrayList();
            for (int i = 0; i <= items.Length - 1; i++)
            {
                if (!noDupsArrList.Contains(items[i].Trim()))
                {
                    noDupsArrList.Add(items[i].Trim());
                }
            }

            string[] uniqueItems = new string[noDupsArrList.Count];
            noDupsArrList.CopyTo(uniqueItems);
            return uniqueItems;
        }

        public static string RemoveRepeatWords(string source)
        {
            Dictionary<string, bool> listofUniqueWords = new Dictionary<string, bool>();
            StringBuilder destBuilder = new StringBuilder();
            string[] spilltedwords = source.Split(new[] { ' ', ',', ';', '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in spilltedwords)
            {
                string lower = item.ToLower();
                if (!listofUniqueWords.ContainsKey(lower))
                {
                    destBuilder.Append(item).Append(' ');
                    listofUniqueWords.Add(lower, true);
                }
            }

            return destBuilder.ToString().Trim();
        }
        public static ArrayList RemoveDuplicate(ArrayList sourceList)
        {
            ArrayList list = new ArrayList();
            foreach (string item in sourceList)
            {
                if (!list.Contains(item)) { list.Add(item); }
            } 
            return list;
        }

        /// <summary>
        /// Replaces and  and Quote characters to HTML safe equivalents.
        /// </summary>
        /// <param name="html">HTML to convert</param>
        /// <returns>Returns an HTML string of the converted text</returns>
        public static string FixHtmlForDisplay(string html)
        {
            html = html.Replace("<", "&lt;");
            html = html.Replace(">", "&gt;");
            html = html.Replace("\"", "&quot;");
            return html;
        }

        public static string RemoveHtmlTag(string html, string replace)
        {
            string noHtml = Regex.Replace(html, @"<[^>]+>|&nbsp;", "").Trim();
            string noHtmlNormalised = Regex.Replace(noHtml, @"\s{2,}", " "); //multiple spaces 
            noHtmlNormalised = Regex.Replace(noHtmlNormalised, "\r|\t|\n", replace);
            return noHtmlNormalised;
        }

        public static ArrayList SplitSentences(string sSourceText)
        {
            // create a local string variable
            // set to contain the string passed it
            string sTemp = sSourceText;

            // create the array list that will be used to hold the sentences
            var al = new ArrayList();

            // split the sentences with a regular expression
            string[] splitSentences = Regex.Split(sTemp, @"(?<=['""A-Za-z0-9][\.\!\?])\s+(?=[A-Z])");

            // loop the sentences
            for (int i = 0; i < splitSentences.Length; i++)
            {
                // clean up the sentence one more time, trim it, and add it to the array list
                string sSingleSentence = splitSentences[i].Replace(Environment.NewLine, string.Empty);
                al.Add(sSingleSentence.Trim());
            }
            // return the arraylist with all sentences added
            return al;
        }


        /*** Strips accents off words = Loai bo dau trong tieng viet*/
        public static string StripDiacritics(string accented)
        {
            string result = null;
            if (accented != null)
            {
                string pattern = @"\\pp{IsCombiningDiacriticalMarks}+";
                var regex = new Regex(pattern);
                string strFormD = accented.Normalize(NormalizationForm.FormD);
                result = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            return result;
        }  

        /// <summary>
        /// Strips HTML tags out of an HTML string and returns just the text.
        /// </summary>
        /// <param name="html">Html String</param>
        /// <returns></returns>
        public static string StripHtml(string html)
        {
            html = Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
            html = html.Replace("\t", " ");
            html = html.Replace("\r\n", string.Empty);
            html = html.Replace("   ", " ");
            return html.Replace("  ", " ");
        }
        
        public static string UTF8_Encode(string strInput)
        {
            if (string.IsNullOrEmpty(strInput)) return null;
            Encoding enc = Encoding.UTF8;
            byte[] byteArray = enc.GetBytes(strInput);
            var result = enc.GetString(byteArray);
            return result;
        }

        #region XU LY HTML TAGS ===========================================================
        /// <summary>
        /// Fixes a plain text field for display as HTML by replacing carriage returns 
        /// with the appropriate br and p tags for breaks.
        /// </summary>
        /// <param name="htmlText">Input string</param>
        /// <returns>Fixed up string</returns>
        public static string DisplayMemo(string htmlText)
        {
            if (htmlText == null)
                return string.Empty;

            htmlText = htmlText.Replace("\r\n", "\r");
            htmlText = htmlText.Replace("\n", "\r");
            //HtmlText = HtmlText.Replace("\r\r","<p>");
            htmlText = htmlText.Replace("\r", "<br />\r\n");
            return htmlText;
        }

        /// <summary>
        /// Method that handles handles display of text by breaking text.
        /// Unlike the non-encoded version it encodes any embedded HTML text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DisplayMemoEncoded(string text)
        {
            if (text == null)
                return string.Empty;

            bool preTag = false;
            if (text.Contains("<pre>"))
            {
                text = text.Replace("<pre>", "__pre__");
                text = text.Replace("</pre>", "__/pre__");
                preTag = true;
            }


            // fix up line breaks into <br><p>
            text = DisplayMemo(HtmlEncode(text)); //HttpUtility.HtmlEncode(Text));

            if (!preTag) return text;
            text = text.Replace("__pre__", "<pre>");
            text = text.Replace("__/pre__", "</pre>");

            return text;
        }

        /// <summary>
        /// HTML-encodes a string and returns the encoded string.
        /// </summary>
        /// <param name="text">The text string to encode. </param>
        /// <returns>The HTML-encoded text.</returns>
        public static string HtmlEncode(string text)
        {
            if (text == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder(text.Length);

            int len = text.Length;
            for (int i = 0; i < len; i++)
            {
                switch (text[i])
                {

                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case '&':
                        sb.Append("&amp;");
                        break;
                    default:
                        if (text[i] > 159)
                        {
                            // decimal numeric entity
                            sb.Append("&#");
                            sb.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
                            sb.Append(";");
                        }
                        else
                            sb.Append(text[i]);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Expands links into HTML hyperlinks inside of text or HTML.
        /// </summary>
        /// <param name="text">The text to expand</param>
        /// <param name="target">Target frame where links are displayed</param>
        /// <param name="parseFormattedLinks">Allows parsing of links in the following format [text|www.site.com]</param>
        /// <returns></returns>
        public static string ExpandUrls(string text, string target = null, bool parseFormattedLinks = false)
        {
            if (target == null)
                target = string.Empty;

            ExpandUrlsParser parser = new ExpandUrlsParser
            {
                Target = target,
                ParseFormattedLinks = parseFormattedLinks
            };
            return parser.ExpandUrls(text);
        }

        /// <summary>
        /// Create an Href HTML link
        /// </summary>
        /// <param name="text"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="additionalMarkup"></param>
        /// <returns></returns>
        public static string Href(string text, string url, string target = null, string additionalMarkup = null)
        {
            if (url.StartsWith("~"))
                url = ResolveUrl(url);

            return "<a href=\"" + url + "\" " +
                (string.IsNullOrEmpty(target) ? string.Empty : "target=\"" + target + "\" ") +
                (string.IsNullOrEmpty(additionalMarkup) ? string.Empty : additionalMarkup) +
                ">" + text + "</a>";
        }

        /// <summary>
        /// Creates an HREF HTML Link
        /// </summary>
        /// <param name="url"></param>
        public static string Href(string url)
        {
            return Href(url, url);
        }

        /// <summary>
        /// Returns an IMG link as a string. If the image is null
        /// or empty a blank string is returned.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="additionalMarkup">any additional attributes added to the element</param>
        /// <returns></returns>
        public static string ImgRef(string imageUrl, string additionalMarkup = null)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;

            if (imageUrl.StartsWith("~"))
                imageUrl = ResolveUrl(imageUrl);

            string img = "<img src=\"" + imageUrl + "\" ";

            if (!string.IsNullOrEmpty("additionalMarkup"))
                img += additionalMarkup + " ";

            img += "/>";
            return img;
        }
        #endregion XU LY HTML TAGS ========================================================
      
        #region XU LY LINK ================================================================
        /// <summary>
        /// Resolves a URL based on the current HTTPContext
        /// 
        /// Note this method is added here internally only
        /// to support the HREF() method and ~ expansion
        /// on urls.
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        internal static string ResolveUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return string.Empty;

            // Absolute path - just return
            if (originalUrl.IndexOf("://", StringComparison.Ordinal) != -1)
                return originalUrl;

            // Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                //return VirtualPathUtility.ToAbsolute(originalUrl);
                string newUrl;
                if (HttpContext.Current != null)
                {
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1);
                    newUrl = newUrl.Replace("//", "/"); // must fix up for root path
                }
                else
                    // Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");

                // Just to be sure fix up any double slashes
                return newUrl;
            }

            return originalUrl;
        }
        #endregion XU LY LINK =============================================================


        ///
        /// Converts the String to UTF8 Byte array and is used in De-serialization
        ///      
        public static Byte[] StringToUtf8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }


        /// <summary>
        /// Extracts a string from between a pair of delimiters. Only the first 
        /// instance is found.
        /// </summary>
        /// <param name="source">Input String to work on</param>
        /// <param name="beginDelim"></param>
        /// <param name="endDelim">ending delimiter</param>
        /// <param name="caseSensitive"></param>
        /// <param name="allowMissingEndDelimiter"></param>
        /// <param name="returnDelimiters"></param>
        /// <returns>Extracted string or ""</returns>
        public static string ExtractString(string source,
                                           string beginDelim,
                                           string endDelim,
                                           bool caseSensitive = false,
                                           bool allowMissingEndDelimiter = false,
                                           bool returnDelimiters = false)
        {
            int at1, at2;

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (caseSensitive)
            {
                at1 = source.IndexOf(beginDelim, StringComparison.Ordinal);
                if (at1 == -1)
                    return string.Empty;

                at2 = !returnDelimiters ? source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.Ordinal) : source.IndexOf(endDelim, at1, StringComparison.Ordinal);
            }
            else
            {
                //string Lower = source.ToLower();
                at1 = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (at1 == -1)
                    return string.Empty;

                if (!returnDelimiters)
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.OrdinalIgnoreCase);
                else
                    at2 = source.IndexOf(endDelim, at1, StringComparison.OrdinalIgnoreCase);
            }

            if (allowMissingEndDelimiter && at2 == -1)
                return source.Substring(at1 + beginDelim.Length);

            if (at1 > -1 && at2 > 1)
            {
                if (!returnDelimiters)
                    return source.Substring(at1 + beginDelim.Length, at2 - at1 - beginDelim.Length);
                else
                    return source.Substring(at1, at2 - at1 + endDelim.Length);
            }

            return string.Empty;
        }



        /// <summary>
        /// String replace function that support
        /// </summary>
        /// <param name="origString">Original input string</param>
        /// <param name="findString">The string that is to be replaced</param>
        /// <param name="replaceWith">The replacement string</param>
        /// <param name="instance">Instance of the FindString that is to be found. if Instance = -1 all are replaced</param>
        /// <param name="caseInsensitive">Case insensitivity flag</param>
        /// <returns>updated string or original string if no matches</returns>
        public static string ReplaceStringInstance(string origString, string findString,
                                                   string replaceWith, int instance,
                                                   bool caseInsensitive)
        {
            if (instance == -1)
                return ReplaceString(origString, findString, replaceWith, caseInsensitive);

            int at1 = 0;
            for (int x = 0; x < instance; x++)
            {

                at1 = caseInsensitive ? origString.IndexOf(findString, at1, origString.Length - at1, StringComparison.OrdinalIgnoreCase) : origString.IndexOf(findString, at1, StringComparison.Ordinal);

                if (at1 == -1)
                    return origString;

                if (x < instance - 1)
                    at1 += findString.Length;
            }

            return origString.Substring(0, at1) + replaceWith + origString.Substring(at1 + findString.Length);
        }

        /// <summary>
        /// Replaces a substring within a string with another substring with optional case sensitivity turned off.
        /// </summary>
        /// <param name="origString">String to do replacements on</param>
        /// <param name="findString">The string to find</param>
        /// <param name="replaceString">The string to replace found string wiht</param>
        /// <param name="caseInsensitive">If true case insensitive search is performed</param>
        /// <returns>updated string or original string if no matches</returns>
        public static string ReplaceString(string origString, string findString,
                                           string replaceString, bool caseInsensitive)
        {
            int at1 = 0;
            while (true)
            {
                at1 = caseInsensitive ? origString.IndexOf(findString, at1, origString.Length - at1, StringComparison.OrdinalIgnoreCase) : origString.IndexOf(findString, at1, StringComparison.Ordinal);

                if (at1 == -1)
                    break;

                origString = origString.Substring(0, at1) + replaceString + origString.Substring(at1 + findString.Length);

                at1 += replaceString.Length;
            }

            return origString;
        }


        /// <summary>
        /// Trims a sub string from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textToTrim"></param>
        /// <param name="caseInsensitive"></param>
        /// <returns></returns>
        public static string TrimStart(string text, string textToTrim, bool caseInsensitive)
        {
            while (true)
            {
                string match = text.Substring(0, textToTrim.Length);

                if (match == textToTrim ||
                    (caseInsensitive && match.ToLower() == textToTrim.ToLower()))
                {
                    if (text.Length <= match.Length)
                        text = "";
                    else
                        text = text.Substring(textToTrim.Length);
                }
                else
                    break;
            }
            return text;
        }

        /// <summary>
        /// Replicates an input string n number of times
        /// </summary>
        /// <param name="input"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string Replicate(string input, int charCount)
        {
            return new StringBuilder().Insert(0, "input", charCount).ToString();
        }

        /// <summary>
        /// Replicates a character n number of times and returns a string
        /// </summary>
        /// <param name="charCount"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Replicate(char character, int charCount)
        {
            return new string(character, charCount);
        }

        /// <summary>
        /// Return a string in proper Case format
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ProperCase(string input)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        /// <summary>
        /// Takes a phrase and turns it into CamelCase text.
        /// White Space, punctuation and separators are stripped
        /// </summary>
        /// <param name="?"></param>
        /// <param name="phrase"></param>
        public static string ToCamelCase(string phrase)
        {
            if (phrase == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder(phrase.Length);

            // First letter is always upper case
            bool nextUpper = true;

            foreach (char ch in phrase)
            {
                if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch) || char.IsSeparator(ch))
                {
                    nextUpper = true;
                    continue;
                }

                if (nextUpper)
                    sb.Append(char.ToUpper(ch));
                else
                    sb.Append(char.ToLower(ch));

                nextUpper = false;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Tries to create a phrase string from CamelCase text.
        /// Will place spaces before capitalized letters.
        /// 
        /// Note that this method may not work for round tripping 
        /// ToCamelCase calls, since ToCamelCase strips more characters
        /// than just spaces.
        /// </summary>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        public static string FromCamelCase(string camelCase)
        {
            if (camelCase == null)
                throw new ArgumentException("Null is not allowed for StringUtils.FromCamelCase");

            StringBuilder sb = new StringBuilder(camelCase.Length + 10);
            bool first = true;
            char lastChar = '\0';

            foreach (char ch in camelCase)
            {
                if (!first &&
                     (char.IsUpper(ch) ||
                       char.IsDigit(ch) && !char.IsDigit(lastChar)))
                    sb.Append(' ');

                sb.Append(ch);
                first = false;
                lastChar = ch;
            }

            return sb.ToString(); 
        }

        /// <summary>
        /// Terminates a string with the given end string/character, but only if the
        /// value specified doesn't already exist and the string is not empty.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="value"></param>
        /// <param name="terminator"></param>
        /// <returns></returns>
        public static string TerminateString(string value, string terminator)
        {
            if (string.IsNullOrEmpty(value) || value.EndsWith(terminator))
                return value;

            return value + terminator;
        }

        /// <summary>
        /// Trims a string to a specific number of max characters
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string TrimTo(string value, int charCount)
        {
            if (value == null)
                return string.Empty;

            if (value.Length > charCount)
                return value.Substring(0, charCount);

            return value;
        }

        /// <summary>
        /// Strips any common white space from all lines of text that have the same
        /// common white space text. Effectively removes common code indentation from
        /// code blocks for example so you can get a left aligned code snippet.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string NormalizeIndentation(string code)
        {
            // normalize tabs to 3 spaces
            string text = code.Replace("\t", "   ");

            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // keep track of the smallest indent
            int minPadding = 1000;

            foreach (var line in lines)
            {
                if (line.Length == 0)  // ignore blank lines
                    continue;

                int count = 0;
                foreach (char chr in line)
                {
                    if (chr == ' ' && count < minPadding)
                        count++;
                    else
                        break;
                }
                if (count == 0)
                    return code;

                minPadding = count;
            }

            string strip = new String(' ', minPadding);

            StringBuilder sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(ReplaceStringInstance(line, strip, "", 1, false));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns an abstract of the provided text by returning up to Length characters
        /// of a text string. If the text is truncated a ... is appended.
        /// </summary>
        /// <param name="text">Text to abstract</param>
        /// <param name="length">Number of characters to abstract to</param>
        /// <returns>string</returns>
        public static string TextAbstract(string text, int length)
        {
            if (text == null)
                return string.Empty;

            if (text.Length <= length)
                return text;

            text = text.Substring(0, length);

            text = text.Substring(0, text.LastIndexOf(" ", StringComparison.Ordinal));
            return text + "...";
        }

        /// <summary>
        /// Creates an Abstract from an HTML document. Strips the 
        /// HTML into plain text, then creates an abstract.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string HtmlAbstract(string html, int length)
        {
            return TextAbstract(StripHtml(html), length);
        }

        /// <summary>
        /// Simple Logging method that allows quickly writing a string to a file
        /// </summary>
        /// <param name="output"></param>
        /// <param name="filename"></param>
        public static void LogString(string output, string filename)
        {
            StreamWriter writer = new StreamWriter(filename, true);
            writer.WriteLine(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + " - " + output);
            writer.Close();
        }

        /// <summary>
        /// Creates short string id based on a GUID hashcode.
        /// Not guaranteed to be unique across machines, but unlikely
        /// to duplicate in medium volume situations.
        /// </summary>
        /// <returns></returns>
        public static string NewStringId()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }


        /// <summary>
        /// Parses an string into an integer. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberFormat"></param>
        /// <returns></returns>
        public static int ParseInt(string input, int defaultValue, IFormatProvider numberFormat)
        {
            int val;
            int.TryParse(input, NumberStyles.Any, numberFormat, out val);
            return val;
        }

        /// <summary>
        /// Parses an string into an integer. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseInt(string input, int defaultValue)
        {
            return ParseInt(input, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
        }

        /// <summary>
        /// Parses an string into an decimal. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberFormat"></param>
        /// <returns></returns>
        public static decimal ParseDecimal(string input, decimal defaultValue, IFormatProvider numberFormat)
        {
            decimal val;
            decimal.TryParse(input, NumberStyles.Any, numberFormat, out val);
            return val;
        }

        /// <summary>
        /// Strips all non digit values from a string and only
        /// returns the numeric string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripNonNumber(string input)
        {
            var chars = input.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (var chr in chars)
            {
                if (char.IsNumber(chr) || char.IsSeparator(chr))
                    sb.Append(chr);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Creates a Stream from a string. Internally creates
        /// a memory stream and returns that.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Stream StringToStream(string text, Encoding encoding)
        {
            MemoryStream ms = new MemoryStream(text.Length * 2);
            byte[] data = encoding.GetBytes(text);
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Creates a Stream from a string. Internally creates
        /// a memory stream and returns that.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Stream StringToStream(string text)
        {
            return StringToStream(text, Encoding.Default);
        }

        /// <summary>
        /// Turns a BinHex string that contains raw byte values
        /// into a byte array
        /// </summary>
        /// <param name="hex">BinHex string (just two byte hex digits strung together)</param>
        /// <returns></returns>
        public static byte[] BinHexToString(string hex)
        {
            int offset = hex.StartsWith("0x") ? 2 : 0;
            if ((hex.Length % 2) != 0)
                throw new ArgumentException();

            byte[] ret = new byte[(hex.Length - offset) / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)((ParseHexChar(hex[offset]) << 4)
                                 | ParseHexChar(hex[offset + 1]));
                offset += 2;
            }
            return ret;
        }

        static int ParseHexChar(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            throw new ArgumentException();
        }

        /// <summary>
        /// Retrieves a value from an XML-like string
        /// </summary>
        /// <param name="propertyString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProperty(string propertyString, string key)
        {
            return ExtractString(propertyString, "<" + key + ">", "</" + key + ">");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyString"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetProperty(string propertyString, string key, string value)
        {
            string extract = ExtractString(propertyString, "<" + key + ">", "</" + key + ">");

            if (string.IsNullOrEmpty(value) && extract != string.Empty)
            {
                return propertyString.Replace(extract, "");
            }

            string xmlLine = "<" + key + ">" + value + "</" + key + ">";

            // replace existing
            if (extract != string.Empty)
                return propertyString.Replace(extract, xmlLine);

            // add new
            return propertyString + xmlLine + "\r\n";
        }

        public static string SetRandomCaptchaString(string s, int size, bool lowerCase)
        {
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * r.NextDouble() + 65)));
                s += ch.ToString();
            }
            // storing the password in the DataSet
            return ((lowerCase ? s.ToLower() : s));
        }

        public static string Truncate(string input, int characterLimit)
        {
            string output = input;

            // Check if the string is longer than the allowed amount otherwise do nothing
            if (output.Length > characterLimit && characterLimit > 0)
            {
                // cut the string down to the maximum number of characters
                output = output.Substring(0, characterLimit);

                // Check if the character right after the truncate point was a space
                // if not, we are in the middle of a word and need to remove the rest of it
                if (input.Substring(output.Length, 1) != " ")
                {
                    int lastSpace = output.LastIndexOf(" ", StringComparison.Ordinal);

                    // if we found a space then, cut back to that space
                    if (lastSpace != -1)
                    {
                        output = output.Substring(0, lastSpace);
                    }
                }
                // Finally, add the "..."
                output += "...";
            }
            return output;
        }
        /*
        public static string ToUTF8Xml()
        {
            string result;

            MemoryStream stream = new MemoryStream(); // The writer closes this for us
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
                writer.Indentation = 4;
                writer.IndentChar = '\t';
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();

                writer.WriteStartElement("Root");
                writer.WriteAttributeString("myattrib", "123");
                writer.WriteEndElement();

                writer.WriteEndDocument();
                writer.Flush();

                // Make sure you flush or you only get half the text
                writer.Flush();

                // Use a StreamReader to get the byte order correct
                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);
                result = reader.ReadToEnd();

                // #2 - doesn't write the byte order reliably
                //result = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
            }

            // Make sure you use File.WriteAllText("myfile", xml, Encoding.UTF8);
            // or equivalent to write your file back.
            return result;
        }
         *   public static string ToXml()
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                using (XmlTextWriter writer = new XmlTextWriter(stringWriter))
                {
                    // This produces UTF16 XML
                    writer.Indentation = 4;
                    writer.IndentChar = '\t';
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartDocument();

                    writer.WriteStartElement("Root");
                    writer.WriteAttributeString("myattrib", "123");
                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
            }


            return builder.ToString();
        }

        
         * */
        ///
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        ///
        /// Unicode Byte Array to be converted to String /// String converted from Unicode Byte Array
        public static string Utf8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        public static int CountRepeatedCharInString(string str, char c)
        {
            int numSlash = 0;
            for (int index = 0; index < str.Length; index++)
            {
                if (str[index] == c)
                {
                    numSlash++;
                }
            }
            return numSlash;
        }
        public static string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();

            // select all that starts with the value string (in this case with "*")
            //string value = "*";
            // the dataView.RowFilter will be: "Name LIKE '[*]*'"
            //DataTable dt=new DataTable();
            //DataView dv = new DataView(dt);
            //dataView.RowFilter = String.Format("Name LIKE '{0}*'", EscapeLikeValue(value));
        }
       
      
        //=====================================================
        public static string ConverId_Id_rewrite(string id)
        {
            string result = "";
            if (id != "")
            {
                if (id.IndexOf("-", StringComparison.Ordinal) > -1)
                    result = id.Substring(0, id.IndexOf("-", StringComparison.Ordinal));
                else
                    result = id;
            }
            return result;
        }
        public static string Conver_rewrite_GetId(string link)
        {
            string result = "";

            if (link.IndexOf("_", StringComparison.Ordinal) > -1)
                result = link.Substring(4, link.IndexOf("_", StringComparison.Ordinal) - 1);

            return result;
        }
        public static string GetGenderSpecificPronoun(string input)
        {
            switch (input)
            {
                case "m":
                    return "his";
                case "f":
                    return "her";
                default:
                    return "their";
            }
        }
        public static string AppendWithDelimiter(string queryString, string delimiter, string text)
        {
            if (queryString != "")
                queryString += delimiter;
            return queryString + text;
        }
        public static string Base64ForUrlEncode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64ForUrlDecode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {

                return "";
            }
        }


        //================================================================
        public static bool IsNullOrEmpty(string strVal)
        {
            return string.IsNullOrWhiteSpace(strVal);
        }

        public static bool IsNullOrEmpty(object strVal)
        {
            return strVal == null || IsNullOrEmpty(strVal.ToString());
        }

        public static bool IsNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }
        public static bool IsNullOrEmptyT<T>(IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
        public static bool IsNullOrEmptyAll(params object[] collection)
        {
            if (IsNullOrEmpty(collection)) return true;
            return Array.TrueForAll(collection, IsNullOrEmpty);
        }
        public static bool IsEmptyGuid(Guid value)
        {
            return value.Equals(Guid.Empty);
        }
        public static bool IsGuid(string value)
        {
            try
            {
                Guid test;
                return Guid.TryParse(value, out test);
            }
            catch (Exception)
            {
                // ignored
            }
            return false;
        }

        public static Guid ToGuid(string value)
        {
            try
            {
                Guid test;
                Guid.TryParse(value, out test);
                return test;
            }
            catch (Exception)
            {
                // ignored
            }
            return Guid.Empty;
        }

        public static T ToType<T>(object value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch { return default(T); }
        }

        public static T ToEnum<T>(string enumValue)
        {
            return (T)System.Enum.Parse(typeof(T), enumValue);
        }

        public static string DateStr(DateTime dateTime, string format, string defaultIfDateMin = "N/A")
        {
            if (dateTime == DateTime.MinValue.ToUniversalTime())
                return defaultIfDateMin;
            return dateTime.ToString(format);
        }

        public static string List2Str(IEnumerable<string> ls, string seperate = ",")
        {
            if (IsNullOrEmptyT(ls)) return string.Empty;
            return Join(seperate, ls);
        }

        public static List<string> Str2List(string str, string seperate = ",", bool trimStartEnd = true)
        {
            if (IsNullOrEmpty(str)) return new List<string>();

            var arr = str.Split(seperate.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            if (trimStartEnd)
                return arr.Select(p => p.TrimStart(' ').TrimEnd(' ')).ToList();

            return arr.ToList();
        }

        public static string Join(string separator, params string[] arrayVals)
        {
            return string.Join(separator, arrayVals);
        }

        public static string Join(string separator, params object[] arrayVals)
        {
            return string.Join(separator, arrayVals);
        }

        public static string Join(string separator, IEnumerable<string> arrayVals)
        {
            return string.Join(separator, arrayVals);
        }

        public static string Join(string separator, IEnumerable<object> arrayVals)
        {
            return string.Join(separator, arrayVals);
        }

        public static List<string> FetchImageUrlFromHtmlSource(string htmlSource)
        {
            List<string> links = new List<string>();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (Match m in matchesImgSrc)
            {
                string imageURL = m.Groups[1].Value;
                links.Add(imageURL);
            }
            return links;
        }

        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            var expression = memberAccess.Body.ToString();
            var subExpressions = expression.Split('.').ToList();

            if (subExpressions.Count > 2)
            {
                subExpressions.RemoveAt(0);
                return string.Join(".", subExpressions);
            }

            return ((MemberExpression)memberAccess.Body).Member.Name;
        }

        public static Dictionary<string, object> ToDictionay(object model)
        {
            return model.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(m => m.Name, prop => prop.GetValue(model, null));
        }

        public static char GetSeparator(string val)
        {
            char separator = new char();
            switch (val.Last())
            {
                case 't':
                    separator = '\t';
                    break;
                case 'r':
                    separator = '\r';
                    break;
                default:
                    separator = Convert.ToChar(val);
                    break;
            }
            return separator;
        }
    }
}
