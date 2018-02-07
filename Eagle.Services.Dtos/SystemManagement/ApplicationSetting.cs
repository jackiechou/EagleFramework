using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ApplicationSettingDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SettingId")]
        public int SettingId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SettingName")]
        public string SettingName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SettingValue")]
        public string SettingValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ApplicationSettingStatus IsActive { get; set; }
    }

    public class ApplicationSettingCustomEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SettingId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int SettingId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SettingName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SettingName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ApplicationSettingStatus IsActive { get; set; }
    }

    public class ApplicationSettingEditEntry : ApplicationSettingEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SettingId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int SettingId { get; set; }
    }

    public class ApplicationSettingEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SettingName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SettingName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SettingValue")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string SettingValue { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ApplicationSettingStatus IsActive { get; set; }
    }
}
