using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
     public enum MediaTopicStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 1)]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 2)]
        InActive = 0
    }
}
