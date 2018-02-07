using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Permission
{
    public enum MenuPositionSite
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Top")]
        Top = 6,
        [Display(ResourceType = typeof(LanguageResource), Name = "Bottom")]
        Bottom =7,
        [Display(ResourceType = typeof(LanguageResource), Name = "Left")]
        Left = 8,
        [Display(ResourceType = typeof(LanguageResource), Name = "Right")]
        Right = 9,
        [Display(ResourceType = typeof(LanguageResource), Name = "Middle")]
        Middle = 10,
    }
}
