using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoredDateType : ValidationAttribute
    {
        public IgnoredDateType()
        {
            ErrorMessage = LanguageResource.InvalidDate;
        }

        public IgnoredDateType(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            var dateString = value as string;
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return true; // Not our problem
            }
            DateTime result;
            DateTime.TryParse(dateString, out result);
            return result != DateTime.MinValue.ToUniversalTime();
        }
    }
}