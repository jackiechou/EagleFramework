using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageModule")]
    public class PageModule : EntityBase
    {
        public PageModule()
        {
            IsVisible = true;
        }
     
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageModuleId { get; set; }

        [Key]
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }

        [Key]
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int ModuleId { get; set; }


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

        [Display(ResourceType = typeof(LanguageResource), Name = "IsVisible")]
        public bool? IsVisible { get; set; }

        //[ForeignKey("PageId")]
        //public virtual Page Page { get; set; }

        //[ForeignKey("ModuleId")]
        //public virtual ICollection<Module> Modules { get; set; }
    }
}
