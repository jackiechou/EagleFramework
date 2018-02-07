using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    public class HostSettingDetail: DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SettingName")]
        public string SettingName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SettingValue")]
        public string SettingValue { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool IsSecure { get; set; }
    }
}
