using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum PageStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1,
    }
}
