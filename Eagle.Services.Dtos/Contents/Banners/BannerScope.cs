using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Banners
{
    public class BannerScopeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int ScopeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ScopeName")]
        public string ScopeName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public BannerScopeStatus Status { get; set; }
    }
    public class BannerScopeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "BannerTypeName")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ScopeName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "BannerTypeStatus")]
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTypeStatus")]
        public BannerScopeStatus Status { get; set; }
    }
    public class BannerScopeEditEntry : BannerScopeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ScopeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ScopeId { get; set; }
    }
}
