using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
   public enum ProductAttributeOptionStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive")]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active")]
        Active = 1
    }
}
