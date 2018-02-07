using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public class LanguageType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Vietnamese", Description = "Vietnamese", Order = 0)]
        public const string Vietnamese = "vi-VN";

        [Display(ResourceType = typeof(LanguageResource), Name = "English", Description = "English", Order = 1)]
        public const string English = "en-US";
    }
}
