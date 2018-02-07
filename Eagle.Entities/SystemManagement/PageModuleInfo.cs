using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class PageModuleInfo : Module
    {
        public int PageModuleId { get; set; }
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Pane")]
        public string Pane { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alignment")]
        public string Alignment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Border")]
        public string Border { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InsertedPosition")]
        public string InsertedPosition { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleOrder")]
        public int? ModuleOrder { get; set; }

        public bool? IsVisible { get; set; }
    }
}
