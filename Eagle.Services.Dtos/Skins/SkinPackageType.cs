using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Skins
{
    public class SkinPackageTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class SkinPackageTypeEditEntry : SkinPackageTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }
    }
    public class SkinPackageTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
}
