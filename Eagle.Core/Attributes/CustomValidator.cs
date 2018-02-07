using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using Eagle.Resources;

namespace Eagle.Core.Attributes
{
    public class EmailValidator : RequestFieldValidatorBase
    {
        public EmailValidator(string errorMessage = null) : base(errorMessage) { }

        protected override bool IsValid(HttpContextBase httpContext, string value)
        {
            try
            {
                MailAddress email = new MailAddress(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class BirthdayValidator : RequestFieldValidatorBase
    {
        public BirthdayValidator(string errorMessage = null) : base(errorMessage) { }

        protected override bool IsValid(HttpContextBase httpContext, string value)
        {
            DateTime bd;
            if (DateTime.TryParse(value, out bd))
            {
                return bd < DateTime.UtcNow && bd > DateTime.UtcNow.AddYears(-100);
            }
            return false;
        }
    }

    /// <summary>
    ///  register the validator in your .cshtml page as follows:
    /// Validation.Add("Email", CustomValidator.Email());
    /// </summary>
    public class CustomValidator
    {
        //Email Validator
        public static IValidator Email(string errorMessage = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = LanguageResource.InvalidEmail;
            }
            return new EmailValidator(errorMessage);
        }

        //Birthday Validator
        public static IValidator Birthday(string errorMessage = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = LanguageResource.InvalidBirthday;
            }
            return new BirthdayValidator(errorMessage);
        }

    }


}
