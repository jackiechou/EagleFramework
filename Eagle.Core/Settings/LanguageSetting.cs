using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public class LanguageSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "English", Description = "English", Order = 1)]
        public static string English = "en-US";

        [Display(ResourceType = typeof(LanguageResource), Name = "Vietnamese", Description = "Vietnamese", Order = 0)]
        public static string Vietnamese = "vi-VN";
    }
}
