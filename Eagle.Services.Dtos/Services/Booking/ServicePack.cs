using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServicePackSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ServicePackName")]
        public string ServicePackName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int? CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ServicePackType")]
        public int? ServiceType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ServicePackStatus? Status { get; set; }
    }
    public class ServicePackInfoDetail : ServicePackDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        public decimal? DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        public decimal? TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ImageUrl")]
        public string FileUrl { get; set; }

        public ServiceCategoryDetail Category { get; set; }
        public ServicePackTypeDetail Type { get; set; }
        public ServicePeriodDetail Period { get; set; }
        public ServiceTaxRateDetail Tax { get; set; }
        public ServiceDiscountDetail Discount { get; set; }
        public ServicePackDurationDetail Duration { get; set; }
        public DocumentInfoDetail Document { get; set; }
        public IEnumerable<ServicePackOptionDetail> Options { get; set; }
        public IEnumerable<EmployeeInfoDetail> Employees { get; set; }
        public IEnumerable<ServicePackRatingDetail> Ratings { get; set; }
    }
    public class ServicePackDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageCode")]
        public string PackageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageName")]
        public string PackageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AvailableQuantity")]
        public int? AvailableQuantity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Capacity")]
        public int Capacity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DurationId")]
        public int? DurationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        public int? DiscountId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRateId")]
        public int? TaxRateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageFee")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? PackageFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalFee")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? TotalFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int? FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Specification")]
        public string Specification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Rating")]
        public decimal? Rating { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalViews")]
        public int? TotalViews { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ServicePackStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class ServicePackEntry : DtoBase
    {
        public ServicePackEntry()
        {
            Capacity = 1;
            PackageFee = 0;
            TotalFee = 0;
            TypeId = 1;
            PackageCode = Guid.NewGuid().ToString();
            Status = ServicePackStatus.Active;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "SelectCategory")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Capacity")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int Capacity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AvailableQuantity")]
        public int? AvailableQuantity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DurationId")]
        public int? DurationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountId")]
        public int? DiscountId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRateId")]
        public int? TaxRateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageCode")]
        public string PackageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PackageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "NetFee")]
        public decimal? PackageFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalFee")]
        public decimal? TotalFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CurrencyCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Specification")]
        public string Specification { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ServicePackStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Employees")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public List<int> SelectedProviders { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }

        public List<ServicePackOptionEntry> Options { get; set; }
    }
    public class ServicePackEditEntry : ServicePackEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PackageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PackageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LinkToImage")]
        public string FileUrl { get; set; }

        public int? Index { get; set; }
        public List<ServicePackOptionEditEntry> ExistedOptions { get; set; }
    }
}
