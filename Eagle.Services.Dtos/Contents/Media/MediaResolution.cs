using System.ComponentModel.DataAnnotations;
using Eagle.Resources;
using Newtonsoft.Json;
namespace Eagle.Services.Dtos.Contents.Media
{
    //[Bind(Exclude = "MediaResolutionId")]
    public class MediaResolution : DtoBase
    {
        [ScaffoldColumn(false)]
        [Display(ResourceType = typeof(LanguageResource), Name = "MediaResolutionId")]
        public int MediaResolutionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Percent")]
        public int Percent { get; set; }

        [JsonIgnore]
        [Display(ResourceType = typeof(LanguageResource), Name = "DeviceType")]
        public int DeviceType { get; set; }

        [JsonIgnore]
        [Display(ResourceType = typeof(LanguageResource), Name = "MediaType")]
        public int MediaType { get; set; }
    }
}
