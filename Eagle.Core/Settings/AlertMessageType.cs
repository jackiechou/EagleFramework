using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum AlertMessageType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Error", Description = "Error", Order = 0)]
        Error = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Info", Description = "Info", Order = 0)]
        Info = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Emergency", Description = "Emergency", Order = 0)]
        Success =3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Emergency", Description = "Emergency", Order = 0)]
        Warning = 4
    }
}
