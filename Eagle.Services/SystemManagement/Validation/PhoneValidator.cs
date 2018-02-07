using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eagle.Services.SystemManagement.Validation
{
    public class PhoneValidator
    {
        static readonly IDictionary<string, Regex> CountryRegex = new Dictionary<string, Regex>() {
           { "USA", new Regex("^[2-9]\\d{2}-\\d{3}-\\d{4}$")},
           { "UK", new Regex("(^1300\\d{6}$)|(^1800|1900|1902\\d{6}$)|(^0[2|3|7|8]{1}[0-9]{8}$)|(^13\\d{4}$)|(^04\\d{2,3}\\d{6}$)")},
           { "Netherlands", new Regex("(^\\+[0-9]{2}|^\\+[0-9]{2}\\(0\\)|^\\(\\+[0-9]{2}\\)\\(0\\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\\-\\s]{10}$)")},
        };

        /// <summary>
        /// if (!PhoneValidator.IsValidNumber(ContactPhone, Country)) yield return new RuleViolation("Phone# does not match country", "ContactPhone");
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static bool IsValidNumber(string phoneNumber, string country)
        {

            if (country != null && CountryRegex.ContainsKey(country))
                return CountryRegex[country].IsMatch(phoneNumber);
            else
                return false;
        }

        public static IEnumerable<string> Countries => CountryRegex.Keys;
    }
}
