using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
     public enum MediaComposerStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 0)]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 1)]
        Active = 1
    }
}
