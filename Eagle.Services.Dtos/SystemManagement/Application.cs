using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ApplicationEntry : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ApplicationName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DefaultLanguage")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(10, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string DefaultLanguage { get; set; }

        public string HomeDirectory { get; set; }
        public string Currency { get; set; }
        public string TimeZoneOffset { get; set; }
        public string Url { get; set; }
        public string LogoFile { get; set; }
        public string BackgroundFile { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "KeyWords")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string KeyWords { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CopyRight { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "FooterText")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FooterText { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        public int HostSpace { get; set; }
        public double HostFee { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiredDate")]
        public DateTime? ExpiryDate { get; set; }
        public Guid? RegisteredUserId { get; set; }
    }
    public class ApplicationDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationName")]
        public string ApplicationName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DefaultLanguage")]
        public string DefaultLanguage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HomeDirectory")]
        public string HomeDirectory { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Currency")]
        public string Currency { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TimeZoneOffset")]
        public string TimeZoneOffset { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LogoFile")]
        public string LogoFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BackgroundFile")]
        public string BackgroundFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "KeyWords")]
        public string KeyWords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FooterText")]
        public string FooterText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HostSpace")]
        public int? HostSpace { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HostFee")]
        public double? HostFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public Guid? RegisteredUserId { get; set; }
    }


    public class ApplicationLanguageDetail : DtoBase
    {
        public int ApplicationLanguageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public ApplicationLanguageStatus Status { get; set; }

        public LanguageDetail Language { get; set; }
    }
    public class ApplicationLanguageEditEntry : ApplicationLanguageEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int LanguageId { get; set; }
    }
    public class ApplicationLanguageEntry : DtoBase
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string LanguageCode { get; set; }
    }
    public class ApplicationLanguageListEntry : DtoBase
    {
        public List<string> SelectedLanguages { get; set; }
    }
}
