using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Permission
{
    public enum PagePermissionLevel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "View")]
        View = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Create")]
        Create = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Edit")]
        Edit = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Delete")]
        Delete = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "FullControl")]
        FullControl = 5,
    }
}
