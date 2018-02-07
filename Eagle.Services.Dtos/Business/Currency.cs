using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class CurrencySearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CurrencyStatus? IsActive { get; set; }
    }
    public class CurrencyDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyId")]
        public int CurrencyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyName")]
        public string CurrencyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CurrencyStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }
    public class CurrencyEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CurrencyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public CurrencyStatus IsActive { get; set; }

    }
    public class CurrencyEditEntry : CurrencyEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CurrencyId { get; set; }
    }
    public class CurrencyRateDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyRateId")]
        public int CurrencyRateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyRateDate")]
        public DateTime CurrencyRateDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromCurrencyCode")]
        public string FromCurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToCurrencyCode")]
        public string ToCurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AverageRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal AverageRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndOfDayRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal EndOfDayRate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
    }
    public class CurrencyRateEntry : DtoBase
    {
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyRateDate")]
        public DateTime CurrencyRateDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromCurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string FromCurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToCurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ToCurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AverageRate")]
        public decimal AverageRate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndOfDayRate")]
        public decimal EndOfDayRate { get; set; }
    }
}
