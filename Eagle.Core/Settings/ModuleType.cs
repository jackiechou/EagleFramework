using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum ModuleType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Admin", Description = "Admin", Order = 0)]
        Admin = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Site", Description = "Site", Order = 1)]
        Site = 2
    }
}
