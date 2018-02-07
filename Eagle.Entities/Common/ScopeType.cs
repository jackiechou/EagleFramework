using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Common
{
    public enum ScopeType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Host")]
        Host = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Admin")]
        Admin = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Site")]
        Site = 3
    }
}
