using System;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Entities.Common
{
    public class DateTimeFormatAttribute : ValidationAttribute
    {
        public string Format { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            DateTime val;
            try
            {
                val = DateTime.ParseExact(value.ToString(), Format, null);
            }
            catch (FormatException)
            {
                return false;
            }

            //Not entirely sure it'd ever reach this, but I need a return statement in all codepaths
            return val != DateTime.MinValue.ToUniversalTime();
        }
    }
}
