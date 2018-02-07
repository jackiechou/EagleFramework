using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Eagle.Common.Utilities
{
    public class NumberUtils
    {
        public static string GenerateThousandSeparator(string input)
        {
            string result = string.Empty;
            System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
            System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowThousands;
            double number = Double.Parse(input, style, provider);
            if (number > 0)
                result = String.Format("{0:#,###}", number.ToString("#,#", CultureInfo.InvariantCulture).Replace(",", "."));
            return result;
        }

    }
}
