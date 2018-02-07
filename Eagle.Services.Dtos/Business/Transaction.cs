using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class TransactionMethodSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TransactionMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public TransactionMethodStatus? IsActive { get; set; }
    }
    public class TransactionMethodDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodId")]
        public int TransactionMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodName")]
        public string TransactionMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodFee")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? TransactionMethodFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public TransactionMethodStatus IsActive { get; set; }
    }

    public class TransactionMethodEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TransactionMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodFee")]
        public string TransactionMethodFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public TransactionMethodStatus IsActive { get; set; }
    }
    public class TransactionMethodEditEntry : TransactionMethodEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TransactionMethodId { get; set; }
    }
}
