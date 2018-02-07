using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class VendorPartnerEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PartnerName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PartnerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Telephone")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Telephone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsAuthorized")]
        public bool Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class VendorPartnerEditEntry : VendorPartnerEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PartnerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PartnerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }
    public class VendorPartnerDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PartnerId")]
        public int PartnerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PartnerName")]
        public string PartnerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        public string Slogan { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Telephone")]
        public string Telephone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class VendorPartnerInfoDetail : VendorPartnerDetail
    {
        public string FileUrl { get; set; }
    }
    public class VendorPartnerSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PartnerName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PartnerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? Status { get; set; }
    }
}
