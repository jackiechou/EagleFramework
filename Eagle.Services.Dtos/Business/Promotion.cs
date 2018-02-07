using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class PromotionSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        //[DateGreaterThan("StartDate", true, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        // [GreaterThan("StartDate", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "StartDateMustBeBeforeEndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public PromotionStatus? IsActive { get; set; }
    }
    public class PromotionDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionId")]
        public int PromotionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionType")]
        public PromotionType PromotionType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionTitle")]
        public string PromotionTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionValue")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal PromotionValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPercent")]
        public bool IsPercent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(PromotionStatus))]
        public PromotionStatus IsActive { get; set; }
    }
    public class PromotionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public PromotionType PromotionType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string PromotionCode { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionTitle")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string PromotionTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionValue")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal PromotionValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPercent")]
        public bool IsPercent { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        //[DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public PromotionStatus IsActive { get; set; }
    }
    public class PromotionEditEntry : PromotionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PromotionId { get; set; }
    }
}
