using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.SystemManagement.Identity
{
    public class ContactEntry : DtoBase
    {
        //[Display(ResourceType = typeof (LanguageResource), Name = "Title")]
        //[StringLength(256, MinimumLength = 2, ErrorMessageResourceType = typeof (LanguageResource),
        //    ErrorMessageResourceName = "MinMaxTitleLength")]
        //[RegularExpression(@"[a-zA-Z0-9 \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+",
        //    ErrorMessageResourceType = typeof (LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        //public string Title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayName")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Sex")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [EnumDataType(typeof(SexType))]
        public SexType Sex { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        //[DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LinePhone1")]
        [MaxLength(17, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string LinePhone1 { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LinePhone2")]
        [MaxLength(17, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string LinePhone2 { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [MaxLength(15, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Dob")]
        public DateTime? Dob { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JobTitle")]
        [MaxLength(256, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string JobTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IDNo")]
        public string IdNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IssuedDate")]
        public DateTime? IdIssuedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxNo")]
        public string TaxNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FullName")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUpload")]
        public HttpPostedFileBase FileUpload { get; set; }
    }

    public class ContactEditEntry : ContactEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContactId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ContactId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
    }
    public class ContactDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ContactId")]
        public int ContactId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayName")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Sex")]
        public SexType Sex { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "JobTitle")]
        public string JobTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Dob")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dob { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Photo")]
        public int? Photo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LinePhone1")]
        public string LinePhone1 { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LinePhone2")]
        public string LinePhone2 { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Website")]
        public string Website { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IdNo")]
        public string IdNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IssuedDate")]
        public DateTime? IdIssuedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxNo")]
        public string TaxNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedOn")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ModifiedOn")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }
    }

    public class ContactInfoDetail : ContactDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FullName")]
        public string FullName { get; set; }
        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }
        public DocumentInfoDetail DocumentInfo { get; set; }
    }
}
