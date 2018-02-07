using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.Dtos.Business
{
    public class CompanyEntry : DtoBase
    {
        public CompanyEntry()
        {
            HasChild = false;
            ParentId = 0;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(300, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Slogan { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Hotline")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Hotline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Telephone")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Telephone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SupportOnline")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string SupportOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TaxCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public CompanyStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public CompanyAddressEntry Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class CompanyEditEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(300, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Slogan { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Hotline")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Hotline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Telephone")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Telephone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SupportOnline")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string SupportOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TaxCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public CompanyStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public CompanyAddressEditEntry Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class CompanyDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyId")]
        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int? Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        public string Slogan { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Hotline")]
        public string Hotline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Telephone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SupportOnline")]
        public string SupportOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public CompanyStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }
    }
    public class CompanyInfoDetail : CompanyDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        public AddressInfoDetail Address { get; set; }
    }
    public class CompanyAddressEntry : AddressEntry
    {
        public CompanyAddressEntry()
        {
            AddressTypeId = AddressType.Company;
            CountryId = GlobalSettings.DefaultCountryId;
        }
    }
    public class CompanyAddressEditEntry : CompanyAddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }
    }
}
