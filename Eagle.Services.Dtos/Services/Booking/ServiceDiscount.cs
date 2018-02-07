using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServiceDiscountSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(ServiceDiscountStatus))]
        public ServiceDiscountStatus? IsActive { get; set; }
    }
    public class ServiceDiscountEntry : DtoBase
    {
        public ServiceDiscountEntry()
        {
            DiscountType = DiscountType.Normal;
            Quantity = 1;
            StartDate = DateTime.UtcNow;
            IsActive = ServiceDiscountStatus.Active;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountCode")]
        public string DiscountCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountType")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [EnumDataType(typeof(DiscountType))]
        public DiscountType DiscountType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Quantity")]
        public int? Quantity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPercent")]
        public bool IsPercent { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(ServiceDiscountStatus))]
        public ServiceDiscountStatus IsActive { get; set; }
    }
    public class ServiceDiscountEditEntry : ServiceDiscountEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int DiscountId { get; set; }
    }
    public class ServiceDiscountDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        public int DiscountId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountCode")]
        public string DiscountCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountType")]
        public DiscountType DiscountType { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "Quantity")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public int? Quantity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPercent")]
        public bool IsPercent { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ServiceDiscountStatus IsActive { get; set; }
    }
}
