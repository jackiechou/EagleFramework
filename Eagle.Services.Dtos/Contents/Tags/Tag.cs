using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;
using Newtonsoft.Json;

namespace Eagle.Services.Dtos.Contents.Tags
{
    public class SearchTagEntry : DtoBase
    {
        public string TagName { get; set; }
        public TagType TagTypeId { get; set; }
    }

    public class TagEntry : DtoBase
    {
        public TagType TagType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, MinimumLength = 3, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength")]
        public string TagName { get; set; }

        public TagStatus TagStatus { get; set; }
    }

    public class TagDetail : DtoBase
    {
        [JsonIgnore]
        public int TagId { get; set; }
        public int TagTypeId { get; set; }
        public string TagName { get; set; }
        public TagStatus Status { get; set; }
    }
}
