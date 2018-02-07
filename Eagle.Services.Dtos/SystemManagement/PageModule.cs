using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class PageModuleEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Pane")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Pane { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alignment")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Alignment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Border")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Border { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InsertedPosition")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string InsertedPosition { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public int IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleOrder")]
        public int? ModuleOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsVisible")]
        public bool? IsVisible { get; set; }
    }
    public class PageModuleEditEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Pane")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Pane { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alignment")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Alignment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Border")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Border { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "InsertedPosition")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string InsertedPosition { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public int IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleOrder")]
        public int? ModuleOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsVisible")]
        public bool? IsVisible { get; set; }
    }
    public class PageModuleDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageModuleId")]
        public int PageModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleId")]
        public int ModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
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

        [Display(ResourceType = typeof(LanguageResource), Name = "ReferencedModuleId")]
        public int? ReferencedModuleId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModuleOrder")]
        public int? ModuleOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsVisible")]
        public bool? IsVisible { get; set; }
    }
}
