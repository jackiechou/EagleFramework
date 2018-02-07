using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Skins
{
    public class SkinPackageBackgroundEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundLink")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WrongUrlFormat")]
        public string BackgroundLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsExternalLink")]
        public bool IsExternalLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "AttachFile")]
        public HttpPostedFileBase File { get; set; }
    }
    public class SkinPackageBackgroundEditEntry : SkinPackageBackgroundEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        public int BackgroundId { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundFile")]
        public int? BackgroundFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }
    public class SkinPackageBackgroundDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SkinBackgroundId")]
        public int BackgroundId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundName")]
        public string BackgroundName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundFile")]
        public int? BackgroundFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundLink")]
        public string BackgroundLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsExternalLink")]
        public bool IsExternalLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }

    public class SkinPackageBackgroundInfoDetail : SkinPackageBackgroundDetail
    {
        public SkinPackageDetail Package { get; set; }
    }
    public class SkinPackageBackgroundSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int? SearchTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int? SearchPackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? SearchStatus { get; set; }
    }
}
