using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.SystemManagement
{ 
    public enum MenuTypeStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive")]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active")]
        Active = 1
    }
}
