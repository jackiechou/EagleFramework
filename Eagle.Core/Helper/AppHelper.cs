using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core.Helper
{
    public class AppHelper
    {
        public static SmtpSection GetSmtpSetting(string sectionName = "smtp")
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/" + sectionName);
            return smtpSection;
        }
    }
}
