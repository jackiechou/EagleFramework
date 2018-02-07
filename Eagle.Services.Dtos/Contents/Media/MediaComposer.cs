using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Media
{
    public class MediaComposerSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaComposerStatus? Status { get; set; }
    }
    public class MediaComposerDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerId")]
        public int ComposerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerName")]
        public string ComposerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImage")]
        public string FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImage")]
        public string MainImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public MediaComposerStatus Status { get; set; }
    }
    public class MediaComposerEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ComposerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int Photo { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public MediaComposerStatus Status { get; set; }
    }
    public class MediaComposerEditEntry : MediaComposerEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ComposerId { get; set; }
    }
}
