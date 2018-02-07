using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Skins
{
    public class SkinPackageTemplateEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TemplateName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateKey")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TemplateKey { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateSrc")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TemplateSrc { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class SkinPackageTemplateEditEntry : SkinPackageTemplateEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TemplateId { get; set; }
    }
    public class SkinPackageTemplateDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        public int TemplateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateName")]
        public string TemplateName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateKey")]
        public string TemplateKey { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateSrc")]
        public string TemplateSrc { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class SkinPackageTemplateInfoDetail : SkinPackageTemplateDetail
    {
        public SkinPackageDetail Package { get; set; }
        public SkinPackageTypeDetail Type { get; set; }
    }
    public class SkinPackageTemplateSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int? SearchTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int? SearchPackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? SearchStatus { get; set; }
    }
}
