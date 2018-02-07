using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Eagle.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }
        public static bool IsNullOrEmpty(this object strVal)
        {
            return strVal == null || IsNullOrEmpty(strVal.ToString());
        }
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }
        public static bool IsNullOrEmptyT<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
        public static bool ToBoolean(this string s)
        {
            return s.ToBoolean(false);
        }
        public static bool ToBoolean(this string s, bool def)
        {
            bool result;
            if (Boolean.TryParse(s, out result))
                return result;
            else
                return def;
        }
        public static int ToInt(this string source)
        {
            return int.Parse(source);
        }
     
        public static T ToType<T>(this object value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch { return default(T); }
        }

        public static T ToEnum<T>(this string enumValue)
        {
            return (T)System.Enum.Parse(typeof(T), enumValue);
        }

        public static string[] SplitNoEmpties(this string fromString, string seperator)
        {
            if (!string.IsNullOrEmpty(fromString))
                return fromString.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
            return new string[0];
        }

        public static string[] SplitNoEmpties(this string fromString, char seperator)
        {
            return fromString.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static int ToIntDef(this string s, int def)
        {
            Int32 result;
            if (Int32.TryParse(s, out result))
                return result;
            else
                return def;
        }
        public static string Left(this string str, int length)
        {
            return str.Substring(0, Math.Min(length, str.Length));
        }
        public static string UpdateTemplateFields(this string template, Dictionary<string, string> values)
        {
            foreach (var item in values)
            {
                template = template.Replace("##" + item.Key + "##", item.Value);
                //template = template.Replace("<" + TemplateTags.TemplateTag + " id=\">" + item.Key + "\" />", item.Value);
            }
            return template;
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
        /// <summary>
        /// Allows Case sensitivity options in Contains
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="toCheck">String to search for</param>
        /// <param name="comp">Compare rules</param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string AppendWithDelimiter(string queryString, string delimiter, string text)
        {
            if (queryString != "")
                queryString += delimiter;
            return queryString + text;
        }

        public static string RemoveHtmlTags(this string source)
        {
            return Regex.Replace(source, "<.*?>", String.Empty);
        }

        public static string Titlize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        public static bool InsensitiveEquals(this string strA, string strB)
        {
            return strA.ToLower().Equals(strB.ToLower());
        }
    }
}
