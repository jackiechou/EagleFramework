using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.Dtos.Business
{
    public class VendorSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string VendorName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsAuthorized")]
        public VendorStatus? IsAuthorized { get; set; }
    }
    public class VendorInfoDetail : VendorDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        public List<VendorAddressDetail> Addresses { get; set; }
    }
    public class VendorEntry : DtoBase
    {
        public VendorEntry()
        {
            Address = new VendorAddressEntry();
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string VendorName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StoreName")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AccountNumber")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string AccountNumber { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Slogan { get; set; }

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

        [Display(ResourceType = typeof(LanguageResource), Name = "Hotline")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Hotline { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "SupportOnline")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string SupportOnline { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "RefundPolicy")]
        public string RefundPolicy { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "ShoppingHelp")]
        public string ShoppingHelp { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "OrganizationalStructure")]
        public string OrganizationalStructure { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "FunctionalAreas")]
        public string FunctionalAreas { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ClickThroughs")]
        public decimal? ClickThroughs { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreditRating")]
        public int? CreditRating { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "TermsOfService")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TermsOfService { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Keywords { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Summary")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Summary { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsAuthorized")]
        public VendorStatus IsAuthorized { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public VendorAddressEntry Address { get; set; }

        public VendorCurrencyEntry Currency { get; set; }

    }
    public class VendorEditEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string VendorName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StoreName")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AccountNumber")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string AccountNumber { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Slogan { get; set; }

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

        [Display(ResourceType = typeof(LanguageResource), Name = "Hotline")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Hotline { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "SupportOnline")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string SupportOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ClickThroughs")]
        public decimal? ClickThroughs { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreditRating")]
        public int? CreditRating { get; set; }

        [AllowHtml]
        [Display(ResourceType = typeof(LanguageResource), Name = "TermsOfService")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TermsOfService { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Keywords { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Summary")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Summary { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "RefundPolicy")]
        public string RefundPolicy { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "ShoppingHelp")]
        public string ShoppingHelp { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "OrganizationalStructure")]
        public string OrganizationalStructure { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "FunctionalAreas")]
        public string FunctionalAreas { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsAuthorized")]
        public VendorStatus IsAuthorized { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public VendorAddressEditEntry Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Currency")]
        public VendorCurrencyEntry Currency { get; set; }
    }
    public class VendorDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorName")]
        public string VendorName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AccountNumber")]
        public string AccountNumber { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CopyRight")]
        public string CopyRight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SupportOnline")]
        public string SupportOnline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Hotline")]
        public string Hotline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Telephone")]
        public string Telephone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ClickThroughs")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? ClickThroughs { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "KeyWords")]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreditRating")]
        public int? CreditRating { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StoreName")]
        public string StoreName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Logo")]
        public int? Logo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Slogan")]
        public string Slogan { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TermsOfService")]
        public string TermsOfService { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Summary")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "RefundPolicy")]
        public string RefundPolicy { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShoppingHelp")]
        public string ShoppingHelp { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrganizationalStructure")]
        public string OrganizationalStructure { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FunctionalAreas")]
        public string FunctionalAreas { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsAuthorized")]
        public VendorStatus IsAuthorized { get; set; }

    }
    public class VendorAddressDetail : DtoBase
    {
        public int VendorAddressId { get; set; }
        public int VendorId { get; set; }
        public int AddressId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public AddressInfoDetail Address { get; set; }
    }

    public class VendorAddressEntry : DtoBase
    {
        public VendorAddressEntry()
        {
            AddressTypeId = AddressType.Vendor;
            CountryId = GlobalSettings.DefaultCountryId;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public AddressType AddressTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Street")]
        [StringLength(512, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string Street { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        [StringLength(10, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PostalCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        public int? CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }
    }
    public class VendorAddressEditEntry : VendorAddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }
    }

    public class VendorCurrencyEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencySymbol")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CurrencySymbol { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Decimals")]
        public int? Decimals { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DecimalSymbol")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string DecimalSymbol { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThousandSeparator")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string ThousandSeparator { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositiveFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PositiveFormat { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NegativeFormat")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string NegativeFormat { get; set; }
    }
    public class VendorCurrencyDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int VendorCurrencyId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencySymbol")]
        public string CurrencySymbol { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Decimals")]
        public int? Decimals { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DecimalSymbol")]
        public string DecimalSymbol { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThousandSeparator")]
        public string ThousandSeparator { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositiveFormat")]
        public string PositiveFormat { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NegativeFormat")]
        public string NegativeFormat { get; set; }
    }
}
