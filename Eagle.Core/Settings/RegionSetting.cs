using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eagle.Core.Settings
{
    public class RegionSetting
    {
        public static List<Dictionary<string, object>> Regions;
        public static Dictionary<string, string> CountryStates;

        static RegionSetting()
        {
            Regions = GetRegions();
            CountryStates = GetCountryStates();
        }


        static List<Dictionary<string, object>> GetRegions()
        {
            var dataCuisine = new List<string> {
                "Africa/Middle East",
                "Asia Pacific",
                "Caribbean/Bahamas/Mexico",
                "Europe",
                "Latin America (South/Central America)",
                "North America",
            };
            var data = new List<Dictionary<string, object>>();
            foreach (string item in dataCuisine)
            {
                data.Add(new Dictionary<string, object>
                {
                    { "id", Guid.NewGuid().ToString() },
                    { "value", item },
                    { "name", item }
                });
            }
            return data;
        }


        static Dictionary<string, string> GetCountryStates()
        {
            var returnData = new Dictionary<string, string> {
                {"Alabama", "AL" },
                {"Alaska", "AK"},
                {"American Samoa", "AS"},
                {"Arizona", "AZ"},
                {"Arkansas", "AR"},
                {"California", "CA"},
                {"Colorado", "CO"},
                {"Connecticut", "CT"},
                {"Delaware", "DE"},
                {"District Of Columbia", "DC"},
                {"Federated States Of Micronesia", "FM"},
                {"Florida", "FL"},
                {"Georgia", "GA"},
                {"Guam", "GU"},
                {"Hawaii", "HI"},
                {"Idaho", "ID"},
                {"Illinois", "IL"},
                {"Indiana", "IN"},
                {"Iowa", "IA"},
                {"Kansas", "KS"},
                {"Kentucky", "KY"},
                {"Louisiana", "LA"},
                {"Maine", "ME"},
                {"Marshall Islands", "MH"},
                {"Maryland", "MD"},
                {"Massachusetts", "MA"},
                {"Michigan", "MI"},
                {"Minnesota", "MN"},
                {"Mississippi", "MS"},
                {"Missouri", "MO"},
                {"Montana", "MT"},
                {"Nebraska", "NE"},
                {"Nevada", "NV"},
                {"New Hampshire", "NH"},
                {"New Jersey", "NJ"},
                {"New Mexico", "NM"},
                {"New York", "NY"},
                {"North Carolina", "NC"},
                {"North Dakota", "ND"},
                {"Northern Mariana Islands", "MP"},
                {"Ohio", "OH"},
                {"Oklahoma", "OK"},
                {"Oregon", "OR"},
                {"Palau", "PW"},
                {"Pennsylvania", "PA"},
                {"Puerto Rico", "PR"},
                {"Rhode Island", "RI"},
                {"South Carolina", "SC"},
                {"South Dakota", "SD"},
                {"Tennessee", "TN"},
                {"Texas", "TX"},
                {"Utah", "UT"},
                {"Vermont", "VT"},
                {"Virgin Islands", "VI"},
                {"Virginia", "VA"},
                {"Washington", "WA"},
                {"West Virginia", "WV"},
                {"Wisconsin", "WI"},
                {"Wyoming", "WY"},

                {"British Columbia", "BC"},
                {"Ontario", "ON"},
                {"Newfoundland and Labrador", "NL"},
                {"Nova Scotia", "NS"},
                {"Prince Edward Island", "PE"},
                {"New Brunswick", "NB"},
                {"Quebec", "QC"},
                {"Manitoba", "MB"},
                {"Saskatchewan", "SK"},
                {"Alberta", "AB"},
                {"Northwest Territories", "NT"},
                {"Nunavut", "NU"},
                { "Yukon Territory", "YT"},
            };
            return returnData;
        }

        public static string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
