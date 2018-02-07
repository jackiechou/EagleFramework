using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ResourceEntryDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ResourceCulture")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ResourceCulture { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ResourceName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string ResourceName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ResourceType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ResourceType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ResourceValue")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ResourceValue { get; set; }
    }

    public class ResourceSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ResourceName")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ResourceName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ProductStatus? Status { get; set; }
    }
}
