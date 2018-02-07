using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Common
{
    public enum IsSecured
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "IsDesktop")]
        IsDesktop = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "IsAdmin")]
        IsAdmin = 1
    }
}
