using System;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Entities.Common
{
    public class MinDateAttribute : ValidationAttribute
    {
        public DateTime MinDate { get; set; }
        public MinDateAttribute(int year, int month, int day)
        {
            MinDate = new DateTime((year >= 1900) ? year : 1900, (month > 0 && month <= 12) ? month : 1, (day > 0 && day <= 31) ? day : 1);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            try
            {
                var val = Convert.ToDateTime(value);

                if (val.CompareTo(MinDate) < 0)
                {
                    return false;
                }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
