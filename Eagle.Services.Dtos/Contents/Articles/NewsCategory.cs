using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Articles
{
    public class NewsCategoryTreeGridDetail : DtoBase
    {
        public int id { get; set; }
        public NewsCategoryInfoDetail data { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public TreeState state { get; set; }
        public List<NewsCategoryTreeGridDetail> children { get; set; }
    }

    public class TreeState
    {
        public bool? opened { get; set; }
        public bool? disabled { get; set; }
        public bool? selected { get; set; }
    }
    public class NewsCategorySortOrderEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ListOrder { get; set; }
    }
    public class NewsCategoryListOrderEntry : DtoBase
    {
        public List<NewsCategorySortOrderEntry> ListOrders { get; set; }
    }
    public class NewsCategoryInfoDetail : NewsCategoryDetail
    {
        [NotMapped]
        public string Action { get; set; }
    }
    public class NewsCategoryDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryImage")]
        public string CategoryImage { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string NavigateUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public NewsCategoryStatus? Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }
    }
    public class NewsCategoryEntry : DtoBase
    {
        //[Display(ResourceType = typeof(LanguageResource), Name = "CategoryCode")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        //public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryImage")]
        public string CategoryImage { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(Int32.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        [StringLength(1000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string NavigateUrl { get; set; }

        [EnumDataType(typeof(NewsCategoryStatus))]
        public NewsCategoryStatus? Status { get; set; }


        [NotMapped]
        public HttpPostedFileBase FileUpload { get; set; }
    }
    public class NewsCategoryEditEntry : NewsCategoryEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
    }
}
