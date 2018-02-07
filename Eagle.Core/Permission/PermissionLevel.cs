using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Permission
{
    public enum PermissionLevel
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FullControl")]
        FullControl = -1,
        [Display(ResourceType = typeof(LanguageResource), Name = "None")]
        None = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "View")]
        View = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Create")]
        Create = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Edit")]
        Edit = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Delete")]
        Delete = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Publish")]
        Publish = 5,
        [Display(ResourceType = typeof(LanguageResource), Name = "Print")]
        Print = 6,
        [Display(ResourceType = typeof(LanguageResource), Name = "Import")]
        Import = 7,
        [Display(ResourceType = typeof(LanguageResource), Name = "Export")]
        Export = 8,
        [Display(ResourceType = typeof(LanguageResource), Name = "Upload")]
        Upload = 9
    }
}
