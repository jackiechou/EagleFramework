using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductDiscountSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductDiscountStatus? IsActive { get; set; }
    }
    public class ProductDiscountEntry : DtoBase
    {
        public ProductDiscountEntry()
        {
            Quantity = 1;
            DiscountType = DiscountType.Normal;
            StartDate = DateTime.UtcNow;
            IsActive = ProductDiscountStatus.Active;
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
        [EnumDataType(typeof(ProductDiscountStatus))]
        public ProductDiscountStatus IsActive { get; set; }
    }
    public class ProductDiscountEditEntry : ProductDiscountEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int DiscountId { get; set; }

        public string StartDateText { get; set; }
        public string EndDateText { get; set; }
    }
    public class ProductDiscountDetail : BaseDto
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
        public ProductDiscountStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }
    }
}
