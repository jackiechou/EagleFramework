using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Skins
{
    public class ThemeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PackageName")]
        public string PackageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageSrc")]
        public string PackageSrc { get; set; }
    }
}
