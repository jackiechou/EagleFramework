using System;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Entities.Common
{
    public class AllowedYearAttribute : ValidationAttribute
    {
        public DateTime MinDateTime { get; set; }
        public AllowedYearAttribute(int allowedYear)
        {
            int year = (allowedYear > 0) ? allowedYear : 18;
            MinDateTime = DateTime.UtcNow.AddYears(-Math.Abs(year));
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            try
            {
                var val = Convert.ToDateTime(value);

                if (val.CompareTo(MinDateTime) > 0)
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
