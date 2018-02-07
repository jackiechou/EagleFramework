using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Permission
{ 
   public enum MenuPositionAdmin
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Top")]
        Top = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Bottom")]
        Bottom = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Left")]
        Left = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Right")]
        Right = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "Middle")]
        Middle = 5,
    }
}
