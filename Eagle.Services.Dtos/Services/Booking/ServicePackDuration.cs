using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Booking
{
    public class ServicePackDurationSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerName")]
        public string DurationName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? Status { get; set; }
    }
    public class ServicePackDurationEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DurationName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string DurationName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllotedTime")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int AllotedTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class ServicePackDurationEditEntry : ServicePackDurationEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DurationId")]
        public int DurationId { get; set; }
    }
    public class ServicePackDurationDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "DurationId")]
        public int DurationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DurationName")]
        public string DurationName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AllotedTime")]
        public int AllotedTime { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Unit")]
        public string Unit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
}
