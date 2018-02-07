using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class SiteMapDetail : DtoBase
    {
        public int SiteMapId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public SiteMapFrequency? Frequency { get; set; }
        public decimal? Priority { get; set; }
        public int ListOrder { get; set; }
        public bool Status { get; set; }
        public DateTime? LastModified { get; set; }

        public List<SiteMapDetail> Children { get; set; }
    }
    public class SiteMapEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Action")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Action { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Controller")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Controller { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Frequency")]
        public SiteMapFrequency? Frequency { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Priority")]
        public decimal? Priority { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool Status { get; set; }
    }
    public class SiteMapEditEntry : SiteMapEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SiteMapId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int SiteMapId { get; set; }
    }
}
