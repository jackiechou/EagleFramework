using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement;

namespace Eagle.Services.Dtos.Business
{
    public class CustomerSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeId")]
        public int CustomerTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CustomerStatus? IsActive { get; set; }
    }
    public class CustomerEntry : DtoBase
    {
        public CustomerEntry()
        {
            CustomerTypeId = 1;
            Gender = Sex.NoneSpecified;
            IsActive = CustomerStatus.Published;
        }
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CustomerTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerNo")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CustomerNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardNo")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IdentificationNumber")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string IdCardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PassPortNo")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PassPortNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BirthDay")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BirthDay { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HomePhone")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string HomePhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkPhone")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string WorkPhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Gender")]
        public Sex Gender { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CustomerStatus IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }
        public CustomerAddressEntry Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class CustomerEditEntry : DtoBase
    {
        public CustomerEditEntry()
        {
            CustomerTypeId = 1;
            Gender = Sex.NoneSpecified;
            IsActive = CustomerStatus.Published;
        }
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CustomerTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerNo")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CustomerNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardNo")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IdentificationNumber")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string IdCardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PassPortNo")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PassPortNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Gender")]
        public Sex Gender { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BirthDay")]
        public DateTime? BirthDay { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HomePhone")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string HomePhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkPhone")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string WorkPhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CustomerStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FileUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }
        public CustomerAddressEditEntry Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class CustomerDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeId")]
        public int CustomerTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerNo")]
        public string CustomerNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardNo")]
        public string CardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        public string LastName { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IdCardNo")]
        public string IdCardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PassPortNo")]
        public string PassPortNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxCode")]
        public string TaxCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Gender")]
        public Sex Gender { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BirthDay")]
        public DateTime? BirthDay { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HomePhone")]
        public string HomePhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WorkPhone")]
        public string WorkPhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PasswordSalt")]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Verified")]
        public bool Verified { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CustomerStatus IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int? VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        public int? AddressId { get; set; }
    }
    public class CustomerInfoDetail : CustomerDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        public AddressInfoDetail Address { get; set; }
    }
    public class CustomerChangePassword : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OldPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string OldPassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NewPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string NewPassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RetypeNewPassword")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ConfirmedPassword { get; set; }
    }
    public class CustomerAddressEntry : AddressEntry
    {
        public CustomerAddressEntry()
        {
            AddressTypeId = AddressType.Customer;
            CountryId = GlobalSettings.DefaultCountryId;
        }
    }
    public class CustomerAddressEditEntry : CustomerAddressEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AddressId")]
        // [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? AddressId { get; set; }
    }
    public class CustomerRegisterEntry : DtoBase
    {
        public CustomerRegisterEntry()
        {
            PhoneType = PhoneType.Mobile;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 1)]
        public string LastName { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "PhoneType")]
        public PhoneType PhoneType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
       // [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        public string PasswordSalt { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ConfirmedPassword")]
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        [System.ComponentModel.DataAnnotations.Compare("PasswordSalt", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ValidatePassword")]
        public string ConfirmedPassword { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BirthDay")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BirthDay { get; set; }

        public CustomerRegisterAddressEntry Address { get; set; }
    }

    public class CustomerRegisterAddressEntry
    {
        public CustomerRegisterAddressEntry()
        {
            AddressTypeId = AddressType.Customer;
            CountryId = GlobalSettings.DefaultCountryId;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "AddressTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public AddressType AddressTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Street")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(512, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Street { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(10, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PostalCode { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }
    }
    public class CustomerLogin : DtoBase
    {
        public CustomerLogin()
        {
            RememberMe = false;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"(\S)+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "WhiteSpaceIsNotAllowed")]
        [StringLength(128, MinimumLength = 3)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RememberMe")]
        public bool RememberMe { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DesiredUrl")]
        public string DesiredUrl { get; set; }
    }
    public class CustomerTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeId")]
        public int CustomerTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeName")]
        public string CustomerTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionalRate")]
        public int? PromotionalRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Note")]
        public string Note { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CustomerTypeStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }
    }
    public class CustomerTypeEditEntry : CustomerTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CustomerTypeId { get; set; }
    }
    public class CustomerTypeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CustomerTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionalRate")]
        //[Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "GreaterThanZero")]
        public int? PromotionalRate { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Note")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Note { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(CustomerTypeStatus))]
        public CustomerTypeStatus IsActive { get; set; }
    }
    public class CustomerTypeSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerTypeName")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CustomerTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public CustomerTypeStatus? IsActive { get; set; }
    }
}
