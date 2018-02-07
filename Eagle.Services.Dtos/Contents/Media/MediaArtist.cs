using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Media
{
    public class MediaArtistEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ArtistName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaArtistStatus Status { get; set; }
    }
    public class MediaArtistEditEntry : MediaArtistEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ArtistId { get; set; }
    }

    public class MediaArtistSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaArtistStatus? Status { get; set; }
    }
    public class MediaArtistDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistId")]
        public int ArtistId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistName")]
        public string ArtistName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ArtistAlias")]
        public string ArtistAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public MediaArtistStatus Status { get; set; }
    }
}
