using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum DocumentFolderStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive")]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active")]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Published")]
        Published = 2
    }
}
