using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Eagle.Common.Extensions.EnumHelper
{
    public static class EnumExtension
    {
        //Console.WriteLine(1.In(2, 1, 3));
        //Console.WriteLine(1.In(2, 3));
        //Console.WriteLine(UserStatus.Active.In(UserStatus.Removed, UserStatus.Banned));
        public static bool In<T>(this T val, params T[] values) where T : struct
        {
            return values.Contains(val);
        }
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)

            where TEnum : struct, IComparable, IFormattable, IConvertible
        {

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))

                            select new { Id = e, Name = e.ToString(CultureInfo.InvariantCulture) };

            return new SelectList(values, "Id", "Name", enumObj);
            //ViewData["taskStatus"] = task.Status.ToSelectList();
        }

        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }

        public static string DisplayDescription(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Description;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).Description;
            }

            return outString;
        }
        public static string GetDisplayName<TEnum>(TEnum value)
        {
            Type enumType = typeof(TEnum);
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Description;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).Description;
            }

            return outString;
        }

        public static string GetDescription<TEnum>(TEnum value)
        {
            Type type = typeof(TEnum);
            var name = Enum.GetNames(type).FirstOrDefault(f => f.Equals(value));

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : name;
        }

        public static string GetEnumDescriptionFromString<T>(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value,
                StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();
            if (name == null)
            {
                return string.Empty;
            }
            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        public static Dictionary<int, string> BindEnum(Type enumType)
        {
            // get the names from the enumeration
            string[] keys = Enum.GetNames(enumType);
            // get the values from the enumeration
            Array values = Enum.GetValues(enumType);

            ////var sortedPairs = values.Select((x, i) => new { Value = x, Key = enumNames[i] })
            ////            .OrderBy(x => x.Value)
            ////            .ThenBy(x => x.Key)
            ////            .ToArray();

            ////string[] sortedEnumKeys = sortedPairs.Select(x => x.Key).ToArray();
            ////int[] sortedEnumValues = sortedPairs.Select(x => x.Value).ToArray();  

            Dictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                dict.Add((int)values.GetValue(i), keys[i]);
            }
            return dict;
        }

        public static Dictionary<int, string> BindEnumOrderByName(Type enumType)
        {
            string[] keys = Enum.GetNames(enumType);
            Array values = Enum.GetValues(enumType);
            Array.Sort(keys, values);           
            Dictionary<int, string> dict = new Dictionary<int, string>();
            //string key = string.Empty;
            //int val = 0;
            //Hashtable hashtable = new Hashtable();          
            for (int i = 0; i < values.Length; i++)
            {
                //key = keys[i];
                //val = Convert.ToInt32(keys.Where(e => e.StartsWith(key)).Select(e => (int)Enum.Parse(enumType, e)).FirstOrDefault());
                //hashtable[val] = key;
                //hashtable.Add((int)values.GetValue(i), keys[i]);
                dict.Add((int)values.GetValue(i), keys[i]);
            }
            return dict;
        }

        public static Dictionary<int, string> BindEnumOrderByKey(Type enumType)
        {
            // get the names from the enumeration
            string[] keys = Enum.GetNames(enumType);
            // get the values from the enumeration
            Array values = Enum.GetValues(enumType);
            Array.Sort(values, keys);
            //Array.Reverse(names, 0, names.Length);
            //Array.Reverse(values, 0, values.Length);

            Dictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                dict.Add((int)values.GetValue(i), keys[i]);
            }
            return dict;
        }

        /// <summary>
        /// Get Enum Description
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>string value of the description</returns>
        public static string EnumDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Enum value is null !");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])
            fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }

        /// <summary>
        /// Find enum value from a string
        /// </summary>
        /// <typeparam name="TEnum">Enum Type</typeparam>
        /// <param name="value">Value to be seached</param>
        /// <returns>Enum value of type TYourEnum</returns>
        public static TEnum FindEnumFromDescription<TEnum>(this string value)
           where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum || string.IsNullOrWhiteSpace(value))
            {
                return default(TEnum);
            }

            var enumValues = Enum.GetValues(typeof(TEnum));

            foreach (var item in enumValues)
            {
                if (value.ToLower().Equals((item as Enum).EnumDescription().ToLower()))
                {
                    return (TEnum)item;
                }
            }

            return default(TEnum);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
