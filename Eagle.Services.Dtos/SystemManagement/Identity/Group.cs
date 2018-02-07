using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Search;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class GroupDetail : DtoBase
    {
        public Guid ApplicationId { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }
        public bool? IsActive { get; set; }
    }
    public class GroupEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "GroupName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string GroupName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }
    public class GroupEditEntry : GroupEntry
    {
        public Guid GroupId { get; set; }
    }

    public class GroupSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "GroupName")]
        public string GroupName { get; set; }
    }

    public class GroupSearchResult : SearchResult<GroupDetail>
    {
        public GroupSearchEntry Filter { get; set; }
    }
}
