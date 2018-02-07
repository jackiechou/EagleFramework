using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ContentTree : DtoBase
    {
        public int id { get; set; }
        public int key { get; set; }
        public int? parentId { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public bool? isParent { get; set; }
        public bool? open { get; set; }
        public List<ContentTree> children { get; set; }

        public ContentTree()
        {
            children = new List<ContentTree>();
        }
    }
    public class ContentItemDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int? PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int? ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContentTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentItemId")]
        public int ContentItemId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentItemName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ContentItemName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentItemTitle")]
        public string ContentItemTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Content")]
        public string Content { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentKey")]
        public string ContentKey { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }

        public string PageName { get; set; }
        public string ModuleName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ScopeTypeId")]
        public int? ScopeTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsParent")]
        public bool? IsParent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }
    }
    public class ContentItemEditEntry : ContentItemEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContentItemId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContentItemId { get; set; }
    }
    public class ContentItemEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ItemKey")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ItemKey { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Content")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string ItemContent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ContentTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContentTypeId { get; set; }
    }


    public class ContentTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContentTypeName")]
        public string ContentTypeName { get; set; }
    }
    public class ContentTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContentTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ContentTypeName { get; set; }
    }
}
