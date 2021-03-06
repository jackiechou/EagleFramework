﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServiceTaxRateSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }
    public class ServiceTaxRateDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRateId")]
        public int TaxRateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPercent")]
        public bool IsPercent { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class ServiceTaxRateEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPercent")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsPercent { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class ServiceTaxRateEditEntry : ServiceTaxRateEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRateId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TaxRateId { get; set; }
    }


}
