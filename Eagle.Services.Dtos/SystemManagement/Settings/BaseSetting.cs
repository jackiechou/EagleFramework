using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement.Settings
{
    public class BaseSettingDetail : DtoBase
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string KeyName { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string KeyValue { get; set; }
    }

    public class SettingItemDetail : ApplicationSettingDetail
    {
        public BaseSettingDetail Setting { get; set; }
    }


    public class SettingItemEditEntry : DtoBase
    {
        public ApplicationSettingCustomEntry ApplicationSetting { get; set; }
        public List<BaseSettingDetail> Settings { get; set; }
    }


    #region Payal Settings
    public class PaypalSettingDetail : ApplicationSettingDetail
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Mode { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ClientId { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ClientSecret { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ConnectionTimeout { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string RequestRetries { get; set; }
    }

    public class PaypalSettingEntry : DtoBase
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Mode { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ClientId { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ClientSecret { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ConnectionTimeout { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string RequestRetries { get; set; }
    }

    public class PaypalSettingEditEntry : DtoBase
    {
        public ApplicationSettingCustomEntry ApplicationSetting { get; set; }

        public PaypalSettingEntry PaypalSetting { get; set; }
    }
    #endregion
    
}
