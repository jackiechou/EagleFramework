using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Skins
{
    public class SkinPackageEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PackageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageSrc")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PackageSrc { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class SkinPackageEditEntry : SkinPackageEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PackageId { get; set; }
    }

    [Serializable]
    public class SkinPackageDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageName")]
        public string PackageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageAlias")]
        public string PackageAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageSrc")]
        public string PackageSrc { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }


    }
    public class SkinPackageInfoDetail : SkinPackageDetail
    {
        public SkinPackageTypeDetail Type { get; set; }
    }
    public class SkinPackageSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int? SearchTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? SearchStatus { get; set; }
    }
}
