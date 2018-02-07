using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductTypeSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? ProductCategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductTypeName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductTypeStatus? IsActive { get; set; }
    }
    public class ProductTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        public string TypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductTypeStatus IsActive { get; set; }
    }

    public class ProductTypeEditEntry : ProductTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }
    }

    public class ProductTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        //[Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCategory")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TypeName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ProductTypeStatus IsActive { get; set; }
    }
}
