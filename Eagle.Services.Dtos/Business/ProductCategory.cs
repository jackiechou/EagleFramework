using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductCategoryDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryCode")]
        public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryAlias")]
        public string CategoryAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryLink")]
        public string CategoryLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ViewOrder")]
        public int? ViewOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Icon")]
        public string Icon { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BriefDescription")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string BriefDescription { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ProductCategoryStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }
    }

    public class ProductCategoryEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryCode")]
        public string CategoryCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryLink")]
        public string CategoryLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Icon")]
        public string Icon { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "BriefDescription")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string BriefDescription { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(ProductCategoryStatus))]
        public ProductCategoryStatus Status { get; set; }
    }

    public class ProductCategoryEditEntry : ProductCategoryEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
    }
}
