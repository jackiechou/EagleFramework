using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductAttributeOptionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionId")]
        public int OptionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string OptionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionValue")]
        public decimal? OptionValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductAttributeOptionStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AttributeId")]
        public int AttributeId { get; set; }
    }

    public class ProductAttributeOptionEditEntry : ProductAttributeOptionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AttributeId")]
        public int? AttributeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionId")]
        public int? OptionId { get; set; }
    }

    public class ProductAttributeOptionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OptionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string OptionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OptionValue")]
        public decimal? OptionValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ProductAttributeOptionStatus IsActive { get; set; }

        public int? AttributeIndex { get; set; }
    }
}
