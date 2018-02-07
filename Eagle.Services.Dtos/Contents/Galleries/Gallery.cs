using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Galleries
{
    public class GallerySearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TopicId")]
        public string SearchTopicCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CollectionId")]
        public int? SearchCollectionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public GalleryFileStatus? SearchStatus { get; set; }
    }
}
