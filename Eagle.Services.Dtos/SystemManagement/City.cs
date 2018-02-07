using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class CityDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CityId")]
        public int CityId { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "CityName")]
        public string CityName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
    public class CityEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CityName")]
        public string CityName { get; set; }
    }
}
