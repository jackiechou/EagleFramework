using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ManufacturerEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ManufacturerName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ManufacturerStatus IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class ManufacturerEditEntry : ManufacturerEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ManufacturerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }
    public class ManufacturerSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ManufacturerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ManufacturerStatus? IsActive { get; set; }
    }
    public class ManufacturerDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerId")]
        public int ManufacturerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerName")]
        public string ManufacturerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(ManufacturerStatus))]
        public ManufacturerStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }
    }
    public class ManufacturerInfoDetail : ManufacturerDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }
    public class ManufacturerCategorySearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ManufacturerCategoryStatus? IsActive { get; set; }
    }
    public class ManufacturerCategoryEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CategoryName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ManufacturerCategoryStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class ManufacturerCategoryEditEntry : ManufacturerCategoryEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
    }
    public class ManufacturerCategoryDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ManufacturerCategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectCategory")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CategoryName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(ManufacturerCategoryStatus))]
        public ManufacturerCategoryStatus IsActive { get; set; }
    }
}
